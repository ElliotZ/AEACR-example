using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Define;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.VPR.QtUI;

namespace yoyokity.VPR.SlotResolver.GCD;

public class 祖灵降临 : ISlotResolver
{
    public int Check()
    {
        //可释放
        if (!Data.Spells.祖灵降临.GetSpell().IsReadyWithCanCast()) return -2;

        //距离检测
        if (Core.Me.Distance(Core.Me.GetCurrTarget()!, DistanceMode.IgnoreHeight) > 5) return -1;

        //在蛇剑连中不打
        if (VprHelper.蛇剑连 != 0) return -4;

        //没buff不打
        if (!Core.Me.HasAura(Data.Buffs.B2) || !Core.Me.HasAura(Data.Buffs.A2)) return -6;

        //倾泻
        if (Qt.Instance.GetQt("倾泻资源")) return 1;

        //战斗开始五秒内不打
        if (AI.Instance.BattleData.CurrBattleTimeInMs < 5000) return -5;

        //有蛇灵气buff直接打
        if (Core.Me.HasAura(Data.Buffs.祖灵降临预备)) return 2;

        //刚打完一套祖灵连接着打
        if (Data.Spells.祖灵大蛇牙.GetSpell().RecentlyUsed(VprHelper.GetDreadGcdDuration) && VprHelper.祖灵力 >= 50)
            return 3;

        //平时80灵力
        if (VprHelper.祖灵力 < 80) return -3;

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.祖灵降临.GetSpell());
        MeleePosHelper2.Clear();
    }
}