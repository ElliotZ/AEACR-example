using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.Ability;

public class 扇舞序 : ISlotResolver
{
    private static int 幻扇 => Core.Resolve<JobApi_Dancer>().FourFoldFeathers;
    private static bool 大舞buff => Core.Me.HasAura(Data.Buffs.大舞Buff);
    private static bool in爆发药 => Core.Me.HasAura(49) && Qt.Instance.GetQt("爆发药");

    public int Check()
    {
        if (!Data.Spells.扇舞序.IsUnlock()) return -1;
        if (!Qt.Instance.GetQt("扇舞")) return -1;
        if (幻扇 <= 0) return -1;
        if (Core.Resolve<JobApi_Dancer>().IsDancing) return -1;

        //大舞前0g不打
        if (Data.Spells.大舞.GetSpell().Cooldown.TotalMilliseconds < 2500) return -5;

        //大舞跳完1g内不打
        if (Data.Spells.四色大舞结束.RecentlyUsed(1500)) return -4;

        //百花前0g以及百花亮着就不打
        if (Data.Spells.百花.GetSpell().Cooldown.TotalMilliseconds < 2500) return -6;

        //爆发药期间猛猛打
        if (in爆发药) return 1;

        //大舞期间有就打
        if (大舞buff) return 1;

        //倾泻资源qt
        if (Qt.Instance.GetQt("倾泻资源")) return 5;

        //60秒的百花亮之前，只攒2个，以防止百花时溢出
        if (!大舞buff && 幻扇 > 2
                    && Data.Spells.百花.GetSpell().Cooldown.TotalMilliseconds < 7 * 1000)
            return 3;

        //平时攒够4个，且逆瀑泻或坠喷泉亮了打
        if (幻扇 < 4) return -2;
        if (Core.Me.HasAura(Data.Buffs.普通3预备) || Core.Me.HasAura(Data.Buffs.百花3预备) ||
            Core.Me.HasAura(Data.Buffs.普通4预备) || Core.Me.HasAura(Data.Buffs.百花4预备))
            return 2;

        return -3;
    }

    private static uint GetSpells()
    {
        var enemyCount = TargetHelper.GetNearbyEnemyCount(5);

        if (Qt.Instance.GetQt("AOE") && Data.Spells.扇舞破.IsUnlock() &&
            enemyCount >= 3)
            return Data.Spells.扇舞破;

        return Data.Spells.扇舞序;
    }

    public void Build(Slot slot)
    {
        slot.Add(GetSpells().GetSpell());
    }
}