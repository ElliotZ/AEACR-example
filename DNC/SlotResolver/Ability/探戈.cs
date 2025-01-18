using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.Ability;

public class 探戈 : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("探戈")) return -1;
        if (!Data.Spells.探戈.GetSpell().IsReadyWithCanCast()) return -1;

        //倾泻资源qt
        if (Qt.Instance.GetQt("倾泻资源")) return 5;

        return -1;

        //非大舞中不打
        // if (!Data.Spells.四色大舞结束.RecentlyUsed(5000)) return -2;

        // return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.探戈.GetSpell());
    }
}