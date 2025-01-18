using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using AEAssist.Helper;

namespace yoyokity.SGE.Triggers;

public class 团血检测人数设置 : ITriggerAction
{
    public string DisplayName => "Sage/自动群奶检测人数";
    public int Value = SgeSettings.Instance.团血检测人数;
    public string Remark { get; set; }

    public bool Draw()
    {
        ImGuiHelper.LeftInputInt("团血检测人数", ref Value, 1, 8);
        return true;
    }

    public bool Handle()
    {
        SgeSettings.Instance.团血检测人数 = Value;
        if (SgeSettings.Instance.TimeLinesDebug) LogHelper.Print("时间轴", $"团血检测人数 => {Value}");
        return true;
    }
}