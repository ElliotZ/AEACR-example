using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using AEAssist.Helper;

namespace yoyokity.DNC.Triggers;

public class 提拉纳阈值设置 : ITriggerAction
{
    public string DisplayName => "Dancer/提拉纳阈值设置";
    public int _阈值 = 30;
    public string Remark { get; set; }

    public bool Draw()
    {
        ImGuiHelper.LeftInputInt("伶俐小于等于这个值才打提拉纳", 
            ref _阈值, 0, 100,5);
        return true;
    }

    public bool Handle()
    {
        BattleData.Instance.提拉纳阈值 = _阈值;
        if (DncSettings.Instance.TimeLinesDebug) LogHelper.Print("时间轴", $"提拉纳阈值 => {_阈值}");
        return true;
    }
}