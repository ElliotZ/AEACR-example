using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;

namespace yoyokity.SGE.SlotResolver.Ability;

public class 醒梦 : ISlotResolver
{
    public int Check()
    {
        if (Core.Me.IsCasting) return -1;
        if (!Data.Spells.醒梦.GetSpell().IsReadyWithCanCast()) return -2;
        if (AI.Instance.BattleData.HighPrioritySlots_OffGCD.Count > 0) return -2;
        
        if (Core.Me.CurrentMpPercent() > SgeSettings.Instance.醒梦阈值 / 100f) return -3;
        
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.醒梦.GetSpell());
    }
}