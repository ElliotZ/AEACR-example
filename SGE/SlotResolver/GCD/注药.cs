using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.Common;

namespace yoyokity.SGE.SlotResolver.GCD;

public class 注药 : ISlotResolver
{
    public int Check()
    {
        if (Helper.IsMove && !Core.Me.HasAura(Data.Buffs.即刻)) return -1;
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Helper.GetActionChange(Data.Spells.注药).GetSpell());
    }
}