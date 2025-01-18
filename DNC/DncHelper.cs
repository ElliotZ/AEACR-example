using yoyokity.Common;

namespace yoyokity.DNC;

public static class DncHelper
{
    /// <summary>
    /// 获取当前gcd复唱时间
    /// </summary>
    public static int GetGcdDuration => BattleData.Instance.GcdDuration;
    
    /// <summary>
    /// 自身buff剩余时间是否在x个gcd内
    /// </summary>
    /// <param name="buffId"></param>
    /// <param name="gcd"></param>
    /// <returns></returns>
    public static bool AuraInGCDs(uint buffId, int gcd)
    {
        var timeLeft = Helper.GetAuraTimeLeft(buffId);
        if (timeLeft <= 0) return false;
        if (GetGcdDuration <= 0) return false;

        return timeLeft / GetGcdDuration < gcd;
    }
}