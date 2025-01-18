using AEAssist;

namespace yoyokity.SGE.SlotResolver.Data;

public static class Spells
{
    public const uint
        营救 = 7571,
        神翼 = 24295,
        即刻咏唱 = 7561,
        醒梦 = 7562,
        均衡 = 24290,
        注药 = 24283,
        注药2 = 24306,
        注药3 = 24312,
        均衡注药 = 24293,
        均衡注药2 = 24308,
        均衡注药3 = 24314,
        发炎 = 24289,
        发炎2 = 24307,
        发炎3 = 24313,
        贤炮 = 24318,
        心神风息 = 37033,
        均衡失衡 = 37032,
        失衡 = 24297,
        失衡2 = 24315,
        箭毒 = 24304,
        箭毒2 = 24316,
        心关 = 24285,
        根素 = 24309,
        消化 = 24301,
        灵橡清汁 = 24296,
        白牛清汁 = 24303,
        混合 = 24317,
        输血 = 24305,
        拯救 = 24294,
        自生 = 24288,
        自生2 = 24302,
        寄生清汁 = 24299,
        活化 = 24300,
        智慧之爱 = 37035,
        坚角清汁 = 24298,
        泛输血 = 24311,
        整体论 = 24310,
        诊断 = 24284,
        均衡诊断 = 24291,
        预后 = 24286,
        均衡预后 = 24292,
        均衡预后2 = 37034,
        复活 = 24287;
    
    public static uint 失衡adaptive =>
        Core.Me.Level >= 82 ? 失衡2 : 失衡;

    public static uint 均衡预后adaptive =>
        Core.Me.Level >= 96 ? 均衡预后2 : 均衡预后;

    public static uint 自生adaptive =>
        Core.Me.Level >= 60 ? 自生2 : 自生;

    public static uint 发炎adaptive =>
        Core.Me.Level switch
        {
            >= 82 => 发炎3,
            >= 72 => 发炎2,
            _ => 发炎
        };

    public static uint 均衡注药adaptive =>
        Core.Me.Level switch
        {
            >= 82 => 均衡注药3,
            >= 72 => 均衡注药2,
            _ => 均衡注药
        };

    public static uint 箭毒adaptive =>
        Core.Me.Level >= 82 ? 箭毒2 : 箭毒;
    
    public static uint 注药adaptive =>
        Core.Me.Level switch
        {
            >= 82 => 注药3,
            >= 72 => 注药2,
            _ => 注药
        };

}