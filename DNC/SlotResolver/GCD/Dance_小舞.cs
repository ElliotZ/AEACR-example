using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.Common;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.GCD;

public class Dance_小舞 : ISlotResolver
{
    private static bool 大舞buff => Core.Me.HasAura(Data.Buffs.大舞Buff);

    public int Check()
    {
        if (!Qt.Instance.GetQt("小舞")) return -1;
        if (!Data.Spells.标准舞步.IsUnlock()) return -1;
        if (Core.Me.HasAura(Data.Buffs.结束动作预备)) return -2;

        if (Data.Spells.标准舞步.GetSpell().Cooldown.TotalMilliseconds > Helper.小舞卡gcd最长时间) return -3;

        //大舞中且百花亮着或cd小于五秒不打
        if (大舞buff && Data.Spells.百花.GetSpell().Cooldown.TotalMilliseconds < 5000) return -5;

        //大舞前6秒不打
        if (Data.Spells.大舞.GetSpell().Cooldown.TotalMilliseconds <= 6000) return -4;

        return 1;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.标准舞步.GetSpell());
    }
}