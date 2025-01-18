using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using Data = yoyokity.SGE.SlotResolver.Data;

namespace yoyokity.SGE.QtUI;

public class 群盾hotkey : IHotkeyResolver
{
    public void Draw(Vector2 size)
    {
        HotkeyHelper.DrawSpellImage(size, Data.Spells.均衡预后adaptive);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        var spell = Data.Spells.活化.GetSpell();
        if (isActive)
        {
            HotkeyHelper.DrawActiveState(size);
        }
        else
        {
            HotkeyHelper.DrawGeneralState(size);
        }

        HotkeyHelper.DrawCooldownText(spell, size);
    }

    public int Check()
    {
        //判断自身是否有盾的重叠
        if (Core.Me.HasAnyAura([297, Data.Buffs.均衡预后])) return -5;
        return 0;
    }

    public void Run()
    {
        if (GCDHelper.GetGCDCooldown() <= 0)
        {
            var slot = new Slot();
            slot.Add(Data.Spells.均衡.GetSpell());
            if (Data.Spells.活化.GetSpell().IsUnlock() && Data.Spells.活化.GetSpell().Cooldown.TotalMilliseconds <= 1000)
                slot.Add(Data.Spells.活化.GetSpell());
            slot.Add(Data.Spells.均衡预后adaptive.GetSpell());
            AI.Instance.BattleData.NextSlot = slot;
        }
        else
        {
            if (SgeHelper.均衡中)
                群盾活化(GCDHelper.GetGCDCooldown() + 100);
            else
                群盾活化();
        }
    }

    private static async Task 群盾活化(int delay = 0)
    {
        if (delay > 0) await Coroutine.Instance.WaitAsync(delay);
        AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.均衡.GetSpell());
        if (Data.Spells.活化.GetSpell().IsUnlock() && Data.Spells.活化.GetSpell().Cooldown.TotalMilliseconds <= 1000)
            AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.活化.GetSpell());
        AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.均衡预后adaptive.GetSpell());
    }
}