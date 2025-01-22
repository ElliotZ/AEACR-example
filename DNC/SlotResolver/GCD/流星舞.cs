using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.Common;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.GCD;

public class 流星舞 : ISlotResolver
{
    public int Check()
    {
        if (!Data.Spells.流星舞.IsUnlock()) return -1;
        if (!Core.Me.HasAura(Data.Buffs.流星舞预备)) return -1;
        return 0;
    }

    public void Build(Slot slot)
    {
        var target = Data.Spells.流星舞.最优aoe目标(2);
        var spell = !Qt.Instance.GetQt("AOE") || target == null
            ? Data.Spells.流星舞.GetSpell()
            : Data.Spells.流星舞.GetSpell(target);
        slot.Add(spell);
    }
}