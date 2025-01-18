using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.Common;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.奶;

public class 活化贤炮 : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("群奶")) return -1;
        if (Helper.IsMove) return -1;
        if (Core.Me.IsCasting) return -1;
        if (!Data.Spells.活化.GetSpell().IsReadyWithCanCast()) return -2;
        if (!Data.Spells.贤炮.GetSpell().IsReadyWithCanCast()) return -2;

        //判断团血
        if (PartyHelper.CastableAlliesWithin15
                .Concat([Core.Me])
                .Count(ally => 
                    ally.CurrentHpPercent() <= SgeSettings.Instance.活化魂灵风息阈值 / 100f) >= SgeSettings.Instance.团血检测人数)
            return 0;

        return -3;
    }
    
    private class 贤炮 : ISlotSequence
    {
        public List<Action<Slot>> Sequence =>
        [(Slot slot) => slot.Add(Data.Spells.贤炮.GetSpell())];
    }


    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.活化.GetSpell());
        slot.AppendSequence(new 贤炮());
    }
}