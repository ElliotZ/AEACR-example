using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using ImGuiNET;
using Data = yoyokity.SGE.SlotResolver.Data;

namespace yoyokity.SGE.QtUI;

public class 消化hotkey : IHotkeyResolver
{
    public void Draw(Vector2 size)
    {
        var id = Core.Resolve<MemApiSpell>().CheckActionChange(Data.Spells.消化);
        var size1 = size * 0.8f;
        ImGui.SetCursorPos(size * 0.1f);
        if (!Core.Resolve<MemApiIcon>().GetActionTexture(id, out var textureWrap))
            return;
        ImGui.Image(textureWrap.ImGuiHandle, size1);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        SpellHelper.DrawSpellInfo(Data.Spells.消化.GetSpell(), size, isActive);
    }

    public int Check()
    {
        return Data.Spells.消化.GetSpell().IsUnlock() && Data.Spells.消化.GetSpell().IsReadyWithCanCast()
            ? 0
            : -1;
    }

    public void Run()
    {
        //判断自身是否有盾的重叠
        if (Core.Me.HasAura(Data.Buffs.均衡预后))
        {
            var slot = new Slot();
            slot.Add(Data.Spells.消化.GetSpell());
            AI.Instance.BattleData.HighPrioritySlots_OffGCD.Enqueue(slot);
        }
        else
        {
            if (GCDHelper.GetGCDCooldown() <= 0)
            {
                var slot = new Slot();
                slot.Add(Data.Spells.均衡.GetSpell());
                slot.Add(Data.Spells.均衡预后adaptive.GetSpell());
                slot.Add(Data.Spells.消化.GetSpell());
                AI.Instance.BattleData.NextSlot = slot;
            }
            else
            {
                if (SgeHelper.均衡中)
                    群盾消化(GCDHelper.GetGCDCooldown() + 100);
                else
                    群盾消化();
            }
        }
    }

    private async Task 群盾消化(int delay = 0)
    {
        if (delay > 0) await Coroutine.Instance.WaitAsync(delay);
        AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.均衡.GetSpell());
        AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.均衡预后adaptive.GetSpell());
        AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.消化.GetSpell());
    }
}