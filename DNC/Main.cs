using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using yoyokity.Common;
using yoyokity.DNC.QtUI;
using yoyokity.DNC.SlotResolver.Ability;
using yoyokity.DNC.SlotResolver.GCD;
using yoyokity.DNC.SlotResolver.Opener;
using yoyokity.DNC.Triggers;

namespace yoyokity.DNC;

public class DncRotationEntry : IRotationEntry, IDisposable
{
    public string AuthorName { get; set; } = Helper.AuthorName;
    private readonly Jobs _targetJob = Jobs.Dancer;
    private readonly AcrType _acrType = AcrType.Both; //高难专用
    private readonly int _minLevel = 15;
    private readonly int _maxLevel = 100;

    private readonly string _description = "小挂不是挂，亲妈是你妈。\n" +
                                           "吉批说瞎话，牌坊给他擦。\n" +
                                           "飞升加进化，荣光你我他。\n" +
                                           "小吉轮流做，明年到我家。\n\n\n" +
                                           "极致的输出，极致的享受\n" +
                                           "日随练级这种你找别家的去，从现在开始你的目的就只有一个\n\n" +
                                           "—— 那就是成为logs之神";


    private readonly List<SlotResolverData> _slotResolvers =
    [
        //GCD
        new(new Dance_舞步(), SlotMode.Gcd),
        new(new Dance_大舞(), SlotMode.Gcd),
        new(new Dance_结束动作(), SlotMode.Gcd),
        new(new Dance_小舞(), SlotMode.Gcd),

        new(new 流星舞_高优先(), SlotMode.Gcd),
        new(new 落幕舞_高优先(), SlotMode.Gcd),
        new(new Base3_坠喷泉落血雨_高优先(), SlotMode.Gcd),
        new(new Base2_逆瀑泻升风车_高优先(), SlotMode.Gcd),
        new(new 剑舞拂晓舞(), SlotMode.Gcd),
        new(new 流星舞(), SlotMode.Gcd),
        new(new 提拉纳(), SlotMode.Gcd),
        new(new 落幕舞(), SlotMode.Gcd),

        new(new Base3_坠喷泉落血雨(), SlotMode.Gcd),
        new(new Base2_逆瀑泻升风车(), SlotMode.Gcd),
        new(new 大舞前1G优化(), SlotMode.Gcd),
        new(new 大舞前2G优化(), SlotMode.Gcd),
        new(new Base(), SlotMode.Gcd),

        //能力技
        new(new 探戈(), SlotMode.OffGcd),
        new(new 爆发药(), SlotMode.OffGcd),
        new(new 扇舞急(), SlotMode.OffGcd),
        new(new 百花(), SlotMode.OffGcd),
        new(new 扇舞终(), SlotMode.OffGcd),
        new(new 扇舞序(), SlotMode.OffGcd),
    ];


    public Rotation? Build(string settingFolder)
    {
        DncSettings.Build(settingFolder);
        Qt.Build();
        var rot = new Rotation(_slotResolvers)
        {
            TargetJob = _targetJob,
            AcrType = _acrType,
            MinLevel = _minLevel,
            MaxLevel = _maxLevel,
            Description = _description,
        };
        rot.AddOpener(level => level < _minLevel ? null : new OpenerBase());
        rot.SetRotationEventHandler(new EventHandler());
        rot.AddTriggerAction(new TriggerActionQt(), new TriggerActionHotkey(), new 提拉纳阈值设置());
        rot.AddTriggerCondition(new TriggerCondQt());
        rot.AddCanUseHighPrioritySlotCheck(Helper.HighPrioritySlotCheckFunc);
        return rot;
    }

    public IRotationUI GetRotationUI()
    {
        return Qt.Instance;
    }

    public void OnDrawSetting()
    {
    }

    public void Dispose()
    {
    }
}