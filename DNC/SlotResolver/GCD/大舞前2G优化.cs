using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using yoyokity.Common;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.GCD;

public class 大舞前2G优化 : ISlotResolver
{
    private static int 伶俐 => Core.Resolve<JobApi_Dancer>().Esprit;
    private static int 幻扇 => Core.Resolve<JobApi_Dancer>().FourFoldFeathers;

    public int Check()
    {
        //大舞前2G
        if (!Data.Spells.剑舞.IsUnlock() || !Data.Spells.落幕舞.IsUnlock()) return -1;
        if (Data.Spells.大舞.GetSpell().Cooldown.TotalMilliseconds > 7000) return -1;

        //原本要打1
        if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == Data.Spells.瀑泻) return -2;

        //有剑舞+落幕舞
        if (伶俐 < 50 || !Core.Me.HasAura(Data.Buffs.落幕舞预备)) return -3;

        return 1;
    }

    public void Build(Slot slot)
    {
        var target = Data.Spells.剑舞.最优aoe目标(2);
        var spell = !Qt.Instance.GetQt("AOE") || target == null
            ? Data.Spells.剑舞.GetSpell()
            : Data.Spells.剑舞.GetSpell(target);
        var spell2 = !Qt.Instance.GetQt("AOE") || target == null
            ? Data.Spells.落幕舞.GetSpell()
            : Data.Spells.落幕舞.GetSpell(target);

        slot.Add(spell);

        if (幻扇 == 4)
            //进大舞时没有剑舞落幕舞，扇舞有4个时泄掉一个
        {
            slot.Add(spell2);
            slot.Add(Data.Spells.扇舞序.GetSpell());
        }
        else
            slot.Add(spell2);
    }
}