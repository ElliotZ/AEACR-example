using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.GCD;

public class Base : ISlotResolver
{
    private static uint 上个连击 => Core.Resolve<MemApiSpell>().GetLastComboSpellId();

    public int Check()
    {
        return 0;
    }

    private static uint GetSpells()
    {
        var enemyCount = TargetHelper.GetNearbyEnemyCount(5);

        if (Qt.Instance.GetQt("AOE") && Data.Spells.落刃雨.IsUnlock() &&
            上个连击 == Data.Spells.风车 && enemyCount >= 2)
            return Data.Spells.落刃雨;

        if (Qt.Instance.GetQt("AOE") && Data.Spells.风车.IsUnlock() &&
            enemyCount >= 3)
            return Data.Spells.风车;

        if (Data.Spells.喷泉.IsUnlock() && 上个连击 == Data.Spells.瀑泻)
            return Data.Spells.喷泉;

        return Data.Spells.瀑泻;
    }

    public void Build(Slot slot)
    {
        slot.Add(GetSpells().GetSpell());
    }
}