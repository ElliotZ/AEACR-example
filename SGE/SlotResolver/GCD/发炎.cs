using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using yoyokity.Common;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.GCD;

public class 发炎 : ISlotResolver
{
    private IBattleChara? Target { get; set; }

    public int Check()
    {
        if (!Qt.Instance.GetQt("发炎")) return -1;
        if (BattleData.Instance.Lock发炎) return -1;
        if (!Data.Spells.发炎adaptive.GetSpell().IsReadyWithCanCast()) return -2;

        Target = null;

        //距离判断
        if (Core.Me.Distance(Core.Me.GetCurrTarget()!) >
            SettingMgr.GetSetting<GeneralSettings>().AttackRange + 3) return -2;

        //开局3个gcd内不打
        if (!SgeSettings.Instance.没妈妈自嗨打法 &&
            AI.Instance.BattleData.CurrBattleTimeInMs <= 2500 * 3) return -3;

        //在团辅中就打
        if (Helper.In团辅()) return 1;

        //在爆发药中打
        if (Core.Me.HasAura(49)) return 2;

        //倾泻资源
        if (Qt.Instance.GetQt("倾泻资源")) return 3;

        //快溢出了打
        if (Data.Spells.发炎adaptive.GetSpell().Cooldown.TotalMilliseconds < 2000) return 4;

        //周围多余一个人就打
        Target = Data.Spells.发炎adaptive.最优aoe目标(2);
        if (Qt.Instance.GetQt("AOE") && Target != null)
            return 5;

        return -1;
    }

    public void Build(Slot slot)
    {
        var spell = Target == null
            ? Data.Spells.发炎adaptive.GetSpell()
            : Data.Spells.发炎adaptive.GetSpell(Target);
        slot.Add(spell);
    }
}