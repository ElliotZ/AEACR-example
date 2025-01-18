using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using AEAssist.Helper;

namespace yoyokity.SGE.Triggers;

public class 自动单奶阈值设置 : ITriggerAction
{
    public string DisplayName => "Sage/自动单奶阈值设置";
    public string Key = "";
    public int Value;
    public string Remark { get; set; }

    private int _selectIndex;

    private readonly Dictionary<string, (Func<SgeSettings, int> getter, Action<SgeSettings, int> setter)> _dict = new()
    {
        {
            "灵橡阈值", (s => s.灵橡阈值, (s, value) => s.灵橡阈值 = value)
        },
        {
            "白牛阈值", (s => s.白牛阈值, (s, value) => s.白牛阈值 = value)
        },
        {
            "混合阈值", (s => s.混合阈值, (s, value) => s.混合阈值 = value)
        },
        {
            "拯救阈值", (s => s.拯救阈值, (s, value) => s.拯救阈值 = value)
        },
        {
            "输血阈值", (s => s.输血阈值, (s, value) => s.输血阈值 = value)
        },
        {
            "诊断阈值", (s => s.诊断阈值, (s, value) => s.诊断阈值 = value)
        }
    };

    private string[] NameList => _dict.Keys.ToArray();

    public bool Draw()
    {
        _selectIndex = Array.IndexOf(NameList, Key);
        if (_selectIndex == -1)
            _selectIndex = 0;
        ImGuiHelper.LeftCombo("选择技能", ref _selectIndex, NameList);
        Key = NameList[_selectIndex];
        ImGuiHelper.LeftInputInt("奶量阈值", ref Value, 0, 100, 5);

        return true;
    }

    public bool Handle()
    {
        _dict[Key].setter(SgeSettings.Instance, Value);
        if (SgeSettings.Instance.TimeLinesDebug) LogHelper.Print("时间轴", $"{Key} => {Value}");
        return true;
    }
}