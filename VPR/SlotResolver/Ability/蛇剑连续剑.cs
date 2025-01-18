using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.JobGauge.Enums;

namespace yoyokity.VPR.SlotResolver.Ability;

public class 蛇剑连续剑 : ISlotResolver
{
    public int Check()
    {
        if (Core.Me.Distance(Core.Me.GetCurrTarget()!) >
            SettingMgr.GetSetting<GeneralSettings>().AttackRange + 2) return -1;

        if (VprHelper.续剑 != (SerpentCombo)7 && VprHelper.续剑 != (SerpentCombo)8) return -2;

        //防止偶发连打相同续剑
        if (Data.Spells.双牙连击.RecentlyUsed(300) || Data.Spells.双牙乱击.RecentlyUsed(300)) return -3;

        return 0;
    }

    public void Build(Slot slot)
    {
        if (VprHelper.续剑 == (SerpentCombo)7)
        {
            if (Core.Me.HasAura(Data.Buffs.连击双锐牙))
                slot.Add(Data.Spells.双牙连击.GetSpell());
            else if (Core.Me.HasAura(Data.Buffs.乱击双锐牙))
                slot.Add(Data.Spells.双牙乱击.GetSpell());
            else if (!Data.Spells.双牙连击.RecentlyUsed())
                slot.Add(Data.Spells.双牙连击.GetSpell());
            else
                slot.Add(Data.Spells.双牙乱击.GetSpell());
        }
        else
        {
            if (Core.Me.HasAura(Data.Buffs.强化双牙连闪))
                slot.Add(Data.Spells.双牙连闪.GetSpell());
            else if (Core.Me.HasAura(Data.Buffs.强化双牙乱闪))
                slot.Add(Data.Spells.双牙乱闪.GetSpell());
            else if (!Data.Spells.双牙连闪.RecentlyUsed())
                slot.Add(Data.Spells.双牙连闪.GetSpell());
            else
                slot.Add(Data.Spells.双牙乱闪.GetSpell());
        }
    }
}