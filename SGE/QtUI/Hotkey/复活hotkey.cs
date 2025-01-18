using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using Data = yoyokity.SGE.SlotResolver.Data;

namespace yoyokity.SGE.QtUI;

public static class 复活hotkeyWindow
{
    private static HotkeyWindow SgePartnerPanel;
    private static JobViewSave myJobViewSave;

    public static void Build(JobViewWindow instance)
    {
        instance.SetUpdateAction(() =>
        {
            PartyHelper.UpdateAllies();
            if (PartyHelper.Party.Count < 1) return;

            myJobViewSave = new JobViewSave
            {
                QtHotkeySize = new Vector2(SgeSettings.Instance.SgePartnerPanelIconSize),
                ShowHotkey = SgeSettings.Instance.SgePartnerPanelShow,
                LockWindow = SgeSettings.Instance.JobViewSave.LockWindow
            };

            SgePartnerPanel = new HotkeyWindow(myJobViewSave, "SgePartnerPanel")
            {
                HotkeyLineCount = SgeSettings.Instance.SgePartner营救 ? 2 : 1
            };

            for (var i = 1; i < PartyHelper.Party.Count; i++)
            {
                var index = i;
                var text = Data.Spells.即刻咏唱.GetSpell().IsReadyWithCanCast() ? "【即刻】复活" : "【读条】复活";
                SgePartnerPanel?.AddHotkey($"{text}{PartyHelper.Party[i].Name}", new 复活hotkey(index));

                if (SgeSettings.Instance.SgePartner营救)
                    SgePartnerPanel?.AddHotkey($"营救{PartyHelper.Party[i].Name}", new 营救hotkey(index));
            }

            SgePartnerPanel?.DrawHotkeyWindow(new QtStyle(SgeSettings.Instance.JobViewSave));
        });
    }
}

public class 复活hotkey(int index) : IHotkeyResolver
{
    private const uint SpellId = Data.Spells.复活;

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
        
        HotkeyHelper.DrawCooldownText(Data.Spells.即刻咏唱.GetSpell(), size);
    }

    public int Check()
    {
        return _check();
    }

    private int _check()
    {
        if (!PartyHelper.Party[index].IsTargetable ||
            !PartyHelper.Party[index].IsDead() ||
            Core.Me.Distance(PartyHelper.Party[index]) > SettingMgr.GetSetting<GeneralSettings>().AttackRange + 27)
            return -2;
        return 0;
    }

    public void Run()
    {
        var partyMembers = PartyHelper.Party;
        if (partyMembers.Count < index + 1)
            return;

        var slot = new Slot();
        if (Data.Spells.即刻咏唱.GetSpell().IsReadyWithCanCast())
        {
            slot.Add(Data.Spells.即刻咏唱.GetSpell());
            slot.Add(new Spell(SpellId, partyMembers[index]));
            AI.Instance.BattleData.NextSlot = slot;
        }
        else
        {
            slot.Add(new Spell(SpellId, partyMembers[index]));
            AI.Instance.BattleData.NextSlot = slot;
        }
    }
}

public class 营救hotkey : IHotkeyResolver
{
    private const uint SpellId = Data.Spells.营救;
    private IBattleChara target;
    private Spell spell;

    public 营救hotkey(int index)
    {
        target = PartyHelper.Party[index];
        spell = new Spell(SpellId, target);
    }

    public void Draw(Vector2 size)
    {
        HotkeyHelper.DrawSpellImage(size, SpellId);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        if (spell.IsReadyWithCanCast())
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

        HotkeyHelper.DrawCooldownText(spell, size);
    }

    public int Check()
    {
        if (!spell.IsReadyWithCanCast()) return -1;
        return 0;
    }

    public void Run()
    {
        var slot = new Slot();
        slot.Add(spell);
        AI.Instance.BattleData.NextSlot = slot;
    }
}