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

public class 群盾hotkey : IHotkeyResolver
{
    public void Draw(Vector2 size)
    {
        var id = Core.Resolve<MemApiSpell>().CheckActionChange(Data.Spells.均衡预后adaptive);
        var size1 = size * 0.8f;
        ImGui.SetCursorPos(size * 0.1f);
        if (!Core.Resolve<MemApiIcon>().GetActionTexture(id, out var textureWrap))
            return;
        ImGui.Image(textureWrap.ImGuiHandle, size1);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        var spell = Data.Spells.活化.GetSpell();
        var cd = spell.Cooldown.TotalSeconds;
        ImGui.SetCursorPos(new Vector2(0, 0));
        if (isActive)
        {
            //激活状态
            if (Core.Resolve<MemApiIcon>().TryGetTexture(@"Resources\Spells\Icon\activeaction.png",
                    out var textureWrapActive))
                ImGui.Image(textureWrapActive.ImGuiHandle, size);
        }
        else
        {
            //常规状态
            if (Core.Resolve<MemApiIcon>().TryGetTexture(@"Resources\Spells\Icon\iconframe.png",
                    out var textureWrapNormal))
                ImGui.Image(textureWrapNormal.ImGuiHandle, size);
        }

        //cd文字显示
        if (cd > 0 && (int)(cd * 1000) + 1 !=
            Core.Resolve<MemApiSpell>().GetGCDDuration() - Core.Resolve<MemApiSpell>().GetElapsedGCD())
        {
            //cd
            if (spell.Id != 0)
                cd %= spell.RecastTime.TotalSeconds / (float)spell.MaxCharges;
            ImGui.SetCursorPos(new Vector2(4, size.Y - 17));
            ImGui.Text($"{(int)cd + 1}");
        }
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

    private async Task 群盾活化(int delay = 0)
    {
        if (delay > 0) await Coroutine.Instance.WaitAsync(delay);
        AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.均衡.GetSpell());
        if (Data.Spells.活化.GetSpell().IsUnlock() && Data.Spells.活化.GetSpell().Cooldown.TotalMilliseconds <= 1000)
            AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.活化.GetSpell());
        AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.均衡预后adaptive.GetSpell());
    }
}