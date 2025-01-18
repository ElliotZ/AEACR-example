using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;

namespace yoyokity.DNC.SlotResolver.GCD;

public class 流星舞 : ISlotResolver
{
    public int Check()
    {
        if (!Core.Me.HasAura(Data.Buffs.流星舞预备)) return -1;
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.流星舞.GetSpell());
    }
}