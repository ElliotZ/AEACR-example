using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using yoyokity.Common;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.Ability;

public class 扇舞急 : ISlotResolver
{
    public int Check()
    {
        if (!Data.Spells.扇舞急.IsUnlock()) return -1;
        if (!Core.Me.HasAura(Data.Buffs.扇舞急预备)) return -1;
        if (Core.Resolve<JobApi_Dancer>().IsDancing) return -1;

        //大舞跳完1g内不打
        if (Data.Spells.四色大舞结束.RecentlyUsed(1500)) return -4;

        return 0;
    }

    public void Build(Slot slot)
    {
        var target = Data.Spells.扇舞急.最优aoe目标(2);
        var spell = !Qt.Instance.GetQt("AOE") || target == null
            ? Data.Spells.扇舞急.GetSpell()
            : Data.Spells.扇舞急.GetSpell(target);
        slot.Add(spell);
    }
}