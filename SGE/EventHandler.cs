using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Types;
using yoyokity.Common;
using Data = yoyokity.SGE.SlotResolver.Data;
using Task = System.Threading.Tasks.Task;

namespace yoyokity.SGE;

public class EventHandler : IRotationEventHandler
{
    public void OnResetBattle() //重置战斗
    {
        BattleData.Instance = new BattleData();
    }

    public async Task OnNoTarget() //进战且无目标时
    {
        await Task.CompletedTask;
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

        //某些技能后打一个能力技
        if (!Data.Spells.即刻咏唱.GetSpell().RecentlyUsed(2300) && spell.Id is
                Data.Spells.诊断 or Data.Spells.预后 or
                Data.Spells.注药 or Data.Spells.注药2 or Data.Spells.注药3 or
                Data.Spells.贤炮 or
                Data.Spells.均衡失衡 or Data.Spells.均衡 or
                Data.Spells.均衡注药 or Data.Spells.均衡注药2 or Data.Spells.均衡注药3 or
                Data.Spells.均衡诊断 or Data.Spells.均衡预后 or Data.Spells.均衡预后2
           )
            AI.Instance.BattleData.CurrGcdAbilityCount = 1;

        //自动心关
        if (SgeSettings.Instance.自动心关 &&
            !Data.Spells.心关.GetSpell().RecentlyUsed(10000) &&
            AI.Instance.BattleData.CurrBattleTimeInMs - BattleData.Instance.最近一次心关时间 >= 10000)
        {
            // 目标为敌人
            var target = Core.Me.GetCurrTarget();
            if (target != null && target.IsEnemy())
            {
                //目标的目标为队友，且为T
                var targetTarget = target.GetCurrTarget();
                if (targetTarget != null &&
                    PartyHelper.Party.Contains(targetTarget) &&
                    targetTarget.IsTank() &&
                    !targetTarget.HasLocalPlayerAura(Data.Buffs.关心))
                {
                    var _spell = Data.Spells.心关.GetSpell(target);
                    if (_spell.IsReadyWithCanCast())
                    {
                        BattleData.Instance.最近一次心关时间 = AI.Instance.BattleData.CurrBattleTimeInMs;
                        var time = RandomHelper.RandomInt(1000, 2000);
                        给心关(time, _spell);
                    }
                }
            }
        }
    }

    private static async Task 给心关(int time, Spell spell)
    {
        if (SgeSettings.Instance.TimeLinesDebug)
            LogHelper.Print("yoyo贤者", $"将在{time / 1000.0:F2}秒后对【{spell.SpecifyTarget?.Name}】使用心关");

        await Coroutine.Instance.WaitAsync(time);
        var slot = new Slot();
        slot.Add(spell);
        AI.Instance.BattleData.HighPrioritySlots_OffGCD.Enqueue(slot);
    }

    public void OnBattleUpdate(int currTime) //战斗中逐帧检测
    {
        //即刻走路
        if (SgeSettings.Instance.即刻移动 && Data.Spells.即刻咏唱.GetSpell().IsReadyWithCanCast() &&
            currTime - BattleData.Instance.上次即刻时间 > 5000)
        {
            if (MoveHelper.IsMoving() && GCDHelper.GetGCDCooldown() < 600)
            {
                AI.Instance.BattleData.NextSlot ??= new Slot();
                AI.Instance.BattleData.NextSlot.Add(Data.Spells.即刻咏唱.GetSpell());
                BattleData.Instance.上次即刻时间 = currTime;
            }
        }

        //等爆发药剩4S的时候开始补毒然后红豆或者即刻1
        if (Core.Me.HasAura(49))
        {
            if (currTime - BattleData.Instance.上次爆发药补毒时间 > 5000 &&
                Core.Me.GetCurrTarget()?.CurrentHp > Core.Me.GetCurrTarget()?.MaxHp * 0.01 &&
                Helper.目标Buff时间小于(Data.Buffs.均衡注药adaptive, 7000) &&
                Helper.Buff时间小于(49, 4500))
            {
                AI.Instance.BattleData.NextSlot = new Slot();
                AI.Instance.BattleData.NextSlot.Add(Data.Spells.均衡.GetSpell());
                AI.Instance.BattleData.NextSlot.Add(Data.Spells.均衡注药adaptive.GetSpell());

                if (Data.Spells.发炎adaptive.GetSpell().Cooldown.TotalMilliseconds < Helper.GetAuraTimeLeft(49))
                {
                    AI.Instance.BattleData.NextSlot.Add(Data.Spells.发炎adaptive.GetSpell());
                }
                else if (Data.Spells.即刻咏唱.GetSpell().IsReadyWithCanCast())
                {
                    AI.Instance.BattleData.NextSlot.Add(Data.Spells.即刻咏唱.GetSpell());
                    AI.Instance.BattleData.NextSlot.Add(Data.Spells.注药adaptive.GetSpell());
                }
                else if (SgeHelper.红豆 > 0)
                {
                    AI.Instance.BattleData.NextSlot.Add(Data.Spells.箭毒adaptive.GetSpell());
                }

                BattleData.Instance.上次爆发药补毒时间 = currTime;
            }
        }
    }

    public void OnEnterRotation() //切换到当前ACR
    {
        LogHelper.Print(
            "欢迎使用yoyo贤者ACR，使用说明请看：https://discord.gg/PvxfM6ZPxw");
        Core.Resolve<MemApiChatMessage>()
            .Toast2("跟我念：傻逼riku和souma的妈死干净咯！", 1, 5000);


        //检查全局设置
        if (!Helper.GlobalSettings.NoClipGCD3)
            LogHelper.PrintError("建议在acr全局设置中勾选【全局能力技不卡GCD】选项");
        if (Helper.GlobalSettings.ActionQueueInMs > 70)
            LogHelper.PrintError("建议在acr全局设置中设置【提前使用下一个gcd时间】为 70-50");

        //更新时间轴
        if (SgeSettings.Instance.AutoUpdataTimeLines)
            TimeLineUpdater.UpdateFiles(Helper.SgeTimeLineUrl);
    }

    public void OnExitRotation() //退出ACR
    {
    }

    public void OnTerritoryChanged()
    {
    }
}