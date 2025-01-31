using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.Common;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.GCD;

public class 落幕舞 : ISlotResolver
{
    private static bool 大舞buff => Core.Me.HasAura(Data.Buffs.大舞Buff);
    private static bool in爆发药 => Core.Me.HasAura(49) && Qt.Instance.GetQt("爆发药");

    public int Check()
    {
        if (!Data.Spells.落幕舞.GetSpell().IsReadyWithCanCast()) return -1;
        if (!Core.Me.HasAura(Data.Buffs.落幕舞预备)) return -1;

        //爆发药期间猛猛打
        if (in爆发药) return 1;

        //倾泻资源qt
        if (Qt.Instance.GetQt("倾泻资源")) return 5;

        //倾泻伶俐qt
        if (Qt.Instance.GetQt("倾泻伶俐")) return 7;

        if (!大舞buff) return -2;

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.落幕舞.GetSpell());
    }
}