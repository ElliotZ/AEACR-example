using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using ImGuiNET;
using Data = yoyokity.SGE.SlotResolver.Data;

namespace yoyokity.SGE.QtUI;

public class 突进hotkey : IHotkeyResolver
{
    public void Draw(Vector2 size)
    {
        var id = Core.Resolve<MemApiSpell>().CheckActionChange(Data.Spells.神翼);
        var size1 = size * 0.8f;
        ImGui.SetCursorPos(size * 0.1f);
        if (!Core.Resolve<MemApiIcon>().GetActionTexture(id, out var textureWrap))
            return;
        ImGui.Image(textureWrap.ImGuiHandle, size1);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        SpellHelper.DrawSpellInfo(Data.Spells.神翼.GetSpell(), size, isActive);
    }

    public int Check()
    {
        return Core.Me.GetCurrTarget() != null &&
               Data.Spells.神翼.GetSpell().Cooldown.TotalMilliseconds <= 0
            ? 0
            : -1;
    }

    public void Run()
    {
        var slot = new Slot();
        if (GCDHelper.GetGCDCooldown() <= 0)
        {
            slot.Add(Data.Spells.神翼.GetSpell(SpellTargetType.Target));
            AI.Instance.BattleData.NextSlot = slot;
        }
        else
        {
            slot.Add(Data.Spells.神翼.GetSpell(SpellTargetType.Target));
            AI.Instance.BattleData.HighPrioritySlots_OffGCD.Enqueue(slot);
        }
    }
}