using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.Helper;
using AEAssist.JobApi;
using yoyokity.Common;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.Opener;

/// <summary>
/// 1gcd团辅起手
/// </summary>
public class Opener1GCD : IOpener
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
        Step0, Step1, Step2, Step3
    ];

    public void InitCountDown(CountDownHandler countDownHandler)
    {
        //qt除了爆发药以外都复原
        Qt.Reset();
        //
        const int startTime = 15000;
        countDownHandler.AddAction(startTime, () =>
        {
            Helper.SendTips("请先远离boss");
            return Data.Spells.标准舞步.GetSpell();
        });
        countDownHandler.AddAction(startTime - 1500, () => Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
        countDownHandler.AddAction(startTime - 2500, () => Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
        countDownHandler.AddAction(startTime - 5000, () =>
        {
            Helper.SendTips("请靠近boss");
            return Data.Spells.双色标准舞步结束.GetSpell();
        });

        const int lastTime = 0;
        countDownHandler.AddAction(lastTime + 4500, Data.Spells.大舞);
        if (Qt.Instance.GetQt("爆发药") && !Qt.Instance.GetQt("爆发药2分"))
            countDownHandler.AddPotionAction(lastTime + 4000);
        countDownHandler.AddAction(lastTime + 3000, () => Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
        countDownHandler.AddAction(lastTime + 2000, () => Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
        countDownHandler.AddAction(lastTime + 1000, () => Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
    }

    private static void Step0(Slot slot)
    {
        slot.Wait2NextGcd = true;
        slot.Add(Core.Resolve<JobApi_Dancer>().NextStep.GetSpell());
    }

    private static void Step1(Slot slot)
    {
        slot.Add(Data.Spells.四色大舞结束.GetSpell());
        slot.Add2NdWindowAbility(Data.Spells.探戈.GetSpell());
    }

    private static void Step2(Slot slot)
    {
        slot.Add(Data.Spells.提拉纳.GetSpell());
        slot.Add(Data.Spells.百花.GetSpell());
        slot.Add(Data.Spells.扇舞终.GetSpell());
    }

    private static void Step3(Slot slot)
    {
        slot.Add(Data.Spells.拂晓舞.GetSpell());
        slot.Add(Data.Spells.扇舞急.GetSpell());
    }
}