using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using Data = yoyokity.DNC.SlotResolver.Data;

namespace yoyokity.DNC.QtUI;

public static class 舞伴hotkeyWindow
{
    public static HotkeyWindow DancePartnerPanel;
    private static JobViewSave myJobViewSave;

    public static void Build(JobViewWindow instance)
    {
        instance.SetUpdateAction(() =>
        {
            PartyHelper.UpdateAllies();
            if (PartyHelper.Party.Count < 1) return;

            myJobViewSave = new JobViewSave
            {
                QtHotkeySize = new Vector2(DncSettings.Instance.DancePartnerPanelIconSize),
                ShowHotkey = DncSettings.Instance.DancePartnerPanelShow
            };

            DancePartnerPanel = new HotkeyWindow(myJobViewSave, "DancePartnerPanel")
            {
                HotkeyLineCount = 1
            };

            for (var i = 1; i < PartyHelper.Party.Count; i++)
            {
                var index = i;
                DancePartnerPanel?.AddHotkey("切换舞伴: " + PartyHelper.Party[i].Name, new 舞伴hotkey(index));
            }

            DancePartnerPanel?.DrawHotkeyWindow(new QtStyle(DncSettings.Instance.JobViewSave));
        });
    }
}

public class 舞伴hotkey(int index) : IHotkeyResolver
{
    private const uint SpellId = Data.Spells.给舞伴;

    public void Draw(Vector2 size)
    {
        HotkeyHelper.DrawSpellImage(size, SpellId);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        if (_check() >= 0)
        {
            if (isActive)
            {
                HotkeyHelper.DrawActiveState(size);
            }
            else
            {
                HotkeyHelper.DrawGeneralState(size);
            }
        }
        else
        {
            HotkeyHelper.DrawDisabledState(size);
        }

        HotkeyHelper.DrawCooldownText(SpellId.GetSpell(), size);
    }

    public int Check()
    {
        return _check();
    }

    private int _check()
    {
        if (SpellId.GetSpell().Cooldown.TotalMilliseconds > 0 ||
            Core.Resolve<JobApi_Dancer>().IsDancing ||
            !PartyHelper.Party[index].IsTargetable ||
            PartyHelper.Party[index].IsDead() ||
            Core.Me.Distance(PartyHelper.Party[index]) > SettingMgr.GetSetting<GeneralSettings>().AttackRange + 27)
            return -2;
        return 0;
    }

    public void Run()
    {
        var partyMembers = PartyHelper.Party;
        if (partyMembers.Count < index + 1)
            return;

        if (GCDHelper.GetGCDCooldown() <= 0)
        {
            AI.Instance.BattleData.NextSlot ??= new Slot();
            if (Core.Me.HasAura(Data.Buffs.舞伴buff))
            {
                AI.Instance.BattleData.NextSlot.Add(Data.Spells.解除舞伴.GetSpell());
                AI.Instance.BattleData.NextSlot.Add(new Spell(SpellId, partyMembers[index]));
            }
            else
            {
                AI.Instance.BattleData.NextSlot.Add(new Spell(SpellId, partyMembers[index]));
            }
        }
        else
        {
            if (Core.Me.HasAura(Data.Buffs.舞伴buff))
            {
                AI.Instance.BattleData.HighPrioritySlots_OffGCD
                    .Enqueue(new Slot(Data.Spells.解除舞伴.GetSpell(), 2500));
                AI.Instance.BattleData.HighPrioritySlots_OffGCD
                    .Enqueue(new Slot(new Spell(SpellId, partyMembers[index]), 2500));
            }
            else
            {
                AI.Instance.BattleData.HighPrioritySlots_OffGCD
                    .Enqueue(new Slot(new Spell(SpellId, partyMembers[index]), 2500));
            }
        }
    }
}