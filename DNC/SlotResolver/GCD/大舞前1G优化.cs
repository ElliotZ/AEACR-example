using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;

namespace yoyokity.DNC.SlotResolver.GCD;

public class 大舞前1G优化 : ISlotResolver
{
    private static int 伶俐 => Core.Resolve<JobApi_Dancer>().Esprit;
    private static int 幻扇 => Core.Resolve<JobApi_Dancer>().FourFoldFeathers;

    public int Check()
    {
        //大舞前1G
        if (!Data.Spells.剑舞.IsUnlock()) return -1;
        if (Data.Spells.大舞.GetSpell().Cooldown.TotalMilliseconds > 7000 - 2500) return -1;

        //打1
        if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == Data.Spells.瀑泻) return -2;

        return 0;
    }

    private static uint GetSpells()
    {
        //有剑舞优先剑舞
        if (伶俐 >= 50)
            return Data.Spells.剑舞;

        //没有打落幕舞
        if (Data.Spells.落幕舞.IsUnlock() && Core.Me.HasAura(Data.Buffs.落幕舞预备))
            return Data.Spells.落幕舞;

        return Data.Spells.瀑泻;
    }

    public void Build(Slot slot)
    {
        var spell = GetSpells();
        slot.Add(spell.GetSpell());

        //进大舞时没有剑舞落幕舞，扇舞有4个时泄掉一个
        if (Data.Spells.百花.IsUnlock() && 幻扇 == 4 && 
            (spell == Data.Spells.落幕舞 || (spell == Data.Spells.剑舞 && !Data.Spells.落幕舞.IsUnlock())))
            slot.Add(Data.Spells.扇舞序.GetSpell());
    }
}