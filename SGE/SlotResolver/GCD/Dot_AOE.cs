using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.Define;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.GCD;

public class Dot_AOE : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("AOE")) return -1;
        if (!Qt.Instance.GetQt("Dot")) return -1;
        if (!Data.Spells.均衡失衡.GetSpell().IsUnlock()) return -2;

        //5秒内不连续打
        if (Data.Spells.均衡失衡.GetSpell().RecentlyUsed(5000)) return -5;

        var aoeCount = TargetHelper.GetNearbyEnemyCount(Core.Me, 0, 5);

        if (Core.Me.Level < 94)
        {
            if (aoeCount < 2) return -3;
        }
        else
        {
            if (aoeCount < 3) return -3;
        }

        //检查周围人的buff情况
        var list = TargetMgr.Instance.EnemysIn12;
        var count = list.Count(v =>
            Core.Me.Distance(v.Value, DistanceMode.IgnoreTargetHitbox | DistanceMode.IgnoreHeight) <= 5 &&
            Core.Resolve<MemApiBuff>().GetAuraTimeleft(v.Value, Data.Buffs.均衡失衡, true) <= 3000);
        if (count < 2) return -4;

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.均衡.GetSpell());
        slot.Add(Data.Spells.均衡失衡.GetSpell());
    }
}