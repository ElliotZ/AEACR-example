using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Helper;
using AEAssist.IO;

namespace yoyokity.SGE;

public class SgeSettings
{
    public static SgeSettings Instance;

    /// <summary>
    /// 配置文件适合放一些一般不会在战斗中随时调整的开关数据
    /// 如果一些开关需要在战斗中调整 或者提供给时间轴操作 那就用QT
    /// 非开关类型的配置都放配置里 比如诗人绝峰能量配置
    /// </summary>

    #region 标准模板代码 可以直接复制后改掉类名即可

    private static string path;

    public static void Build(string settingPath)
    {
        path = Path.Combine(settingPath, $"{nameof(SgeSettings)}.json");
        if (!File.Exists(path))
        {
            Instance = new SgeSettings();
            Instance.Save();
            return;
        }

        try
        {
            Instance = JsonHelper.FromJson<SgeSettings>(File.ReadAllText(path));
        }
        catch (Exception e)
        {
            Instance = new();
            LogHelper.Error(e.ToString());
        }
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, JsonHelper.ToJson(this));
    }

    #endregion

    public JobViewSave JobViewSave = new()
    {
        QtLineCount = 3,
        HotkeyUnVisibleList = ["寄生清汁", "拯救", "输血(目标的目标)", "白牛(目标的目标)"]
    }; // QT设置存档

    public int SgePartnerPanelIconSize = 47;
    public bool SgePartnerPanelShow = true;
    public bool SgePartner营救 = false;
    public bool AutoUpdataTimeLines = true;
    public bool TimeLinesDebug = false;

    //
    public bool 自动心关 = true;
    public bool 即刻移动 = false;
    public int 醒梦阈值 = 65;
    public bool 贤炮爆发药 = true;
    public bool T无敌给混合 = true;
    public bool 没有箭毒打即刻 = true;

    //
    public int 灵橡阈值 = 40;
    public int 白牛阈值 = 60;
    public int 混合阈值 = 75;
    public int 拯救阈值 = 85;
    public int 输血阈值 = 65;
    public int 诊断阈值 = 0;

    //
    public int 团血检测人数 = 3;
    public int 魂灵风息阈值 = 60;
    public int 寄生青汁阈值 = 70;
    public int 自生阈值 = 80;
    public int 群盾消化阈值 = 0;
    public int 智慧之爱阈值 = 50;
    public int 活化魂灵风息阈值 = 40;
    public int 预后阈值 = 0;
    public int 群盾阈值 = 0;
}