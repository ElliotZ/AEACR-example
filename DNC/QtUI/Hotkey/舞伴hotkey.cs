using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using ImGuiNET;
using Data = yoyokity.DNC.SlotResolver.Data;

namespace yoyokity.DNC.QtUI;

public static class 舞伴hotkeyWindow
{
    public static HotkeyWindow DancePartnerPanel;
    private static JobViewSave myJobViewSave;

    public static void Build(JobViewWindow instance)
    {
        instance.SetUpdateAction(() =>
        {
            PartyHelper.UpdateAllies();
            if (PartyHelper.Party.Count < 1) return;

            myJobViewSave = new JobViewSave
            {
                QtHotkeySize = new Vector2(DncSettings.Instance.DancePartnerPanelIconSize),
                ShowHotkey = DncSettings.Instance.DancePartnerPanelShow
            };

            DancePartnerPanel = new HotkeyWindow(myJobViewSave, "DancePartnerPanel")
            {
                HotkeyLineCount = 1
            };

            for (var i = 1; i < PartyHelper.Party.Count; i++)
            {
                var index = i;
                DancePartnerPanel?.AddHotkey("切换舞伴: " + PartyHelper.Party[i].Name, new 舞伴hotkey(index));
            }

            DancePartnerPanel?.DrawHotkeyWindow(new QtStyle(DncSettings.Instance.JobViewSave));
        });
    }
}

public class 舞伴hotkey(int index) : IHotkeyResolver
{
    private const uint SpellId = Data.Spells.给舞伴;

    public void Draw(Vector2 size)
    {
        var id = SpellId;
        if (Core.Me.HasLocalPlayerAura(Data.Buffs.舞伴buff))
            id = Data.Spells.解除舞伴;

        var size1 = size * 0.8f;
        ImGui.SetCursorPos(size * 0.1f);
        if (!Core.Resolve<MemApiIcon>().GetActionTexture(id, out var textureWrap))
            return;
        ImGui.Image(textureWrap.ImGuiHandle, size1);

        if (SpellId.GetSpell().Cooldown.TotalMilliseconds > 0 || Core.Resolve<JobApi_Dancer>().IsDancing)
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
            ImGui.GetWindowDrawList()
                .AddText(textPos, ImGui.ColorConvertFloat4ToU32(new Vector4(1, 1, 1, 1)), cooldownText);
        }
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        /*SpellTargetType targetType = Index >= 1 && Index <= 8 ? (SpellTargetType)(Index + 3) : SpellTargetType.Target;
        SpellHelper.DrawSpellInfo(Core.Resolve<MemApiSpell>().CheckActionChange(this.SpellId).GetSpell(targetType), size, isActive);*/
    }

    public int Check()
    {
        if (SpellId.GetSpell().Cooldown.TotalMilliseconds != 0 ||
            Core.Resolve<JobApi_Dancer>().IsDancing)
            return -1;
        return 0;
    }

    public void Run()
    {
        ClosedPosition();
    }

    private void ClosedPosition()
    {
        var partyMembers = PartyHelper.Party;
        if (partyMembers.Count < index + 1)
            return;
        if (SpellId.GetSpell().Cooldown.TotalMilliseconds != 0 ||
            Core.Resolve<JobApi_Dancer>().IsDancing)
            return;

        if (GCDHelper.GetGCDCooldown() <= 0)
        {
            AI.Instance.BattleData.NextSlot ??= new Slot();
            if (Core.Me.HasAura(Data.Buffs.舞伴buff))
            {
                AI.Instance.BattleData.NextSlot.Add(Data.Spells.解除舞伴.GetSpell());
                AI.Instance.BattleData.NextSlot.Add(new Spell(SpellId, partyMembers[index]));
            }
            else
            {
                AI.Instance.BattleData.NextSlot.Add(new Spell(SpellId, partyMembers[index]));
            }
        }
        else
        {
            if (Core.Me.HasAura(Data.Buffs.舞伴buff))
            {
                AI.Instance.BattleData.HighPrioritySlots_OffGCD
                    .Enqueue(new Slot(Data.Spells.解除舞伴.GetSpell(), 2500));
                AI.Instance.BattleData.HighPrioritySlots_OffGCD
                    .Enqueue(new Slot(new Spell(SpellId, partyMembers[index]), 2500));
            }
            else
            {
                AI.Instance.BattleData.HighPrioritySlots_OffGCD
                    .Enqueue(new Slot(new Spell(SpellId, partyMembers[index]), 2500));
            }
        }
    }
}