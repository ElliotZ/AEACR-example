using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using Data = yoyokity.SGE.SlotResolver.Data;

namespace yoyokity.SGE.QtUI;

public class 贤炮hotkey : IHotkeyResolver
{
    public void Draw(Vector2 size)
    {
        HotkeyHelper.DrawSpellImage(size, Data.Spells.贤炮);
    }

    private static int _check()
    {
        if (!Data.Spells.贤炮.GetSpell().IsReadyWithCanCast()) return -1;
        if (Core.Me.GetCurrTarget() == null) return -2;
        return 0;
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        var spell = Data.Spells.贤炮.GetSpell();
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

        HotkeyHelper.DrawCooldownText(spell, size);
    }

    public int Check()
    {
        return _check();
    }

    public void Run()
    {
        var gcdCooldown = GCDHelper.GetGCDCooldown();
        if (Data.Spells.活化.GetSpell().IsReadyWithCanCast())
        {
            if (gcdCooldown is < 700 and > 0 || SgeHelper.均衡中)
            {
                贤炮(gcdCooldown + 100, true);
            }
            else
            {
                贤炮(活化: true);
            }
        }
        else
        {
            if (SgeHelper.均衡中)
            {
                贤炮(gcdCooldown + 100);
            }
            else
            {
                贤炮();
            }
        }
    }

    private static async Task 贤炮(int delay = 0, bool 活化 = false)
    {
        if (delay > 0) await Coroutine.Instance.WaitAsync(delay);
        if (活化)
            AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.活化.GetSpell());
        AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.贤炮.GetSpell());
    }
}