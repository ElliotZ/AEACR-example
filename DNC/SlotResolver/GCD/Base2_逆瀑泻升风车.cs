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
        return Core.Me.HasAura(Data.Buffs.普通3预备) || Core.Me.HasAura(Data.Buffs.百花3预备) ? 0 : -1;
    }

    private static uint GetSpell()
    {
        var enemyCount = TargetHelper.GetNearbyEnemyCount(5);
        return enemyCount >= 3 && Qt.Instance.GetQt("AOE") ? Data.Spells.升风车 : Data.Spells.逆瀑泻;
    }

    public void Build(Slot slot)
    {
        slot.Add(GetSpell().GetSpell());
    }
}