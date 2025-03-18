using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.AILoop;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using yoyokity.Common;
using yoyokity.DNC.QtUI;
using Data = yoyokity.DNC.SlotResolver.Data;
using Task = System.Threading.Tasks.Task;

namespace yoyokity.DNC;

public class EventHandler : IRotationEventHandler
{
    private static int 舞步步骤 => Core.Resolve<JobApi_Dancer>().CompleteSteps;
    private static bool 大舞ing => Core.Me.HasAura(Data.Buffs.正在大舞);
    private static bool 小舞ing => Core.Me.HasAura(Data.Buffs.正在小舞);

    public void OnResetBattle() //重置战斗
    {
        BattleData.Instance = new BattleData();
    }

    public async Task OnNoTarget() //进战且无目标时
    {
        //战斗时间10秒以上
        if (AI.Instance.BattleData.CurrBattleTimeInMs < 10 * 1000) return;

        //小舞
        if (Qt.Instance.GetQt("小舞") &&
            Qt.Instance.GetQt("无目标小舞") &&
            Data.Spells.标准舞步.GetSpell().Cooldown.TotalMilliseconds == 0 &&
            !Core.Me.HasAura(Data.Buffs.结束动作预备))
        {
            var slot = new Slot();
            slot.Add(Data.Spells.标准舞步.GetSpell());
            await slot.Run(AI.Instance.BattleData, false);
        }

        //大舞
        if (Qt.Instance.GetQt("大舞") &&
            Qt.Instance.GetQt("无目标大舞") &&
            Data.Spells.大舞.GetSpell().Cooldown.TotalMilliseconds == 0 &&
            !Core.Me.HasAura(Data.Buffs.提拉纳预备))
        {
            var slot = new Slot();
            slot.Add(Data.Spells.大舞.GetSpell());
            await slot.Run(AI.Instance.BattleData, false);
        }

        //舞步
        if (Core.Resolve<JobApi_Dancer>().IsDancing)
        {
            if (大舞ing && 舞步步骤 == 4) return;
            if (小舞ing && 舞步步骤 == 2) return;

            var skill = Core.Resolve<JobApi_Dancer>().NextStep;
            var slot = new Slot
            {
                Wait2NextGcd = true
            };
            slot.Add(skill.GetSpell());
            await slot.Run(AI.Instance.BattleData, false);
        }
    }

    public void OnSpellCastSuccess(Slot slot, Spell spell) //施法成功判定可以滑步时
    {
    }

    public async Task OnPreCombat() //脱战时
    {
        await Task.CompletedTask;
    }


    public void AfterSpell(Slot slot, Spell spell)
        //某个技能使用之后的处理,比如诗人在刷Dot之后记录这次是否是强化buff的Dot 如果是读条技能，则是服务器判定它释放成功的时间点，比上面的要慢一点
    {
        //记录复唱时间
        var d = Core.Resolve<MemApiSpell>().GetGCDDuration(true);
        if (d > 0) BattleData.Instance.GcdDuration = d;

        //大舞小舞后只打一个能力技
        if (spell.Id is Data.Spells.四色大舞结束 or Data.Spells.双色标准舞步结束)
            AI.Instance.BattleData.CurrGcdAbilityCount = 1;
    }

    public void OnBattleUpdate(int currTime) //战斗中逐帧检测
    {
        //4人本自动绑舞伴
        if (DncSettings.Instance.AutoPartner && Data.Spells.给舞伴.IsUnlock())
        {
            if (currTime > 1000 && !Core.Me.HasAura(Data.Buffs.舞伴buff) && !Core.Resolve<JobApi_Dancer>().IsDancing &&
                PartyHelper.Party.Count <= 4 && BattleData.Instance.上次自动舞伴时间 + 5000 < currTime)
            {
                自动舞伴();
                BattleData.Instance.上次自动舞伴时间 = currTime;
            }
        }
    }

    private static void 自动舞伴()
    {
        var target = PartyHelper.Party.FirstOrDefault(p => p.IsDps() && p != Core.Me);
        if (target == null) return;
        舞伴hotkeyWindow.DancePartnerPanel.SetHotkey("切换舞伴: " + target.Name);
    }

    public void OnEnterRotation() //切换到当前ACR
    {
        LogHelper.Print(
            "欢迎使用yoyo舞者ACR，使用说明请看：https://discord.gg/PvxfM6ZPxw");
        Core.Resolve<MemApiChatMessage>()
            .Toast2("跟我念：傻逼riku和souma的妈死干净咯！", 1, 5000);


        //检查全局设置
        if (!Helper.GlobalSettings.NoClipGCD3)
            LogHelper.PrintError("建议在acr全局设置中勾选【全局能力技不卡GCD】选项");

        //更新时间轴
        if (DncSettings.Instance.AutoUpdataTimeLines)
            TimeLineUpdater.UpdateFiles(Helper.DncTimeLineUrl);
    }

    public void OnExitRotation() //退出ACR
    {
    }

    public void OnTerritoryChanged()
    {
    }
}