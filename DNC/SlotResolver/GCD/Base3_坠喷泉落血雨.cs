using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.GCD;

public class Base3_坠喷泉落血雨 : ISlotResolver
{
    public int Check()
    {
        return Core.Me.HasAura(Data.Buffs.普通4预备) || Core.Me.HasAura(Data.Buffs.百花4预备) ? 0 : -1;
    }

    private static uint GetSpell()
    {
        var enemyCount = TargetHelper.GetNearbyEnemyCount(5);
        return enemyCount >= 2 && Qt.Instance.GetQt("AOE") ? Data.Spells.落血雨 : Data.Spells.坠喷泉;
    }

    public void Build(Slot slot)
    {
        slot.Add(GetSpell().GetSpell());
    }
}