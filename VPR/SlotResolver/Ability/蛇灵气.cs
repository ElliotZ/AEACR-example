using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;

namespace yoyokity.VPR.SlotResolver.Ability;

public class 蛇灵气 : ISlotResolver
{
    public int Check()
    {
        //好了就打
        if (!Data.Spells.蛇灵气.GetSpell().IsReadyWithCanCast()) return -1;
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.蛇灵气.GetSpell());
    }
}