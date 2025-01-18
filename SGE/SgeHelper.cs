using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using Dalamud.Game.ClientState.Objects.Types;
using yoyokity.Common;
using Data = yoyokity.SGE.SlotResolver.Data;

namespace yoyokity.SGE;

public static class SgeHelper
{
    /// <summary>
    /// 获取当前gcd复唱时间
    /// </summary>
    public static int GetGcdDuration => DNC.BattleData.Instance.GcdDuration;

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

    public static int 蓝豆 => Core.Resolve<JobApi_Sage>().Addersgall;
    public static long 量谱时间 => Core.Resolve<JobApi_Sage>().AddersgallTimer;
    public static int 红豆 => Core.Resolve<JobApi_Sage>().Addersting;
    public static bool 均衡中 => Core.Resolve<JobApi_Sage>().Eukrasia;

    public static IBattleChara? 心关目标 => 
        PartyHelper.Party.FirstOrDefault(p => p.HasLocalPlayerAura(Data.Buffs.关心));
    
    public static bool 学者搭档 => 
        PartyHelper.Party.FirstOrDefault(p => p.CurrentJob() == Jobs.Scholar) != null;
}