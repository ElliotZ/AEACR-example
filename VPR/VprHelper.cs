using AEAssist;
using AEAssist.CombatRoutine.View;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.JobGauge.Enums;
using yoyokity.Common;
using Data = yoyokity.VPR.SlotResolver.Data;

namespace yoyokity.VPR;

public static class VprHelper
{
    /// <summary>
    /// 5-1
    /// </summary>
    public static int 祖灵连步骤 => Core.Resolve<JobApi_Viper>().AnguineTribute;

    public static int 祖灵力 => Core.Resolve<JobApi_Viper>().SerpentOffering;
    public static int 蛇尾豆 => Core.Resolve<JobApi_Viper>().RattlingCoilStacks;

    /// <summary>
    /// 上个G不是蛇剑连时返回0
    /// </summary>
    public static DreadCombo 蛇剑连 => Core.Resolve<JobApi_Viper>().DreadCombo;

    /// <summary>
    /// 没有附体续剑时返回NONE
    /// </summary>
    /// <returns></returns>
    public static SerpentCombo 续剑 => Core.Resolve<JobApi_Viper>().SerpentCombo;

    public static uint GetLastComboSpellId => Core.Resolve<MemApiSpell>().GetLastComboSpellId();

    /// <summary>
    /// 当前的基础连步骤
    /// </summary>
    public static int 基础连步骤 =>
        GetLastComboSpellId switch
        {
            Data.Spells.A1 or Data.Spells.B1 => 2,
            Data.Spells.A2 or Data.Spells.B2 => 3,
            _ => 1
        };

    /// <summary>
    /// 获取当前基础连的gcd复唱时间
    /// </summary>
    public static int GetGcdDuration => BattleData.Instance.GcdDuration;

    /// <summary>
    /// 获取当前蛇剑连的gcd复唱时间(包括祖灵大蛇牙)
    /// </summary>
    public static int GetDreadGcdDuration => (int)(BattleData.Instance.GcdDuration * 3 / 2.5);

    /// <summary>
    /// 获取当前祖灵连的gcd复唱时间
    /// </summary>
    public static int GetAnguineGcdDuration => (int)(BattleData.Instance.GcdDuration * 2 / 2.5);

    /// <summary>
    /// 判断蛇剑连是先攻速还是先攻击
    /// </summary>
    /// <param name="time">检测的buff剩余时间</param>
    /// <returns></returns>
    public static bool 蛇剑连优先攻速(int time)
    {
        //如果buff剩余时间不多，优先续buff
        if (Helper.Buff时间小于(Data.Buffs.B2, time))
            return true;
        if (Helper.Buff时间小于(Data.Buffs.A2, time))
            return false;

        //再以上个身位为基准
        return MeleePosHelper2.LastPos == MeleePosHelper.Pos.Behind;
    }
}