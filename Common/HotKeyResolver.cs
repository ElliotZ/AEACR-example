using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using ImGuiNET;

namespace yoyokity.Common;

public class HotKeyResolver : IHotkeyResolver
{
    private readonly uint SpellId;
    private readonly SpellTargetType TargetType;
    private readonly bool UseHighPrioritySlot;
    private readonly bool WaitCoolDown;

    /// <summary>
    /// 只使用不卡gcd的强插
    /// </summary>
    public HotKeyResolver(uint spellId, SpellTargetType targetType, bool useHighPrioritySlot = true,
        bool waitCoolDown = true)
    {
        SpellId = spellId;
        TargetType = targetType;
        UseHighPrioritySlot = useHighPrioritySlot;
        WaitCoolDown = waitCoolDown;
    }

    public void Draw(Vector2 size)
    {
        HotkeyHelper.DrawSpellImage(size, SpellId);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        var targetSpellId = Core.Resolve<MemApiSpell>().CheckActionChange(SpellId);
        var spell = targetSpellId.GetSpell(TargetType);
        
        if (WaitCoolDown)
        {
            if (spell.Cooldown.TotalMilliseconds <= 5000.0)
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
            HotkeyHelper.DrawChargeText(spell, size);
        }
        else
        {
            SpellHelper.DrawSpellInfo(spell, size, isActive);
        }
    }

    public int Check()
    {
        var s = SpellId.GetSpell(TargetType);
        var isReady = WaitCoolDown ? s.Cooldown.TotalMilliseconds <= 5000 : s.IsReadyWithCanCast();
        return isReady ? 0 : -2;
    }

    public void Run()
    {
        var targetSpellId = Core.Resolve<MemApiSpell>().CheckActionChange(SpellId);
        var spell = targetSpellId.GetSpell(TargetType);
        var cooldown = spell.Cooldown.TotalMilliseconds;

        if (WaitCoolDown && cooldown > 0)
        {
            Run1(spell, (int)cooldown);
        }
        else
        {
            Run1(spell);
        }
    }

    private async Task Run1(Spell spell, int delay = 0)
    {
        if (delay > 0) await Coroutine.Instance.WaitAsync(delay);

        if (UseHighPrioritySlot)
        {
            var slot = new Slot();
            slot.Add(spell);
            AI.Instance.BattleData.HighPrioritySlots_OffGCD.Enqueue(slot);
        }
        else
        {
            var gcdCooldown = GCDHelper.GetGCDCooldown();
            if (gcdCooldown is < 700 and > 0)
            {
                Run2(spell, gcdCooldown + 100);
            }
            else
            {
                Run2(spell);
            }
        }
    }

    private static async Task Run2(Spell spell, int delay = 0)
    {
        if (delay > 0) await Coroutine.Instance.WaitAsync(delay);
        AI.Instance.BattleData.AddSpell2NextSlot(spell);
    }
}