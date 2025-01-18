using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;

namespace yoyokity.DNC.SlotResolver.GCD;

public class Dance_舞步 : ISlotResolver
{
    private static int 舞步步骤 => Core.Resolve<JobApi_Dancer>().CompleteSteps;
    private static bool 大舞ing => Core.Me.HasAura(Data.Buffs.正在大舞);
    private static bool 小舞ing => Core.Me.HasAura(Data.Buffs.正在小舞);

    private class 探戈 : ISlotSequence
    {
        public List<Action<Slot>> Sequence =>
        [
            (Slot slot) =>
            {
                slot.Wait2NextGcd = true;
                slot.Add(Data.Spells.探戈.GetSpell());
            }
        ];
    }

    public int Check()
    {
        if (!Core.Resolve<JobApi_Dancer>().IsDancing) return -1;

        return 0;
    }

    public void Build(Slot slot)
    {
        //大舞结束
        if (大舞ing && 舞步步骤 == 4)
        {
            slot.Add(Data.Spells.四色大舞结束.GetSpell());
            slot.AppendSequence(new 探戈(), false);
            return;
        }

        //小舞结束
        if (小舞ing && 舞步步骤 == 2)
        {
            slot.Add(Data.Spells.双色标准舞步结束.GetSpell());
            return;
        }

        //舞步
        slot.Wait2NextGcd = true;
        slot.Add(Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
    }
}