using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;

namespace yoyokity.VPR.QtUI;

public static class Qt
{
    public static JobViewWindow Instance { get; set; }

    /// <summary>
    /// 除了爆发药以外都复原
    /// </summary>
    public static void Reset()
    {
        Instance.SetQt("AOE", true);
        Instance.SetQt("倾泻资源", false);
    }

    public static void Build()
    {
        // JobViewSave是AE底层提供的QT设置存档类 在你自己的设置里定义即可
        // 第二个参数是你设置文件的Save类 第三个参数是QT窗口标题
        Instance = new JobViewWindow(VprSettings.Instance.JobViewSave, VprSettings.Instance.Save, "yoyo蛇批");
        Instance.AddQt("爆发药", false);
        Instance.AddQt("真北", true);
        Instance.AddQt("AOE", true);
        Instance.AddQt("倾泻资源", false, "狂暴鸿儒！");
        
        Instance.AddHotkey("LB", new HotKeyResolver_LB());
        Instance.AddHotkey("防击退",
            new HotKeyResolver_NormalSpell(SpellsDefine.ArmsLength, SpellTargetType.Self));
        Instance.AddHotkey("内丹",
            new HotKeyResolver_NormalSpell(SpellsDefine.SecondWind, SpellTargetType.Self));
        Instance.AddHotkey("疾跑", new HotKeyResolver_疾跑());
        Instance.AddHotkey("爆发药", new HotKeyResolver_Potion());

        VprSettings.Instance.JobViewSave.QtUnVisibleList.Clear();
        // VprSettings.Instance.JobViewSave.QtUnVisibleList.Add("延迟技能");

        //其余tab窗口
        SettingTab.Build(Instance);
    }
}