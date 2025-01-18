using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.Helper;
using AEAssist.JobApi;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.Opener;

public class OpenerBase : IOpener
{
    public int StartCheck()
    {
        if (!(Data.Spells.大舞.GetSpell().IsReadyWithCanCast() && Qt.Instance.GetQt("大舞"))) return -2;
        if (!(Data.Spells.百花.GetSpell().IsReadyWithCanCast() && Qt.Instance.GetQt("百花"))) return -3;
        if (!(Data.Spells.探戈.GetSpell().IsReadyWithCanCast() && Qt.Instance.GetQt("探戈"))) return -4;
        return 0;
    }

    public int StopCheck(int index)
    {
        return -1;
    }

    private class 舞步 : ISlotSequence
    {
        public List<Action<Slot>> Sequence =>
        [
            (Slot slot) =>
            {
                slot.Wait2NextGcd = true;
                slot.Add(Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
            },
            (Slot slot) =>
            {
                slot.Wait2NextGcd = true;
                slot.Add(Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
            },
            (Slot slot) =>
            {
                slot.Wait2NextGcd = true;
                slot.Add(Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
            },
            (Slot slot) =>
            {
                slot.Wait2NextGcd = true;
                slot.Add(Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
            }
        ];
    }

    public List<Action<Slot>> Sequence { get; } =
    [
        Step1, Step2, Step3, Step4
    ];

    public void InitCountDown(CountDownHandler countDownHandler)
    {
        //qt除了爆发药以外都复原
        Qt.Reset();
        //
        const int startTime = 15000;
        countDownHandler.AddAction(startTime, Data.Spells.标准舞步);
        countDownHandler.AddAction(startTime - 1500, () => Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
        countDownHandler.AddAction(startTime - 2500, () => Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
        if (Qt.Instance.GetQt("爆发药") && !Qt.Instance.GetQt("爆发药2分"))
            countDownHandler.AddPotionAction(1000);
        countDownHandler.AddAction(300, Data.Spells.双色标准舞步结束);
    }

    private static void Step1(Slot slot)
    {
        slot.Add(Data.Spells.大舞.GetSpell());
        slot.AppendSequence(new 舞步());
    }

    private static void Step2(Slot slot)
    {
        slot.Add(Data.Spells.四色大舞结束.GetSpell());
        slot.Add2NdWindowAbility(Data.Spells.探戈.GetSpell());
    }

    private static void Step3(Slot slot)
    {
        slot.Add(Data.Spells.提拉纳.GetSpell());
        slot.Add(Data.Spells.百花.GetSpell());
        slot.Add(Data.Spells.扇舞终.GetSpell());
    }

    private static void Step4(Slot slot)
    {
        slot.Add(Data.Spells.拂晓舞.GetSpell());
        slot.Add(Data.Spells.扇舞急.GetSpell());
    }
}