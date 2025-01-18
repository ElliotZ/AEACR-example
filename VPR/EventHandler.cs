using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using yoyokity.Common;
using yoyokity.VPR.QtUI;
using Task = System.Threading.Tasks.Task;
using Data = yoyokity.VPR.SlotResolver.Data;

namespace yoyokity.VPR;

public class EventHandler : IRotationEventHandler
{
    public void OnResetBattle() //重置战斗
    {
        BattleData.Instance = new BattleData();
    }

    public async Task OnNoTarget() //进战且无目标时
    {
        //战斗时间10秒以上
        if (AI.Instance.BattleData.CurrBattleTimeInMs < 10 * 1000) return;
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
        //记录基础连的复唱时间
        switch (spell.Id)
        {
            //基础连1比1记录
            case Data.Spells.A1 or Data.Spells.A2 or Data.Spells.A3红 or Data.Spells.A3绿 or
                Data.Spells.B1 or Data.Spells.B2 or Data.Spells.B3红 or Data.Spells.B3绿 or
                Data.Spells.A1Aoe or Data.Spells.A2Aoe or Data.Spells.A3Aoe or
                Data.Spells.B1Aoe or Data.Spells.B2Aoe or Data.Spells.B3Aoe:
            {
                var d = Core.Resolve<MemApiSpell>().GetGCDDuration(true);
                if (d > 0) BattleData.Instance.GcdDuration = d;
                break;
            }
            //蛇剑连 * 2.5 / 3
            case Data.Spells.蛇剑1 or Data.Spells.蛇剑1Aoe or
                Data.Spells.蛇剑攻速 or Data.Spells.蛇剑攻击Aoe or
                Data.Spells.蛇剑攻击 or Data.Spells.蛇剑攻速Aoe:
            {
                var d = Core.Resolve<MemApiSpell>().GetGCDDuration(true);
                if (d > 0) BattleData.Instance.GcdDuration = (int)(d * 2.5 / 3);
                break;
            }
        }
    }

    public void OnBattleUpdate(int currTime) //战斗中逐帧检测
    {
    }

    public void OnEnterRotation() //切换到当前ACR
    {
        LogHelper.Print(
            "欢迎使用yoyo蛇批ACR，使用说明请看：https://discord.gg/PvxfM6ZPxw");
        Core.Resolve<MemApiChatMessage>()
            .Toast2("跟我念：傻逼riku和souma的妈死干净咯！", 1, 5000);


        //检查全局设置
        if (!Helper.GlobalSettings.NoClipGCD3)
            LogHelper.PrintError("建议在acr全局设置中勾选【全局能力技不卡GCD】选项");
        
        MeleePosHelper2.Init(Qt.Instance,"真北");

        
        
        
        
        
        
        
        //更新时间轴
        // if (VprSettings.Instance.AutoUpdataTimeLines)
        //     TimeLineUpdater.UpdateFiles(Helper.DncTimeLineUrl);
    }

    public void OnExitRotation() //退出ACR
    {
    }

    public void OnTerritoryChanged()
    {
    }
}