using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using yoyokity.Common;
using yoyokity.DNC.QtUI;

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
        var target = Data.Spells.扇舞终.最优aoe目标(2);
        var spell = !Qt.Instance.GetQt("AOE") || target == null
            ? Data.Spells.扇舞终.GetSpell()
            : Data.Spells.扇舞终.GetSpell(target);
        slot.Add(spell);
    }
}