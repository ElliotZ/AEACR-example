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

    private class 扇舞序 : ISlotSequence
    {
        public List<Action<Slot>> Sequence => [(Slot slot) => slot.Add(Data.Spells.扇舞序.GetSpell())];
    }

    public int Check()
    {
        //大舞前1G
        if (Data.Spells.大舞.GetSpell().Cooldown.TotalMilliseconds > 7000 - 2500) return -1;

        //打1
        if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == Data.Spells.瀑泻) return -2;

        return 0;
    }

    private static uint GetSpell()
    {
        //有剑舞优先剑舞
        if (伶俐 >= 50)
            return Data.Spells.剑舞;

        //没有打落幕舞
        if (Core.Me.HasAura(Data.Buffs.落幕舞预备))
            return Data.Spells.落幕舞;

        return Data.Spells.瀑泻;
    }

    public void Build(Slot slot)
    {
        var spell = GetSpell();
        slot.Add(spell.GetSpell());
        if (幻扇 == 4 && spell == Data.Spells.落幕舞) //进大舞时没有剑舞落幕舞，扇舞有4个时泄掉一个
            slot.AppendSequence(new 扇舞序(), false);
    }
}