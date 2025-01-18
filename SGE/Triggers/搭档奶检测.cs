using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.Extension;
using AEAssist.GUI;
using AEAssist.Helper;

namespace yoyokity.SGE.Triggers;

public class 搭档奶检测 : ITriggerCond
{
    public string DisplayName => "Sage/搭档奶检测";
    public string Remark { get; set; }

    public string Key = "";

    private int _selectIndex;
    private string[] _qtArray = ["白魔", "占星", "贤者", "学者"];

    public bool Draw()
    {
        _selectIndex = Array.IndexOf(_qtArray, Key);
        if (_selectIndex == -1)
        {
            _selectIndex = 0;
        }

        ImGuiHelper.LeftCombo("职业", ref _selectIndex, _qtArray);
        Key = _qtArray[_selectIndex];

        return true;
    }

    public bool Handle(ITriggerCondParams triggerCondParams)
    {
        var job = Key switch
        {
            "白魔" => Jobs.WhiteMage,
            "贤者" => Jobs.Sage,
            "学者" => Jobs.Scholar,
            "占星" => Jobs.Astrologian,
            _ => Jobs.Any
        };
        var another = PartyHelper.Party.FirstOrDefault(p => p.IsHealer() && p != Core.Me);
        return another?.CurrentJob() == job;
    }
}