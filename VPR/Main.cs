// using AEAssist.CombatRoutine;
// using AEAssist.CombatRoutine.Module;
// using yoyokity.Common;
// // using yoyokity.VPR.SlotResolver.Opener;
// // using yoyokity.VPR.Triggers;
// using yoyokity.VPR.QtUI;
// using yoyokity.VPR.SlotResolver.Ability;
// using yoyokity.VPR.SlotResolver.GCD;
//
// namespace yoyokity.VPR;
//
// public class VprRotationEntry : IRotationEntry
// {
//     public string AuthorName { get; set; } = Helper.AuthorName;
//     private readonly Jobs _targetJob = Jobs.Viper;
//     private readonly AcrType _acrType = AcrType.HighEnd; //高难专用
//     private readonly int _minLevel = 100;
//     private readonly int _maxLevel = 100;
//
//     private readonly string _description = "小挂不是挂，亲妈是你妈。\n" +
//                                            "吉批说瞎话，牌坊给他擦。\n" +
//                                            "飞升加进化，荣光你我他。\n" +
//                                            "小吉轮流做，明年到我家。\n\n\n" +
//                                            "极致的输出，极致的享受\n" +
//                                            "日随练级这种你找别家的去，从现在开始你的目的就只有一个\n\n" +
//                                            "—— 那就是成为logs之神";
//
//     private static readonly List<SlotResolverData> _slotResolvers =
//     [
//         //GCD
//         new(new 祖灵连(), SlotMode.Gcd),
//         new(new 祖灵降临(), SlotMode.Gcd),
//         new(new 飞蛇之尾_高优先(), SlotMode.Gcd),
//         new(new 蛇剑连(), SlotMode.Gcd),
//         new(new 基础连(), SlotMode.Gcd),
//         new(new 飞蛇之尾(), SlotMode.Gcd),
//
//         //能力技
//         new(new 基础续剑(), SlotMode.OffGcd),
//         new(new 蛇剑连续剑(), SlotMode.OffGcd),
//         new(new 飞蛇续剑(), SlotMode.OffGcd),
//         new(new 祖灵续剑(), SlotMode.OffGcd),
//         new(new 蛇灵气(), SlotMode.OffGcd),
//     ];
//
//     public Rotation? Build(string settingFolder)
//     {
//         VprSettings.Build(settingFolder);
//         Qt.Build();
//         var rot = new Rotation(_slotResolvers)
//         {
//             TargetJob = _targetJob,
//             AcrType = _acrType,
//             MinLevel = _minLevel,
//             MaxLevel = _maxLevel,
//             Description = _description,
//         };
//         rot.AddOpener(null);
//         // rot.AddOpener(level => level < _minLevel ? null : new OpenerBase());
//         rot.SetRotationEventHandler(new EventHandler());
//         // rot.AddTriggerAction(new TriggerActionQt(), new TriggerActionHotkey(),new 提拉纳阈值设置());
//         // rot.AddTriggerCondition(new 目标可选中条件());
//         rot.AddCanUseHighPrioritySlotCheck(Helper.HighPrioritySlotCheckFunc);
//         return rot;
//     }
//
//     public IRotationUI GetRotationUI()
//     {
//         return Qt.Instance;
//     }
//
//     public void OnDrawSetting()
//     {
//     }
//
//     public void Dispose()
//     {
//     }
// }