using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.Common;

namespace yoyokity.DNC.SlotResolver.GCD;

public class 落幕舞_高优先 : ISlotResolver
{
    public int Check()
    {
        if (!Data.Spells.落幕舞.IsUnlock()) return -1;
        if (!Core.Me.HasAura(Data.Buffs.落幕舞预备)) return -1;

        //判断是否即将过期
        if (DncHelper.AuraInGCDs(Data.Buffs.落幕舞预备, 1))
            return 1;

        //小舞前1g打出去
        if (Data.Spells.结束动作.GetSpell().Cooldown.TotalMilliseconds <= 2500 + Helper.小舞卡gcd最长时间 ||
            Data.Spells.标准舞步.GetSpell().Cooldown.TotalMilliseconds <= 2500 + Helper.小舞卡gcd最长时间)
            return 2;

        return -5;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.落幕舞.GetSpell());
    }
}