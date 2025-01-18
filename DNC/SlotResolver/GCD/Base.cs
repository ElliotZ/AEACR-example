using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.GCD;

public class Base : ISlotResolver
{
    public int Check()
    {
        return 0;
    }

    private static uint GetSpell()
    {
        var enemyCount = TargetHelper.GetNearbyEnemyCount(5);

        //AOE连击
        if (enemyCount >= 3 && Qt.Instance.GetQt("AOE"))
            return Core.Resolve<MemApiSpell>().GetLastComboSpellId() == Data.Spells.风车
                ? Data.Spells.落刃雨
                : Data.Spells.风车;

        return Core.Resolve<MemApiSpell>().GetLastComboSpellId() == Data.Spells.瀑泻
            ? Data.Spells.喷泉
            : Data.Spells.瀑泻;
    }

    public void Build(Slot slot)
    {
        slot.Add(GetSpell().GetSpell());
    }
}