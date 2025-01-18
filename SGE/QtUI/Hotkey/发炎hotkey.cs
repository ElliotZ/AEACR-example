using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using Data = yoyokity.SGE.SlotResolver.Data;

namespace yoyokity.SGE.QtUI;

public class 发炎hotkey : IHotkeyResolver
{
    public void Draw(Vector2 size)
    {
        HotkeyHelper.DrawSpellImage(size, Data.Spells.发炎adaptive);
    }

    private static int _check()
    {
        if (!Data.Spells.发炎adaptive.GetSpell().IsReadyWithCanCast()) return -1;
        if (Core.Me.GetCurrTarget() == null) return -2;
        if (Core.Me.Distance(Core.Me.GetCurrTarget()!) >
            SettingMgr.GetSetting<GeneralSettings>().AttackRange + 3) return -3;
        return 0;
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        var spell = Data.Spells.发炎adaptive.GetSpell();
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
        HotkeyHelper.DrawChargeText(spell, size);
    }

    public int Check()
    {
        return _check();
    }

    public void Run()
    {
        AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.发炎adaptive.GetSpell());
    }
}