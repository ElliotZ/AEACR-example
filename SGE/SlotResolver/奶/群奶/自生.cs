using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.奶;

public class 自生 : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("群奶")) return -1;
        if (Core.Me.IsCasting) return -1;
        if (!Data.Spells.自生adaptive.GetSpell().IsReadyWithCanCast()) return -2;

        //判断团血
        if (PartyHelper.CastableAlliesWithin30
                .Concat([Core.Me])
                .Count(ally => 
                    ally.CurrentHpPercent() <= SgeSettings.Instance.自生阈值 / 100f) >= SgeSettings.Instance.团血检测人数)
            return 0;

        return -3;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.自生adaptive.GetSpell());
    }
}