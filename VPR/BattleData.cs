namespace yoyokity.VPR;

public class BattleData
{
    public static BattleData Instance = new();

    /// <summary>
    /// 用于记录gcd复唱时间
    /// </summary>
    public int GcdDuration = 2500;
}