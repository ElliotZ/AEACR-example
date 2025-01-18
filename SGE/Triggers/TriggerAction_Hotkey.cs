using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using AEAssist.Helper;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.Triggers;

public class TriggerActionHotkey : ITriggerAction
{
    public string DisplayName => "Sage/Hotkey";
    public string Remark { get; set; }

    public string Key = "";

    // 辅助数据 因为是private 所以不存档
    private int _selectIndex;
    private string[] _hotkeyArray;

    public TriggerActionHotkey()
    {
        _hotkeyArray = Qt.Instance.GetHotkeyArray();
    }

    public bool Draw()
    {
        _selectIndex = Array.IndexOf(_hotkeyArray, Key);
        if (_selectIndex == -1)
        {
            _selectIndex = 0;
        }

        ImGuiHelper.LeftCombo("使用Hotkey", ref _selectIndex, _hotkeyArray);
        Key = _hotkeyArray[_selectIndex];
        return true;
    }

    public bool Handle()
    {
        Qt.Instance.SetHotkey(Key);
        if (SgeSettings.Instance.TimeLinesDebug) LogHelper.Print("时间轴", $"使用hotkey => {Key}");
        return true;
    }
}