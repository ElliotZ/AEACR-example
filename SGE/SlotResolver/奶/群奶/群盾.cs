using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.Common;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.奶;

public class 群盾 : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("群奶")) return -1;
        if (!Data.Spells.均衡.GetSpell().IsUnlock()) return -2;

        //判断自身是否有盾的重叠
        if (Core.Me.HasAnyAura([297, Data.Buffs.均衡预后])) return -5;

        //判断团血
        if (PartyHelper.CastableAlliesWithin15
                .Concat([Core.Me])
                .Count(ally =>
                    ally.CurrentHpPercent() <= SgeSettings.Instance.群盾阈值 / 100f) >= SgeSettings.Instance.团血检测人数)
            return 0;

        return -3;
    }

    private class 均衡预后 : ISlotSequence
    {
        public List<Action<Slot>> Sequence =>
            [(Slot slot) => slot.Add(Helper.GetActionChange(Data.Spells.预后).GetSpell())];
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.均衡.GetSpell());
        slot.AppendSequence(new 均衡预后());
    }
}