using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using ImGuiNET;
using yoyokity.DNC.SlotResolver.Data;

namespace yoyokity.DNC.QtUI;

public class 即兴表演hotkey : IHotkeyResolver
{
    private const uint SpellId = Spells.即兴表演;

    public void Draw(Vector2 size)
    {
        var id = Core.Resolve<MemApiSpell>().CheckActionChange(SpellId);
        var size1 = size * 0.8f;
        ImGui.SetCursorPos(size * 0.1f);
        if (!Core.Resolve<MemApiIcon>().GetActionTexture(id, out var textureWrap))
            return;
        ImGui.Image(textureWrap.ImGuiHandle, size1);
        // Check if skill is on cooldown and apply grey overlay if true

        if (!Core.Resolve<MemApiSpell>().CheckActionChange(SpellId).GetSpell().IsReadyWithCanCast())
        {
            // Use ImGui.GetItemRectMin() and ImGui.GetItemRectMax() for exact icon bounds
            Vector2 overlayMin = ImGui.GetItemRectMin();
            Vector2 overlayMax = ImGui.GetItemRectMax();

            // Draw a grey overlay over the icon
            ImGui.GetWindowDrawList().AddRectFilled(
                overlayMin,
                overlayMax,
                ImGui.ColorConvertFloat4ToU32(new Vector4(0, 0, 0, 0.5f))); // 50% transparent grey
        }

        var cooldownRemaining = SpellId.GetSpell().Cooldown.TotalMilliseconds / 1000;
        if (cooldownRemaining > 0)
        {
            // Convert cooldown to seconds and format as string
            string cooldownText = Math.Ceiling(cooldownRemaining).ToString();

            // 计算文本位置，向左下角偏移
            Vector2 textPos = ImGui.GetItemRectMin();
            textPos.X -= 1; // 向左移动一点
            textPos.Y += size1.Y - ImGui.CalcTextSize(cooldownText).Y + 5; // 向下移动一点

            // 绘制冷却时间文本
            //ImGui.GetWindowDrawList().AddText(textPos, ImGui.ColorConvertFloat4ToU32(new Vector4(1, 1, 1, 1)), cooldownText);
        }
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        SpellHelper.DrawSpellInfo(Core.Resolve<MemApiSpell>().CheckActionChange(SpellId).GetSpell(), size,
            isActive);
    }

    public int Check()
    {
        if (Spells.即兴表演.GetSpell().Cooldown.TotalMilliseconds != 0 ||
            Core.Resolve<JobApi_Dancer>().IsDancing)
            return -1;
        return 0;
    }

    public void Run()
    {
        if (Spells.即兴表演.GetSpell().Cooldown.TotalMilliseconds != 0 ||
            Core.Resolve<JobApi_Dancer>().IsDancing)
            return;
        var slot = new Slot();
        slot.Add(Spells.即兴表演.GetSpell());
        slot.Add(Spells.即兴表演结束.GetSpell());
        AI.Instance.BattleData.HighPrioritySlots_OffGCD.Enqueue(slot);
    }
}