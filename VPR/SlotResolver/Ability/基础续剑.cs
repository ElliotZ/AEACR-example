using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;

namespace yoyokity.VPR.SlotResolver.Ability;

public class 基础续剑 : ISlotResolver
{
    public int Check()
    {
        if (Core.Me.Distance(Core.Me.GetCurrTarget()!) >
            SettingMgr.GetSetting<GeneralSettings>().AttackRange + 2) return -1;

        if (!Data.Spells.蛇尾击.GetSpell().IsReadyWithCanCast()) return -2;
        
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.蛇尾击.GetSpell());
    }
}