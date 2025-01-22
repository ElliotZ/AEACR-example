using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.Common;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.GCD;

public class Dance_结束动作 : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("延迟技能")) return -1;
        if (!Data.Spells.结束动作.IsUnlock()) return -1;
        if (!Core.Me.HasAura(Data.Buffs.结束动作预备)) return -2;

        if (Data.Spells.结束动作.GetSpell().Cooldown.TotalMilliseconds > Helper.小舞卡gcd最长时间) return -3;

        //没开小舞qt但是buff快消失了打
        if (!Qt.Instance.GetQt("小舞") && DncHelper.AuraInGCDs(Data.Buffs.结束动作预备, 1)) return 2;

        if (!Qt.Instance.GetQt("小舞")) return -4;
        
        return 1;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.结束动作.GetSpell());
    }
}