using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.奶;

public class 白牛 : ISlotResolver
{
    private static IBattleChara _target;

    public int Check()
    {
        if (!Qt.Instance.GetQt("单奶")) return -1;
        if (Core.Me.IsCasting) return -1;
        if (!Data.Spells.白牛清汁.GetSpell().IsReadyWithCanCast()) return -2;
        
        if (Qt.Instance.GetQt("保留1蓝豆") && SgeHelper.蓝豆 <= 1) return -6;

        //获取符合血量阈值的人
        var list = PartyHelper.CastableAlliesWithin30
            .Concat([Core.Me])
            .Where(ally => ally.CurrentHpPercent() <= SgeSettings.Instance.白牛阈值 / 100f)
            .ToList();
        if (list.Count > 2) return -3; //超过两个人就不单奶
        var target = list
            .OrderBy(ally => ally.CurrentHpPercent())
            .FirstOrDefault();
        if (target == null) return -4;
        
        //活死人不给
        if (target.HasAura(Data.Buffs.活死人)) return -5;

        _target = target;
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(new Spell(Data.Spells.白牛清汁, _target));
    }
}