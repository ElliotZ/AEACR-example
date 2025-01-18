using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.Common;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.GCD;

public class Dot : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("Dot")) return -1;

        //dot刚用过
        if (Data.Spells.均衡注药.RecentlyUsed(5000) ||
            Data.Spells.均衡注药2.RecentlyUsed(5000) ||
            Data.Spells.均衡注药3.RecentlyUsed(5000)) return -2;

        //不在均衡状态中
        if (Core.Me.HasAura(Data.Buffs.均衡)) return -3;

        //如果boss血量小于1.5%了就不要补dot了
        if (Core.Me.GetCurrTarget()?.CurrentHpPercent() <= 0.015f) return -4;

        //dot快消失或者没有
        List<uint> buffs = [Data.Buffs.均衡注药, Data.Buffs.均衡注药2, Data.Buffs.均衡注药3, Data.Buffs.均衡失衡];
        if (!Helper.目标有任意我的buff(buffs)) return 1;
        if (buffs.Any(buff => Helper.目标Buff时间小于(buff, 3000)))
            return 2;

        return -5;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.均衡.GetSpell());
        slot.Add(Data.Spells.均衡注药adaptive.GetSpell());
    }
}