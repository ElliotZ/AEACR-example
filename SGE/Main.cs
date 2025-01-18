using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using yoyokity.Common;
using yoyokity.SGE.QtUI;
using yoyokity.SGE.SlotResolver.Ability;
using yoyokity.SGE.SlotResolver.GCD;
using yoyokity.SGE.SlotResolver.奶;
using yoyokity.SGE.SlotResolver.Opener;
using yoyokity.SGE.Triggers;

namespace yoyokity.SGE;

public class DncRotationEntry : IRotationEntry, IDisposable
{
    public string AuthorName { get; set; } = Helper.AuthorName;
    private readonly Jobs _targetJob = Jobs.Sage;
    private readonly AcrType _acrType = AcrType.Both;
    private readonly int _minLevel = 70;
    private readonly int _maxLevel = 100;

    private readonly string _description = "小挂不是挂，亲妈是你妈。\n" +
                                           "吉批说瞎话，牌坊给他擦。\n" +
                                           "飞升加进化，荣光你我他。\n" +
                                           "小吉轮流做，明年到我家。\n\n\n" +
                                           "极致的输出，极致的享受\n" +
                                           "从现在开始你的目的就只有一个\n\n" +
                                           "—— 那就是成为logs之神\n\n" +
                                           "推荐复唱：2.49、2.5 (2.49对网络波动容错率更高)";


    private readonly List<SlotResolverData> _slotResolvers =
    [
        //GCD
        new(new Dot_AOE(), SlotMode.Gcd),
        new(new Dot(), SlotMode.Gcd),
        new(new 发炎(), SlotMode.Gcd),

        new(new 群盾(), SlotMode.Gcd),
        new(new 预后(), SlotMode.Gcd),
        new(new 群盾消化(), SlotMode.Gcd),
        new(new 贤炮(), SlotMode.Gcd),
        new(new 诊断(), SlotMode.Gcd),

        new(new 箭毒_AOE(), SlotMode.Gcd),
        new(new Aoe(), SlotMode.Gcd),
        new(new 注药(), SlotMode.Gcd),
        new(new 发炎_移动中(), SlotMode.Gcd),
        new(new 箭毒_移动中(), SlotMode.Gcd),
        new(new Aoe_移动中(), SlotMode.Gcd),

        //能力技
        new(new 心神风息(), SlotMode.OffGcd),
        new(new 根素(), SlotMode.OffGcd),

        new(new 智慧之爱(), SlotMode.OffGcd),
        new(new 活化贤炮(), SlotMode.OffGcd),
        new(new 寄生(), SlotMode.OffGcd),
        new(new 自生(), SlotMode.OffGcd),

        new(new 输血(), SlotMode.OffGcd),
        new(new 拯救(), SlotMode.OffGcd),
        new(new 混合(), SlotMode.OffGcd),
        new(new 白牛(), SlotMode.OffGcd),
        new(new 灵橡(), SlotMode.OffGcd),

        new(new 醒梦(), SlotMode.OffGcd),
    ];


    public Rotation? Build(string settingFolder)
    {
        SgeSettings.Build(settingFolder);
        Qt.Build();
        var rot = new Rotation(_slotResolvers)
        {
            TargetJob = _targetJob,
            AcrType = _acrType,
            MinLevel = _minLevel,
            MaxLevel = _maxLevel,
            Description = _description
        };
        rot.AddOpener(level =>
        {
            if (level < 90)
            {
                return new Opener80();
            }

            return new Opener100();
        });
        rot.SetRotationEventHandler(new EventHandler());
        rot.AddTriggerAction(new TriggerActionQt(), new TriggerActionHotkey(), new 团血检测人数设置(), new 自动群奶阈值设置(),
            new 自动单奶阈值设置());
        rot.AddTriggerCondition(new TriggerCondQt(), new 搭档奶检测(), new 红豆检测());
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