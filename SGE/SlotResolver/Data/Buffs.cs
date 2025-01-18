using AEAssist;

namespace yoyokity.SGE.SlotResolver.Data;

public static class Buffs
{
    public const uint
        活死人 = 810,
        死而不僵 = 811,
        死斗 = 409,
        超火流星 = 1836,
        即刻 = 167,
        心关 = 2604,
        关心 = 2605,
        均衡注药 = 2614,
        均衡注药2 = 2615,
        均衡注药3 = 2616,
        均衡失衡 = 3897,
        均衡预后 = 2609,
        均衡 = 2606;

    public static uint 均衡注药adaptive =>
        Core.Me.Level switch
        {
            >= 82 => 均衡注药3,
            >= 72 => 均衡注药2,
            _ => 均衡注药
        };
}