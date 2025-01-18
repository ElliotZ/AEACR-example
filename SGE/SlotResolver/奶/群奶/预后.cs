using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.奶;

public class 预后 : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("群奶")) return -1;

        //判断团血
        if (PartyHelper.CastableAlliesWithin15
                .Concat([Core.Me])
                .Count(ally =>
                    ally.CurrentHpPercent() <= SgeSettings.Instance.预后阈值 / 100f) >= SgeSettings.Instance.团血检测人数)
            return 0;

        return -3;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.预后.GetSpell());
    }
}