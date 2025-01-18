using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;

namespace yoyokity.DNC.SlotResolver.Ability;

public class 扇舞终 : ISlotResolver
{
    public int Check()
    {
        if (!Data.Spells.扇舞终.GetSpell().IsReadyWithCanCast()) return -1;

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.扇舞终.GetSpell());
    }
}