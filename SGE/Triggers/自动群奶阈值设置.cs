using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using AEAssist.Helper;

namespace yoyokity.SGE.Triggers;

public class 自动群奶阈值设置 : ITriggerAction
{
    public string DisplayName => "Sage/自动群奶阈值设置";
    public string Key = "";
    public int Value;
    public string Remark { get; set; }

    private int _selectIndex;

    private readonly Dictionary<string, (Func<SgeSettings, int> getter, Action<SgeSettings, int> setter)> _dict = new()
    {
        {
            "自生阈值", (s => s.自生阈值, (s, value) => s.自生阈值 = value)
        },
        {
            "寄生阈值", (s => s.寄生青汁阈值, (s, value) => s.寄生青汁阈值 = value)
        },
        {
            "贤炮阈值", (s => s.魂灵风息阈值, (s, value) => s.魂灵风息阈值 = value)
        },
        {
            "活化贤炮阈值", (s => s.活化魂灵风息阈值, (s, value) => s.活化魂灵风息阈值 = value)
        },
        {
            "智慧之爱阈值", (s => s.智慧之爱阈值, (s, value) => s.智慧之爱阈值 = value)
        },
        {
            "群盾消化阈值", (s => s.群盾消化阈值, (s, value) => s.群盾消化阈值 = value)
        },
        {
            "预后阈值", (s => s.预后阈值, (s, value) => s.预后阈值 = value)
        },
        {
            "群盾阈值", (s => s.群盾阈值, (s, value) => s.群盾阈值 = value)
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