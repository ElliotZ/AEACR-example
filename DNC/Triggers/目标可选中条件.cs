using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using AEAssist.Helper;

namespace yoyokity.DNC.Triggers;

public class 目标可选中条件:ITriggerCond
{
    
    public string DisplayName => "目标可选中";
    public string Remark { get; set; }
    
    [LabelName("目标id")]
    public int Id { get; set; }
    
    public bool Draw()
    {
        return false;
    }

    public bool Handle(ITriggerCondParams triggerCondParams)
    {
        return ECHelper.Objects
            .Where(gameObject => gameObject.GameObjectId == (ulong)Id)
            .Any(gameObject => gameObject.IsTargetable);
    }
}