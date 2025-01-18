using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using yoyokity.Common;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.GCD;

public class Aoe_移动中 : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("AOE")) return -1;
        if (!Data.Spells.失衡2.GetSpell().IsUnlock()) return -2;

        var aoeCount = TargetHelper.GetNearbyEnemyCount(Core.Me, 0, 5);
        if (aoeCount < 1) return -3;

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Helper.GetActionChange(Data.Spells.失衡2).GetSpell());
    }
}