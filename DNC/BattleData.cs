namespace yoyokity.DNC;

public class BattleData
{
    public static BattleData Instance = new();

    /// <summary>
    /// 用于记录gcd复唱时间
    /// </summary>
    public int GcdDuration = 2500;

    public int 提拉纳阈值 = 30;

    public int 上次自动舞伴时间 = -5000;
}