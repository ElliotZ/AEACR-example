using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;

namespace yoyokity.DNC.SlotResolver.GCD;

public class 流星舞_高优先 : ISlotResolver
{
    public int Check()
    {
        if (!Data.Spells.流星舞.IsUnlock()) return -1;
        if (!Core.Me.HasAura(Data.Buffs.流星舞预备)) return -1;
        //流星舞buff快消了打
        if (DncHelper.AuraInGCDs(Data.Buffs.流星舞预备, 2)) return 1;
        return -2;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.流星舞.GetSpell());
    }
}