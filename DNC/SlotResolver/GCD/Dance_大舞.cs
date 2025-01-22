using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.GCD;

public class Dance_大舞 : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("大舞")) return -1;
        if (!Data.Spells.大舞.IsUnlock()) return -1;
        if (Data.Spells.大舞.GetSpell().Cooldown.TotalMilliseconds > 2000) return -2;
        return 1;
    }

    public void Build(Slot slot)
    {
        AI.Instance.BattleData.CurrGcdAbilityCount = 1;
        slot.Add(Data.Spells.大舞.GetSpell());
    }
}