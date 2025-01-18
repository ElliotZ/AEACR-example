using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.奶;

public class 混合 : ISlotResolver
{
    private static IBattleChara target;

    public int Check()
    {
        if (!Data.Spells.混合.GetSpell().IsReadyWithCanCast()) return -2;

        //死而不僵直接给
        if (SgeSettings.Instance.T无敌给混合)
        {
            var t = PartyHelper.Party.Find(p =>
                p.HasAnyAura([Data.Buffs.死而不僵, Data.Buffs.死斗, Data.Buffs.超火流星]));
            if (t != null)
            {
                target = t;
                return 1;
            }
        }

        if (!Qt.Instance.GetQt("单奶")) return -1;

        //检测心关对象
        if (SgeHelper.心关目标 == null) return -3;
        if (SgeHelper.心关目标.CurrentHpPercent() > SgeSettings.Instance.混合阈值 / 100f) return -5;

        //活死人不给
        if (SgeHelper.心关目标.HasAura(Data.Buffs.活死人)) return -6;

        target = SgeHelper.心关目标;
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(new Spell(Data.Spells.混合, target));
    }
}