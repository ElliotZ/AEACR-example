using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using yoyokity.Common;
using Data = yoyokity.SGE.SlotResolver.Data;

namespace yoyokity.SGE.QtUI;

public static class Qt
{
    public static JobViewWindow Instance { get; set; }

    /// <summary>
    /// 除了爆发药以外都复原
    /// </summary>
    public static void Reset()
    {
        Instance.SetQt("AOE", true);
        Instance.SetQt("保留1蓝豆", true);
        Instance.SetQt("Dot", true);
        Instance.SetQt("发炎", true);
        Instance.SetQt("箭毒", true);
        Instance.SetQt("保留1红豆", true);
        Instance.SetQt("根素", true);
        Instance.SetQt("倾泻资源", false);
    }

    public static void Build()
    {
        Instance = new JobViewWindow(SgeSettings.Instance.JobViewSave, SgeSettings.Instance.Save, "yoyo贤者");
        Instance.AddQt("爆发药", false, "只打起手爆发药，后续用轴打或者手动打");
        Instance.AddQt("AOE", true);
        Instance.AddQt("Dot", true);
        Instance.AddQt("单奶", true);
        Instance.AddQt("群奶", true);
        Instance.AddQt("保留1蓝豆", true, "自动奶的时候保留一个蓝豆用于救急");
        Instance.AddQt("发炎", true, "会自动攒资源来对齐团辅或爆发药。\n如果有长时间远离导致溢出的情况，请用时间轴提前打掉。\n如果有多个目标则直接打空。");
        Instance.AddQt("箭毒", true, "用于无损走位");
        Instance.AddQt("保留1红豆", true, "用于无损打爆发药，打aoe时收益过高会无视这个qt全部打空");
        Instance.AddQt("心神", true, "心神风息");
        Instance.AddQt("根素", true, "蓝豆小于等于1时自动打");
        Instance.AddQt("倾泻资源", false, "狂暴鸿儒！");


        Instance.AddHotkey("LB", new HotKeyResolver_LB());
        Instance.AddHotkey("爆发药", new 爆发药hotkey());
        Instance.AddHotkey("疾跑", new HotKeyResolver_疾跑());
        Instance.AddHotkey("防击退",
            new HotKeyResolver(SpellsDefine.Surecast, SpellTargetType.Self, false));
        Instance.AddHotkey("向目标突进", new 突进hotkey());
        Instance.AddHotkey("即刻",
            new HotKeyResolver(SpellsDefine.Swiftcast, SpellTargetType.Self, false,false));
        Instance.AddHotkey("发炎", new 发炎hotkey());
        Instance.AddHotkey("箭毒", new 箭毒hotkey());
        Instance.AddHotkey("坚角青汁",
            new HotKeyResolver(Data.Spells.坚角清汁, SpellTargetType.Self));
        Instance.AddHotkey("泛输血",
            new HotKeyResolver(Data.Spells.泛输血, SpellTargetType.Self));
        Instance.AddHotkey("整体论",
            new HotKeyResolver(Data.Spells.整体论, SpellTargetType.Self));
        Instance.AddHotkey("[有损盾] 群盾(有活化就加上)", new 群盾hotkey());
        Instance.AddHotkey("自生",
            new HotKeyResolver(Data.Spells.自生adaptive, SpellTargetType.Self));
        Instance.AddHotkey("智慧之爱",
            new HotKeyResolver(Data.Spells.智慧之爱, SpellTargetType.Self));
        Instance.AddHotkey("贤炮(有活化就加上)", new 贤炮hotkey());
        Instance.AddHotkey("[有损奶] 群盾消化", new 消化hotkey());

        Instance.AddHotkey("寄生清汁",
            new HotKeyResolver(Data.Spells.寄生清汁, SpellTargetType.Self));
        Instance.AddHotkey("拯救",
            new HotKeyResolver(Data.Spells.拯救, SpellTargetType.Self));
        Instance.AddHotkey("输血(目标的目标)", new 输血hotkey());
        Instance.AddHotkey("白牛(目标的目标)", new 白牛hotkey());
        

        //其余tab窗口
        ReadmeTab.Build(Instance);
        SettingTab.Build(Instance);

        //心关窗口
        复活hotkeyWindow.Build(Instance);
    }
}