using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using yoyokity.Common;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.GCD;

public class Aoe : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("AOE")) return -1;
        if (!Data.Spells.失衡adaptive.GetSpell().IsUnlock()) return -2;

        var aoeCount = TargetHelper.GetNearbyEnemyCount(Core.Me, 0, 5);

        if (Core.Me.Level < 94)
        {
            if (aoeCount < 2) return -3;
        }
        else
        {
            if (aoeCount < 3) return -3;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Helper.GetActionChange(Data.Spells.失衡adaptive).GetSpell());
    }
}