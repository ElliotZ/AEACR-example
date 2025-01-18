using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using Data = yoyokity.SGE.SlotResolver.Data;

namespace yoyokity.SGE.QtUI;

public class 白牛hotkey : IHotkeyResolver
{
    private uint spellId = Data.Spells.白牛清汁;

    public void Draw(Vector2 size)
    {
        HotkeyHelper.DrawSpellImage(size, spellId);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        SpellHelper.DrawSpellInfo(spellId.GetSpell(), size, isActive);
    }

    public int Check()
    {
        return spellId.GetSpell().IsReadyWithCanCast()
            ? 0
            : -1;
    }

    public void Run()
    {
        var target = Core.Me.GetCurrTargetsTarget();
        var spell = spellId.GetSpell(target != null && spellId.GetSpell(target).IsReadyWithCanCast()
            ? target
            : Core.Me);

        var slot = new Slot();
        slot.Add(spell);
        AI.Instance.BattleData.HighPrioritySlots_OffGCD.Enqueue(slot);
    }
}