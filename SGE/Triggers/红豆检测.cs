using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;

namespace yoyokity.SGE.Triggers;

public class 红豆检测 : ITriggerCond
{
    public string DisplayName => "Sage/红豆检测";
    public string Remark { get; set; }
    
    [LabelName("红豆数量 >=")]
    public int Value { get; set; }
    
    public bool Draw()
    {
        return false;
    }

    public bool Handle(ITriggerCondParams triggerCondParams)
    {
        return SgeHelper.红豆 >= Value;
    }
}