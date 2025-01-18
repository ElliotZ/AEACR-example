using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.JobGauge.Enums;
using yoyokity.Common;

namespace yoyokity.VPR.SlotResolver.GCD;

public class 蛇剑连 : ISlotResolver
{
    private static bool 优先攻速 = true;

    public int Check()
    {
        if (!Helper.在近战范围内) return -1;

        //处于蛇剑连中打
        if (VprHelper.蛇剑连 != 0) return 1;

        //没有蛇剑不打
        if (!Data.Spells.蛇剑1.GetSpell().IsReadyWithCanCast()) return -2;

        //连击快断了不打
        if (Helper.连击剩余时间 is < 9580 and > 0) return -5;

        //刚打完祖灵连2g不打
        if (Data.Spells.祖灵大蛇牙.GetSpell()
            .RecentlyUsed(VprHelper.GetDreadGcdDuration + VprHelper.GetGcdDuration)) return -3;

        //蛇灵气即将转好不打
        if (Data.Spells.蛇灵气.GetSpell().Cooldown.TotalMilliseconds < VprHelper.GetGcdDuration * 2 &&
            Data.Spells.蛇灵气.GetSpell().Cooldown.TotalMilliseconds > 0) return -4;

        return 0;
    }

    private static uint GetSpell()
    {
        return VprHelper.蛇剑连 switch
        {
            //combo3
            DreadCombo.HuntersCoil => Data.Spells.蛇剑攻速,
            DreadCombo.SwiftskinsCoil => Data.Spells.蛇剑攻击,
            //combo2
            DreadCombo.Dreadwinder => 优先攻速 ? Data.Spells.蛇剑攻速 : Data.Spells.蛇剑攻击,
            _ => Data.Spells.蛇剑攻速
        };
    }

    public void Build(Slot slot)
    {
        if (VprHelper.蛇剑连 == 0)
            优先攻速 = VprHelper.蛇剑连优先攻速(VprHelper.GetDreadGcdDuration + VprHelper.GetGcdDuration);
        slot.Add(VprHelper.蛇剑连 == 0 ? Data.Spells.蛇剑1.GetSpell() : GetSpell().GetSpell());


        //身位显示
        //combo1根据最近一次身位，就决定好后面先打哪个
        if (VprHelper.蛇剑连 == 0)
        {
            var pos = 优先攻速 ? MeleePosHelper.Pos.Behind : MeleePosHelper.Pos.Flank;
            if (MeleePosHelper2.InQueue(pos)) return;
            MeleePosHelper2.DrawMeleePosOffset(pos, (int)(VprHelper.GetGcdDuration * 3 / 2.5),
                优先攻速 ? Data.Spells.蛇剑攻速 : Data.Spells.蛇剑攻击);
            return;
        }

        //combo2
        if (VprHelper.蛇剑连 == DreadCombo.Dreadwinder)
        {
            MeleePosHelper2.DrawMeleePosOffset(优先攻速 ? MeleePosHelper.Pos.Flank : MeleePosHelper.Pos.Behind,
                VprHelper.GetDreadGcdDuration, 优先攻速 ? Data.Spells.蛇剑攻击 : Data.Spells.蛇剑攻速);
            return;
        }

        //combo3
        if (VprHelper.蛇剑连 is DreadCombo.HuntersCoil or DreadCombo.SwiftskinsCoil)
        {
            if (Core.Me.HasAura(Data.Buffs.祖灵降临预备)) return;
            //下个依然是蛇剑连的情况
            if (Helper.充能技能冷却时间(Data.Spells.蛇剑1) < VprHelper.GetDreadGcdDuration)
            {
                var time = VprHelper.GetDreadGcdDuration * 2;
                MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper2.LastPos, time,
                    !优先攻速 ? Data.Spells.蛇剑攻速 : Data.Spells.蛇剑攻击);
                return;
            }

            //基础连是combo3的时候
            if (VprHelper.基础连步骤 == 3)
            {
                if (Core.Me.HasAura(Data.Buffs.A3绿))
                    MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                        VprHelper.GetDreadGcdDuration, Data.Spells.A3红);
                else if (Core.Me.HasAura(Data.Buffs.B3绿))
                    MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                        VprHelper.GetDreadGcdDuration, Data.Spells.B3红);
                else if (Core.Me.HasAura(Data.Buffs.B3红))
                    MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Flank,
                        VprHelper.GetDreadGcdDuration, Data.Spells.A3绿);
                else if (Core.Me.HasAura(Data.Buffs.A3红))
                    MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Flank,
                        VprHelper.GetDreadGcdDuration, Data.Spells.B3绿);
                else if (VprHelper.GetLastComboSpellId == Data.Spells.A2)
                    MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Flank,
                        VprHelper.GetDreadGcdDuration, Data.Spells.A3绿);
                else
                    MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                        VprHelper.GetDreadGcdDuration, Data.Spells.B3红);
                return;
            }

            //基础连是combo2的时候
            if (VprHelper.基础连步骤 == 2)
            {
                if (Core.Me.HasAura(Data.Buffs.A3绿))
                    MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                        VprHelper.GetDreadGcdDuration + VprHelper.GetGcdDuration, Data.Spells.A3红);
                else if (Core.Me.HasAura(Data.Buffs.B3绿))
                    MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                        VprHelper.GetDreadGcdDuration + VprHelper.GetGcdDuration, Data.Spells.B3红);
                else if (Core.Me.HasAura(Data.Buffs.B3红))
                    MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Flank,
                        VprHelper.GetDreadGcdDuration + VprHelper.GetGcdDuration, Data.Spells.A3绿);
                else if (Core.Me.HasAura(Data.Buffs.A3红))
                    MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Flank,
                        VprHelper.GetDreadGcdDuration + VprHelper.GetGcdDuration, Data.Spells.B3绿);
                else if (Core.Me.HasAura(Data.Buffs.B1))
                    MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Behind,
                        VprHelper.GetDreadGcdDuration + VprHelper.GetGcdDuration, Data.Spells.B3红);
                else
                    MeleePosHelper2.DrawMeleePosOffset(MeleePosHelper.Pos.Flank,
                        VprHelper.GetDreadGcdDuration + VprHelper.GetGcdDuration, Data.Spells.A3绿);
            }
        }
    }
}