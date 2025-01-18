using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;

namespace yoyokity.DNC.SlotResolver.Ability;

public class 扇舞急 : ISlotResolver
{
    public int Check()
    {
        if (!Core.Me.HasAura(Data.Buffs.扇舞急预备)) return -1;
        if (Core.Resolve<JobApi_Dancer>().IsDancing) return -1;

        //大舞前0g不打
        // if (Data.Spells.大舞.GetSpell().Cooldown.TotalMilliseconds < 2500) return -5;
        
        //大舞跳完1g内不打
        if (Data.Spells.四色大舞结束.RecentlyUsed(1500)) return -4;

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.扇舞急.GetSpell());
    }
}