using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.Common;

namespace yoyokity.VPR.SlotResolver.GCD;

public class 祖灵连 : ISlotResolver
{
    public int Check()
    {
        //距离检测
        if (!Helper.在近战范围内) return -1;

        //在祖灵连内直接打
        if (VprHelper.祖灵连步骤 > 0) return 1;

        return -2;
    }

    private static uint GetSpell()
    {
        return VprHelper.祖灵连步骤 switch
        {
            5 => Data.Spells.祖灵之牙一式,
            4 => Data.Spells.祖灵之牙二式,
            3 => Data.Spells.祖灵之牙三式,
            2 => Data.Spells.祖灵之牙四式,
            1 => Data.Spells.祖灵大蛇牙,
            _ => 0
        };
    }

    public void Build(Slot slot)
    {
        slot.Add(GetSpell().GetSpell());

        //连打祖灵连不画
        if (VprHelper.祖灵力 >= 50) return;

        //在第四下打出后开始画身位
        if (VprHelper.祖灵连步骤 != 2 || Core.Me.HasAura(Data.Buffs.祖灵降临预备)) return;

        //基础连是combo3的时候
        if (VprHelper.基础连步骤 == 3)
        {
            if (Core.Me.HasAura(Data.Buffs.A3绿))
                MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                    VprHelper.GetDreadGcdDuration + VprHelper.GetAnguineGcdDuration, Data.Spells.A3红);
            else if (Core.Me.HasAura(Data.Buffs.B3绿))
                MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                    VprHelper.GetDreadGcdDuration + VprHelper.GetAnguineGcdDuration, Data.Spells.B3红);
            else if (Core.Me.HasAura(Data.Buffs.B3红))
                MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Flank,
                    VprHelper.GetDreadGcdDuration + VprHelper.GetAnguineGcdDuration, Data.Spells.A3绿);
            else if (Core.Me.HasAura(Data.Buffs.A3红))
                MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Flank,
                    VprHelper.GetDreadGcdDuration + VprHelper.GetAnguineGcdDuration, Data.Spells.B3绿);
            else if (VprHelper.GetLastComboSpellId == Data.Spells.A2)
                MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Flank,
                    VprHelper.GetDreadGcdDuration + VprHelper.GetAnguineGcdDuration, Data.Spells.A3绿);
            else
                MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                    VprHelper.GetDreadGcdDuration + VprHelper.GetAnguineGcdDuration, Data.Spells.B3红);
            return;
        }

        //基础连是combo2的时候
        if (VprHelper.基础连步骤 == 2)
        {
            if (Core.Me.HasAura(Data.Buffs.A3绿))
                MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                    VprHelper.GetDreadGcdDuration + VprHelper.GetAnguineGcdDuration + VprHelper.GetGcdDuration,
                    Data.Spells.A3红);
            else if (Core.Me.HasAura(Data.Buffs.B3绿))
                MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                    VprHelper.GetDreadGcdDuration + VprHelper.GetAnguineGcdDuration + VprHelper.GetGcdDuration,
                    Data.Spells.B3红);
            else if (Core.Me.HasAura(Data.Buffs.B3红))
                MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Flank,
                    VprHelper.GetDreadGcdDuration + VprHelper.GetAnguineGcdDuration + VprHelper.GetGcdDuration,
                    Data.Spells.A3绿);
            else if (Core.Me.HasAura(Data.Buffs.A3红))
                MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Flank,
                    VprHelper.GetDreadGcdDuration + VprHelper.GetAnguineGcdDuration + VprHelper.GetGcdDuration,
                    Data.Spells.B3绿);
            else if (Core.Me.HasAura(Data.Buffs.B1))
                MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                    VprHelper.GetDreadGcdDuration + VprHelper.GetAnguineGcdDuration + VprHelper.GetGcdDuration,
                    Data.Spells.B3红);
            else
                MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Flank,
                    VprHelper.GetDreadGcdDuration + VprHelper.GetAnguineGcdDuration + VprHelper.GetGcdDuration,
                    Data.Spells.A3绿);
        }
    }
}