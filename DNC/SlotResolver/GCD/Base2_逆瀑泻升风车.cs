using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.GCD;

public class Base2_逆瀑泻升风车 : ISlotResolver
{
    public int Check()
    {
        if (!Data.Spells.逆瀑泻.IsUnlock()) return -1;
        if (!Core.Me.HasAura(Data.Buffs.普通3预备) && !Core.Me.HasAura(Data.Buffs.百花3预备)) return -1;

        return 0;
    }

    private static uint GetSpells()
    {
        var enemyCount = TargetHelper.GetNearbyEnemyCount(5);

        if (Qt.Instance.GetQt("AOE") && Data.Spells.升风车.IsUnlock() &&
            enemyCount >= 3)
            return Data.Spells.升风车;

        return Data.Spells.逆瀑泻;
    }

    public void Build(Slot slot)
    {
        slot.Add(GetSpells().GetSpell());
    }
}