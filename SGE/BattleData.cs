namespace yoyokity.SGE;

public class BattleData
{
    public static BattleData Instance = new();

    /// <summary>
    /// 用于记录gcd复唱时间
    /// </summary>
    public int GcdDuration = 2500;
    
    public long 最近一次心关时间 = -10000;
    public int 上次爆发药补毒时间 = -10000;
    public int 上次即刻时间 = -10000;
    public bool Lock发炎 = false;
}