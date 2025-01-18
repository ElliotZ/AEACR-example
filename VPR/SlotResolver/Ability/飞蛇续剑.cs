using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.JobGauge.Enums;

namespace yoyokity.VPR.SlotResolver.Ability;

public class 飞蛇续剑 : ISlotResolver
{
    public int Check()
    {
        if (VprHelper.续剑 != (SerpentCombo)9) return -2;
        return 0;
    }

    public void Build(Slot slot)
    {
        if (Core.Me.HasAura(Data.Buffs.强化飞蛇连尾击))
            slot.Add(Data.Spells.飞蛇连尾击.GetSpell());
        else if (Core.Me.HasAura(Data.Buffs.强化飞蛇乱尾击))
            slot.Add(Data.Spells.飞蛇乱尾击.GetSpell());
        else if (!Data.Spells.飞蛇连尾击.RecentlyUsed())
            slot.Add(Data.Spells.飞蛇连尾击.GetSpell());
        else
            slot.Add(Data.Spells.飞蛇乱尾击.GetSpell());
    }
}