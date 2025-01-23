using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Types;

namespace yoyokity.Common;

public static class Helper
{
    public const string AuthorName = "yoyokity";

    public const string DncTimeLineUrl =
        "https://raw.githubusercontent.com/yoyokity/AETimeLines/refs/heads/master/yoyo-DNCMaster.json";

    public const string VprTimeLineUrl =
        "https://raw.githubusercontent.com/yoyokity/AETimeLines/refs/heads/master/yoyo-VPRMaster.json";

    public const string SgeTimeLineUrl =
        "https://raw.githubusercontent.com/yoyokity/AETimeLines/refs/heads/master/yoyo-SgeMaster.json";

    public const string TimeLineLibraryUrl = "https://github.com/yoyokity/AETimeLines";
    private static int _GcdDuration = 0;

    public const int 小舞卡gcd最长时间 = 1000;

    /// <summary>
    /// 获取自身buff的剩余时间
    /// </summary>
    /// <param name="buffId"></param>
    /// <returns></returns>
    public static int GetAuraTimeLeft(uint buffId) => Core.Resolve<MemApiBuff>().GetAuraTimeleft(Core.Me, buffId, true);

    /// <summary>显示一个文本提示，用于在游戏中显示简短的消息。</summary>
    /// <param name="msg">要显示的消息文本。</param>
    /// <param name="s">文本提示的样式。支持蓝色提示（1）和红色提示（2）两种</param>
    /// <param name="time">文本提示显示的时间（单位毫秒）。如显示3秒，填写3000即可</param>
    public static void SendTips(string msg, int s = 1, int time = 3000) => Core.Resolve<MemApiChatMessage>()
        .Toast2(msg, s, time);

    public static bool IsMove => MoveHelper.IsMoving();

    /// <summary>
    /// 全局设置
    /// </summary>
    public static GeneralSettings GlobalSettings => SettingMgr.GetSetting<GeneralSettings>();

    /// <summary>
    /// 当前地图id
    /// </summary>
    public static uint GetTerritoyId => Core.Resolve<MemApiMap>().GetCurrTerrId();

    /// <summary>
    /// 返回可变技能的当前id
    /// </summary>
    public static uint GetActionChange(uint spellId) => Core.Resolve<MemApiSpell>().CheckActionChange(spellId);

    /// <summary>
    /// 高优先级插入条件检测函数
    /// </summary>
    public static int HighPrioritySlotCheckFunc(SlotMode mode, Slot slot)
    {
        if (mode != SlotMode.OffGcd) return 1;
        //限制高优先能力技插入，只能在g窗口前半和后半打
        if (GCDHelper.GetGCDCooldown() is > 750 and < 1500) return -1;
        //连续的两个高优先能力技插入，在gcd前半窗口打，以免卡gcd
        if (slot.Actions.Count > 1 && GCDHelper.GetGCDCooldown() < 1500) return -1;
        return 1;
    }

    public static double 连击剩余时间 => Core.Resolve<MemApiSpell>().GetComboTimeLeft().TotalMilliseconds;

    public static bool 在近战范围内 =>
        Core.Me.Distance(Core.Me.GetCurrTarget()!) <= SettingMgr.GetSetting<GeneralSettings>().AttackRange;

    public static bool 在背身位 => Core.Resolve<MemApiTarget>().IsBehind;
    public static bool 在侧身位 => Core.Resolve<MemApiTarget>().IsFlanking;

    /// <summary>
    /// 充能技能还有多少冷却时间才可用
    /// </summary>
    /// <param name="skillId">技能id</param>
    /// <returns></returns>
    public static int 充能技能冷却时间(uint skillId)
    {
        var spell = skillId.GetSpell();
        return (int)(spell.Cooldown.TotalMilliseconds -
                     (spell.RecastTime.TotalMilliseconds / spell.MaxCharges) * (spell.MaxCharges - 1));
    }

    /// <summary>
    /// 自身有buff且时间小于
    /// </summary>
    public static bool Buff时间小于(uint buffId, int timeLeft)
    {
        if (!Core.Me.HasAura(buffId)) return false;
        return GetAuraTimeLeft(buffId) <= timeLeft;
    }

    /// <summary>
    /// 目标有buff且时间小于，有buff参数如果为false，则当目标没有玩家的buff是也返回true
    /// </summary>
    public static bool 目标Buff时间小于(uint buffId, int timeLeft, bool 有buff = true)
    {
        var target = Core.Me.GetCurrTarget();
        if (target == null) return false;

        if (有buff)
        {
            if (!target.HasLocalPlayerAura(buffId)) return false;
        }
        else
        {
            if (!target.HasLocalPlayerAura(buffId)) return true;
        }

        var time = Core.Resolve<MemApiBuff>().GetAuraTimeleft(target, buffId, true);
        return time <= timeLeft;
    }

    /// <summary>
    /// 在list中添加一个唯一的元素
    /// </summary>
    public static bool TryAdd<T>(this List<T> list, T item)
    {
        if (list.Contains(item)) return false;
        list.Add(item);
        return true;
    }

    public static bool 目标有任意我的buff(List<uint> buffs) =>
        buffs.Any(buff => Core.Me.GetCurrTarget()!.HasLocalPlayerAura(buff));


    public static IBattleChara? 最优aoe目标(this uint spellId, int count)
    {
        return TargetHelper.GetMostCanTargetObjects(spellId, count);
    }

    /// <summary>
    /// 获取非战斗状态时开了盾姿的人
    /// </summary>
    /// <returns></returns>
    public static IBattleChara? GetMt()
    {
        PartyHelper.UpdateAllies();
        return PartyHelper.CastableTanks
            .FirstOrDefault(p => p.HasAnyAura([743, 1833, 79, 91]));
    }

    public static bool In团辅()
    {
        //检测目标团辅
        List<uint> 目标团辅 = [背刺, 连环计];
        if (目标团辅.Any(buff => 目标Buff时间小于(buff, 15000))) return true;

        //检测自身团辅
        List<uint> 自身团辅 = [灼热之光, 星空, 占卜, 义结金兰, 战斗连祷, 大舞, 战斗之声, 鼓励, 神秘环];
        return 自身团辅.Any(buff => Buff时间小于(buff, 15000));
    }

    private static uint
        背刺 = 3849,
        强化药 = 49,
        灼热之光 = 2703,
        星空 = 3685,
        占卜 = 1878,
        义结金兰 = 1185,
        战斗连祷 = 786,
        大舞 = 1822,
        战斗之声 = 141,
        鼓励 = 1239,
        神秘环 = 2599,
        连环计 = 2617;
}