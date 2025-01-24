using AEAssist.CombatRoutine.Trigger;

namespace yoyokity.SGE.Triggers;

public class 是否为自嗨模式 : ITriggerCond
{
    public string DisplayName => "Sage/是否为自嗨模式";
    public string Remark { get; set; }
    
    public bool Draw()
    {
        return false;
    }

    public bool Handle(ITriggerCondParams triggerCondParams)
    {
        return SgeSettings.Instance.没妈妈自嗨打法;
    }
}