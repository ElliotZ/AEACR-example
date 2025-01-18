using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.GCD;

public class 箭毒_移动中 : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("箭毒")) return -1;
        if (!Data.Spells.箭毒.GetSpell().IsUnlock()) return -1;
        if (SgeHelper.红豆 <= 0) return -2;

        if (Qt.Instance.GetQt("爆发药") && Qt.Instance.GetQt("保留1红豆") && SgeHelper.红豆 == 1) return -3;

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.箭毒adaptive.GetSpell());
    }
}