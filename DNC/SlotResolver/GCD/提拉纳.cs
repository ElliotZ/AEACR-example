using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using yoyokity.Common;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.GCD;

public class 提拉纳 : ISlotResolver
{
    private static int 伶俐 => Core.Resolve<JobApi_Dancer>().Esprit;

    public int Check()
    {
        if (!Core.Me.HasAura(Data.Buffs.提拉纳预备)) return -1;

        //倾泻资源qt
        if (Qt.Instance.GetQt("倾泻资源") && 伶俐 <= 40) return 5;

        //buff快消失了打
        if (DncHelper.AuraInGCDs(Data.Buffs.提拉纳预备, 1)) return 6;

        //结束舞步前1、2g不打
        if (Core.Me.HasAura(Data.Buffs.落幕舞预备))
        {
            if (Data.Spells.结束动作.GetSpell().Cooldown.TotalMilliseconds <= 5000 + Helper.小舞卡gcd最长时间)
                return -3;
        }
        else
        {
            if (Data.Spells.结束动作.GetSpell().Cooldown.TotalMilliseconds <= 2500 + Helper.小舞卡gcd最长时间)
                return -3;
        }

        //大舞结束前最后2G时小于等于40打
        if (DncHelper.AuraInGCDs(Data.Buffs.大舞Buff, 2) && 伶俐 <= 40)
            return 1;

        //小于等于阈值才打
        if (伶俐 <= BattleData.Instance.提拉纳阈值)
            return 2;

        return -5;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.提拉纳.GetSpell());
    }
}