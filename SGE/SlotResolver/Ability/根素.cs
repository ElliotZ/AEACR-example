using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.Ability;

public class 根素 : ISlotResolver
{
    public int Check()
    {
        if (Core.Me.IsCasting) return -1;
        if (!Qt.Instance.GetQt("根素")) return -1;
        if (!Data.Spells.根素.GetSpell().IsReadyWithCanCast()) return -2;
        if (AI.Instance.BattleData.HighPrioritySlots_OffGCD.Count > 0) return -2;

        //蓝豆满了不放
        if (SgeHelper.蓝豆 >= 2) return -3;

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.根素.GetSpell());
    }
}