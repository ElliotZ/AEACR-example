using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;

namespace yoyokity.VPR.SlotResolver.GCD;

public class 基础连AOE : ISlotResolver
{
    public int Check()
    {
        if (TargetHelper.GetNearbyEnemyCount(5) < 3) return -1;
        return 0;
    }

    public void Build(Slot slot)
    {
        throw new NotImplementedException();
    }
}