using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.Common;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.GCD;

public class 发炎_移动中 : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("发炎")) return -1;
        if (BattleData.Instance.Lock发炎) return -1;
        if (!Data.Spells.发炎adaptive.GetSpell().IsReadyWithCanCast()) return -2;
        if (Qt.Instance.GetQt("保留1发炎") && Data.Spells.发炎adaptive.GetSpell().Charges is > 0 and < 2) return -6;

        //距离判断
        if (Core.Me.Distance(Core.Me.GetCurrTarget()!) >
            SettingMgr.GetSetting<GeneralSettings>().AttackRange + 3) return -2;

        //开局3个gcd内不打
        if (!SgeSettings.Instance.没妈妈自嗨打法 &&
            AI.Instance.BattleData.CurrBattleTimeInMs <= 2500 * 3) return -3;

        return 0;
    }

    public void Build(Slot slot)
    {
        var target = Data.Spells.发炎adaptive.最优aoe目标(2);
        var spell = !Qt.Instance.GetQt("AOE") || target == null
            ? Data.Spells.发炎adaptive.GetSpell()
            : Data.Spells.发炎adaptive.GetSpell(target);
        slot.Add(spell);
    }
}