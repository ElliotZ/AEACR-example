using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using yoyokity.Common;
using yoyokity.DNC.SlotResolver.Data;

namespace yoyokity.DNC.QtUI;

public static class Qt
{
    public static JobViewWindow Instance { get; set; }

    /// <summary>
    /// 除了爆发药以外都复原
    /// </summary>
    public static void Reset()
    {
        Instance.SetQt("AOE", true);
        Instance.SetQt("大舞", true);
        Instance.SetQt("小舞", true);
        Instance.SetQt("探戈", true);
        Instance.SetQt("百花", true);
        Instance.SetQt("剑舞", true);
        Instance.SetQt("扇舞", true);
        Instance.SetQt("无目标小舞", true);
        Instance.SetQt("无目标大舞", false);
        Instance.SetQt("倾泻资源", false);
        Instance.SetQt("倾泻伶俐", false);
        Instance.SetQt("延迟技能", true);
        Instance.SetQt("100伶俐", false);
    }

    public static void Build()
    {
        Instance = new JobViewWindow(DncSettings.Instance.JobViewSave, DncSettings.Instance.Save, "yoyo舞者");
        Instance.AddQt("爆发药", false);
        Instance.AddQt("爆发药2分", true, "第一个爆发药是否在2分钟时使用\n后续爆发药依然是好了之后对齐大舞用");
        Instance.AddQt("大舞", true);
        Instance.AddQt("小舞", true);
        Instance.AddQt("探戈", true);
        Instance.AddQt("百花", true);
        Instance.AddQt("AOE", true);
        Instance.AddQt("剑舞", true);
        Instance.AddQt("扇舞", true, "是否消耗【幻扇】");
        Instance.AddQt("无目标小舞", true, "进入战斗状态且没有目标时，小舞好了就开跳，但是不会跳结束");
        Instance.AddQt("无目标大舞", false, "进入战斗状态且没有目标时，大舞好了就开跳，但是不会跳结束");
        Instance.AddQt("倾泻资源", false, "狂暴鸿儒！");
        Instance.AddQt("倾泻伶俐", false, "把剑舞和落幕舞都打出去");
        Instance.AddQt("延迟技能", true, "是否使用高延迟的【结束动作】【拂晓舞】");
        Instance.AddQt("90伶俐", false, "将剑舞阈值提高到90伶俐");


        Instance.AddHotkey("前冲步",
            new HotKeyResolver(16010, SpellTargetType.Self, false));
        Instance.AddHotkey("桑巴",
            new HotKeyResolver(16012, SpellTargetType.Self, false));
        Instance.AddHotkey("LB", new HotKeyResolver_LB());
        Instance.AddHotkey("防击退",
            new HotKeyResolver(SpellsDefine.ArmsLength, SpellTargetType.Self, false));
        Instance.AddHotkey("内丹",
            new HotKeyResolver(SpellsDefine.SecondWind, SpellTargetType.Self, false));
        Instance.AddHotkey("华尔兹",
            new HotKeyResolver(Spells.华尔兹, SpellTargetType.Self, false));
        Instance.AddHotkey("秒开关即兴", new 即兴表演hotkey());
        Instance.AddHotkey("疾跑", new HotKeyResolver_疾跑());
        Instance.AddHotkey("爆发药", new HotKeyResolver_Potion());

        DncSettings.Instance.JobViewSave.QtUnVisibleList.TryAdd("延迟技能");
        DncSettings.Instance.JobViewSave.QtUnVisibleList.TryAdd("90伶俐");

        //其余tab窗口
        ReadmeTab.Build(Instance);
        SettingTab.Build(Instance);

        //舞伴窗口
        舞伴hotkeyWindow.Build(Instance);
    }
}