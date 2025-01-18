using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.Ability;

public class 百花 : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("百花")) return -1;
        if (!Data.Spells.百花.GetSpell().IsReadyWithCanCast()) return -1;

        //倾泻资源qt
        if (Qt.Instance.GetQt("倾泻资源")) return 5;

        //大舞前44秒不打
        if (Data.Spells.大舞.GetSpell().Cooldown.TotalMilliseconds <= 44000) return -2;

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.百花.GetSpell());
    }
}