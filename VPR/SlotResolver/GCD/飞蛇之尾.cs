using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;

namespace yoyokity.VPR.SlotResolver.GCD;

public class 飞蛇之尾 : ISlotResolver
{
    public int Check()
    {
        if (VprHelper.蛇尾豆 <= 0) return -1;
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.飞蛇之尾.GetSpell());
    }
}