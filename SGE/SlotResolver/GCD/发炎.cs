using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.Common;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.GCD;

public class 发炎 : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("发炎")) return -1;
        if (BattleData.Instance.Lock发炎) return -1;
        if (!Data.Spells.发炎adaptive.GetSpell().IsReadyWithCanCast()) return -2;

        //距离判断
        if (Core.Me.Distance(Core.Me.GetCurrTarget()!) >
            SettingMgr.GetSetting<GeneralSettings>().AttackRange + 3) return -2;

        //开局3个gcd内不打
        if (AI.Instance.BattleData.CurrBattleTimeInMs <= 2500 * 3) return -3;

        //在团辅中就打
        if (Helper.In团辅()) return 1;

        //在爆发药中打
        if (Core.Me.HasAura(49)) return 2;

        //倾泻资源
        if (Qt.Instance.GetQt("倾泻资源")) return 3;

        //快溢出了打
        if (Data.Spells.发炎adaptive.GetSpell().Cooldown.TotalMilliseconds < 2600) return 4;
        
        //周围多余一个人就打
        if (TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget()!, 6, 5) > 1) return 5;

        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.发炎adaptive.GetSpell());
    }
}