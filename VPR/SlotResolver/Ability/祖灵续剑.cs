using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;

namespace yoyokity.VPR.SlotResolver.Ability;

public class 祖灵续剑 : ISlotResolver
{
    public int Check()
    {
        if (Core.Me.Distance(Core.Me.GetCurrTarget()!) >
            SettingMgr.GetSetting<GeneralSettings>().AttackRange + 2) return -1;

        //不在祖灵连内不打
        if (VprHelper.祖灵连步骤 < 1) return -2;
        
        return 0;
    }

    private static uint GetSpell()
    {
        return VprHelper.祖灵连步骤 switch
        {
            4 => Data.Spells.祖灵之蛇一式,
            3 => Data.Spells.祖灵之蛇二式,
            2 => Data.Spells.祖灵之蛇三式,
            1 => Data.Spells.祖灵之蛇四式,
            _ => 0
        };
    }

    public void Build(Slot slot)
    {
        slot.Add(GetSpell().GetSpell());
    }
}