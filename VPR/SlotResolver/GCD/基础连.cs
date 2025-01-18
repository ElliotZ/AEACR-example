using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.Common;

namespace yoyokity.VPR.SlotResolver.GCD;

public class 基础连 : ISlotResolver
{
    public int Check()
    {
        if (!Helper.在近战范围内) return -1;
        return 0;
    }

    private static uint GetSpell()
    {
        if (VprHelper.基础连步骤 == 3)
        {
            if (Core.Me.HasAura(Data.Buffs.A3绿))
                return Data.Spells.A3红;
            if (Core.Me.HasAura(Data.Buffs.B3绿))
                return Data.Spells.B3红;
            if (Core.Me.HasAura(Data.Buffs.B3红))
                return Data.Spells.A3绿;
            if (Core.Me.HasAura(Data.Buffs.A3红))
                return Data.Spells.B3绿;

            return VprHelper.GetLastComboSpellId == Data.Spells.A2 ? Data.Spells.A3绿 : Data.Spells.B3红;
        }

        if (VprHelper.基础连步骤 == 2)
        {
            if (Core.Me.HasAura(Data.Buffs.A3红) || Core.Me.HasAura(Data.Buffs.B3红))
                return Data.Spells.A2;
            if (Core.Me.HasAura(Data.Buffs.A3绿) || Core.Me.HasAura(Data.Buffs.B3绿))
                return Data.Spells.B2;
            return Core.Me.HasAura(Data.Buffs.B1) ? Data.Spells.B2 : Data.Spells.A2;
        }

        return Core.Me.HasAura(Data.Buffs.B1) ? Data.Spells.A1 : Data.Spells.B1;
    }

    public void Build(Slot slot)
    {
        slot.Add(GetSpell().GetSpell());

        //身位显示
        if (VprHelper.基础连步骤 != 1 || Core.Me.HasAura(Data.Buffs.祖灵降临预备)) return;

        //接下来2g内可开蛇灵气不显示身位
        if (Data.Spells.蛇灵气.GetSpell().Cooldown.TotalMilliseconds < VprHelper.GetGcdDuration * 2 &&
            Data.Spells.蛇灵气.GetSpell().Cooldown.TotalMilliseconds > 0)
            return;

        //接下来1G蛇剑连
        if (Helper.充能技能冷却时间(Data.Spells.蛇剑1) < VprHelper.GetGcdDuration &&
            !Data.Spells.祖灵大蛇牙.GetSpell().RecentlyUsed(VprHelper.GetDreadGcdDuration))
        {
            var 优先攻速 = VprHelper.蛇剑连优先攻速(VprHelper.GetDreadGcdDuration + VprHelper.GetGcdDuration * 2);
            MeleePosHelper2.DrawMeleePosOffset(优先攻速 ? MeleePosHelper.Pos.Behind : MeleePosHelper.Pos.Flank,
                VprHelper.GetDreadGcdDuration + VprHelper.GetGcdDuration, 优先攻速 ? Data.Spells.蛇剑攻速 : Data.Spells.蛇剑攻击);
            return;
        }

        //接下来2G蛇剑连
        if (Helper.充能技能冷却时间(Data.Spells.蛇剑1) < VprHelper.GetGcdDuration * 2)
        {
            var 优先攻速 = VprHelper.蛇剑连优先攻速(VprHelper.GetDreadGcdDuration + VprHelper.GetGcdDuration * 3);
            MeleePosHelper2.DrawMeleePosOffset(优先攻速 ? MeleePosHelper.Pos.Behind : MeleePosHelper.Pos.Flank,
                VprHelper.GetDreadGcdDuration + VprHelper.GetGcdDuration * 2,
                优先攻速 ? Data.Spells.蛇剑攻速 : Data.Spells.蛇剑攻击);
            return;
        }

        //打出1的时候身位显示
        if (Core.Me.HasAura(Data.Buffs.A3绿))
            MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                VprHelper.GetGcdDuration * 2, Data.Spells.A3红);
        else if (Core.Me.HasAura(Data.Buffs.B3绿))
            MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                VprHelper.GetGcdDuration * 2, Data.Spells.B3红);
        else if (Core.Me.HasAura(Data.Buffs.B3红))
            MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Flank,
                VprHelper.GetGcdDuration * 2, Data.Spells.A3绿);
        else if (Core.Me.HasAura(Data.Buffs.A3红))
            MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Flank,
                VprHelper.GetGcdDuration * 2, Data.Spells.B3绿);
        else
            MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                VprHelper.GetGcdDuration * 2, Data.Spells.B3红);
    }
}