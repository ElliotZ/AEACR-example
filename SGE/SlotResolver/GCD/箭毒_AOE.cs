using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.GCD;

public class 箭毒_AOE : ISlotResolver
{
    private IBattleChara? Target { get; set; }

    public int Check()
    {
        if (!Qt.Instance.GetQt("箭毒")) return -1;
        if (!Data.Spells.箭毒.GetSpell().IsUnlock()) return -1;
        if (SgeHelper.红豆 <= 0) return -2;

        Target = TargetHelper.GetMostCanTargetObjects(Data.Spells.箭毒adaptive, 2);
        if (Target == null) return -3;

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.箭毒adaptive.GetSpell(Target!));
    }
}