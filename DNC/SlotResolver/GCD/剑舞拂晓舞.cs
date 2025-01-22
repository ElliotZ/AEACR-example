using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.GCD;

public class 剑舞拂晓舞 : ISlotResolver
{
    private static int 伶俐 => Core.Resolve<JobApi_Dancer>().Esprit;
    private static bool 大舞buff => Core.Me.HasAura(Data.Buffs.大舞Buff);
    private static bool 提拉纳buff => Core.Me.HasAura(Data.Buffs.提拉纳预备);
    private static bool in爆发药 => Core.Me.HasAura(49) && Qt.Instance.GetQt("爆发药");

    public int Check()
    {
        if (!Qt.Instance.GetQt("剑舞") && !Core.Me.HasAura(Data.Buffs.拂晓舞预备)) return -1;
        if (!Data.Spells.剑舞.IsUnlock()) return -1;
        if (伶俐 < 50) return -1;

        //90伶俐QT
        if (Qt.Instance.GetQt("90伶俐") && 伶俐 < 90) return -2;

        //延迟控制不打拂晓舞
        if (Core.Me.HasAura(Data.Buffs.拂晓舞预备) && !Qt.Instance.GetQt("延迟技能")) return -2;

        //倾泻资源qt
        if (Qt.Instance.GetQt("倾泻资源")) return 5;

        //倾泻伶俐qt
        if (Qt.Instance.GetQt("倾泻伶俐")) return 7;

        //爆发药期间猛猛打
        if (in爆发药) return 6;

        //70伶俐就打
        if (伶俐 >= 70) return 1;

        //大舞中有就打
        if (大舞buff || 提拉纳buff) return 1;

        return -1;
    }

    private static uint GetSpells()
    {
        return Data.Spells.拂晓舞.IsUnlock() && Core.Me.HasAura(Data.Buffs.拂晓舞预备) ? 
            Data.Spells.拂晓舞 : Data.Spells.剑舞;
    }

    public void Build(Slot slot)
    {
        slot.Add(GetSpells().GetSpell());
    }
}