using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using Data = yoyokity.SGE.SlotResolver.Data;

namespace yoyokity.SGE.QtUI;

public class 箭毒hotkey : IHotkeyResolver
{
    public void Draw(Vector2 size)
    {
        if (_check() == 1)
        {
            HotkeyHelper.DrawSpellImage(size, Data.Spells.即刻咏唱);
            return;
        }

        HotkeyHelper.DrawSpellImage(size, Data.Spells.箭毒adaptive);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        switch (_check())
        {
            case < 0:
                HotkeyHelper.DrawDisabledState(size);
                return;
            case 1:
                HotkeyHelper.DrawSpellInfo(Data.Spells.即刻咏唱.GetSpell(),size,isActive);
                return;
            case 0:
                HotkeyHelper.DrawSpellInfo(Data.Spells.箭毒adaptive.GetSpell(),size,isActive);
                return;
        }
    }

    public int Check()
    {
        return _check();
    }

    private static int _check()
    {
        var isTargetNull = Core.Me.GetCurrTarget() == null;
        var isNoRedBean = SgeHelper.红豆 <= 0;
        var isInstantSpellReady = Data.Spells.即刻咏唱.GetSpell().IsReadyWithCanCast();

        if (SgeSettings.Instance.没有箭毒打即刻)
        {
            if (isNoRedBean)
            {
                return isInstantSpellReady ? 1 : -2;
            }
            return isTargetNull ? -1 : 0;
        }

        if (isTargetNull) return -1;
        return isNoRedBean ? -2 : 0;
    }


    public void Run()
    {
        if (_check() == 0)
        {
            AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.箭毒adaptive.GetSpell());
        }
        else
        {
            var gcdCooldown = GCDHelper.GetGCDCooldown();
            if (gcdCooldown is < 700 and > 0)
            {
                Run2(Data.Spells.即刻咏唱.GetSpell(), gcdCooldown + 100);
            }
            else
            {
                Run2(Data.Spells.即刻咏唱.GetSpell());
            }
        }
    }

    private static async Task Run2(Spell spell, int delay = 0)
    {
        if (delay > 0) await Coroutine.Instance.WaitAsync(delay);
        AI.Instance.BattleData.AddSpell2NextSlot(spell);
    }
}