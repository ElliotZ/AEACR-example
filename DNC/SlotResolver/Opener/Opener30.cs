using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.Helper;
using AEAssist.JobApi;
using yoyokity.Common;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.Opener;

/// <summary>
/// 30秒小舞起手
/// </summary>
public class Opener30 : IOpener
{
    public int StartCheck()
    {
        return 0;
    }

    public int StopCheck(int index)
    {
        return -1;
    }

    public List<Action<Slot>> Sequence { get; } =
    [
        Step0, Step1, Step2, Step3, Step4
    ];

    public void InitCountDown(CountDownHandler countDownHandler)
    {
        //qt除了爆发药以外都复原
        Qt.Reset();
        //
        const int startTime = 29500;
        countDownHandler.AddAction(startTime, () =>
        {
            Helper.SendTips("请先远离boss");
            return Data.Spells.标准舞步.GetSpell();
        });
        countDownHandler.AddAction(startTime - 1500, () => Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
        countDownHandler.AddAction(startTime - 2500, () => Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
        countDownHandler.AddAction(startTime - 10000, () =>
        {
            Helper.SendTips("请靠近boss");
            return Data.Spells.双色标准舞步结束.GetSpell();
        });

        const int lastTime = 0;
        countDownHandler.AddAction(lastTime + 5000, Data.Spells.大舞);
        countDownHandler.AddAction(lastTime + 4000, () => Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
        countDownHandler.AddAction(lastTime + 3000, () => Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
        countDownHandler.AddAction(lastTime + 2000, () => Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
        countDownHandler.AddAction(lastTime + 1000, () => Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());

        if (Qt.Instance.GetQt("爆发药") && !Qt.Instance.GetQt("爆发药2分"))
            countDownHandler.AddPotionAction(700);
    }

    private static void Step0(Slot slot)
    {
        if (Core.Resolve<JobApi_Dancer>().IsDancing &&
            Core.Resolve<JobApi_Dancer>().CompleteSteps == 4 &&
            !Data.Spells.四色大舞结束.RecentlyUsed())
            slot.Add(Data.Spells.四色大舞结束.GetSpell());
        slot.Add2NdWindowAbility(Data.Spells.探戈.GetSpell());
    }

    private static void Step1(Slot slot)
    {
        slot.Add(Data.Spells.落幕舞.GetSpell());
        slot.Add(Data.Spells.百花.GetSpell());
    }

    private static void Step2(Slot slot)
    {
        slot.Add(Data.Spells.结束动作.GetSpell());
    }

    private static int 伶俐 => Core.Resolve<JobApi_Dancer>().Esprit;
    private static void Step3(Slot slot)
    {
        slot.Add(伶俐 >= 50 ? Data.Spells.拂晓舞.GetSpell() : Data.Spells.提拉纳.GetSpell());
    }

    private static void Step4(Slot slot)
    {
        slot.Add(伶俐 >= 50 ? Data.Spells.剑舞.GetSpell() : Data.Spells.提拉纳.GetSpell());
        slot.Add(Data.Spells.扇舞终.GetSpell());
        slot.Add(Data.Spells.扇舞急.GetSpell());
    }
}