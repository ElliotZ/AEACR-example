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

        //距离判断
        if (Core.Me.Distance(Core.Me.GetCurrTarget()!) >
            SettingMgr.GetSetting<GeneralSettings>().AttackRange + 3) return -2;

        //开局3个gcd内不打
        if (AI.Instance.BattleData.CurrBattleTimeInMs <= 2500 * 3) return -3;

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Helper.GetActionChange(Data.Spells.发炎adaptive).GetSpell());
    }
}