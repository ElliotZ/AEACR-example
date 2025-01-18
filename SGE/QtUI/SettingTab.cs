using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using ImGuiNET;
using yoyokity.Common;
using ItemHelper = yoyokity.Common.ItemHelper;

namespace yoyokity.SGE.QtUI;

public static class SettingTab
{
    private static int _8幻药;
    private static int 宝药;
    private static int _2宝药;

    private static void 技能设置Box()
    {
        ImGui.Checkbox("自动心关", ref SgeSettings.Instance.自动心关);
        ImGuiHelper.SetHoverTooltip("根据当前T自动切换心关，会随机延迟1-2秒模仿绿玩。");
        ImGui.Checkbox("贤炮爆发药", ref SgeSettings.Instance.贤炮爆发药);
        ImGuiHelper.SetHoverTooltip("开启后，如果没有红豆则会用贤炮来使收爆发药益最大化。");
        ImGui.Checkbox("T无敌给混合", ref SgeSettings.Instance.T无敌给混合);
        ImGuiHelper.SetHoverTooltip("开启后，将无视单奶QT，检测到队里死而不僵、死斗、超火流星立马给混合。");
        ImGui.Checkbox("摁下箭毒Hotkey后，没有红豆就打即刻", ref SgeSettings.Instance.没有箭毒打即刻);
        ImGui.Checkbox("即刻移动", ref SgeSettings.Instance.即刻移动);
        ImGuiHelper.SetHoverTooltip("不推荐开，让轴来固定打即刻更好。");
        ImGui.Dummy(new Vector2(0, 5));
        ImGuiHelper.LeftInputInt("醒梦阈值", ref SgeSettings.Instance.醒梦阈值, 1, 100, 5);
    }
    
    private static readonly InfoBox 自动单奶Box = new()
    {
        Label = "自动单奶设置",
        AutoResize = true,
        ContentsAction = () =>
        {
            ImGui.Dummy(new Vector2(0, 5));
            ImGuiHelper.TextColor(Color.DarkGreen, "*无损奶阈值");
            ImGuiHelper.LeftInputInt("灵橡", ref SgeSettings.Instance.灵橡阈值, 0, 100,5);
            ImGuiHelper.LeftInputInt("白牛", ref SgeSettings.Instance.白牛阈值, 0, 100,5);
            ImGuiHelper.LeftInputInt("混合", ref SgeSettings.Instance.混合阈值, 0, 100,5);
            ImGui.SameLine();
            ImGuiHelper.TextColor(Color.Gray, "只针对心关对象");
            ImGuiHelper.LeftInputInt("拯救", ref SgeSettings.Instance.拯救阈值, 0, 100,5);
            ImGui.SameLine();
            ImGuiHelper.TextColor(Color.Gray, "只针对心关对象");
            ImGuiHelper.LeftInputInt("输血", ref SgeSettings.Instance.输血阈值, 0, 100,5);
            ImGui.SameLine();
            ImGuiHelper.TextColor(Color.Gray, "只针对心关对象");
            ImGuiHelper.Separator();
            ImGuiHelper.TextColor(Color.DarkRed, "*有损奶阈值");
            ImGuiHelper.LeftInputInt("诊断", ref SgeSettings.Instance.诊断阈值, 0, 100,5);
        }
    };
    
    private static readonly InfoBox 自动群奶Box = new()
    {
        Label = "自动群奶设置",
        AutoResize = true,
        ContentsAction = () =>
        {
            ImGui.Dummy(new Vector2(0, 5));
            ImGuiHelper.TextColor(Color.Gray, "*满足同时有x人的血量低于阈值时自动奶");
            ImGui.Dummy(new Vector2(0, 5));
            ImGuiHelper.LeftInputInt("团血检测人数", ref SgeSettings.Instance.团血检测人数, 1, 8);
            ImGuiHelper.Separator();
            ImGuiHelper.TextColor(Color.DarkGreen, "*无损奶阈值");
            ImGuiHelper.LeftInputInt("自生", ref SgeSettings.Instance.自生阈值, 0, 100,5);
            ImGuiHelper.LeftInputInt("寄生", ref SgeSettings.Instance.寄生青汁阈值, 0, 100,5);
            ImGuiHelper.LeftInputInt("贤炮", ref SgeSettings.Instance.魂灵风息阈值, 0, 100,5);
            ImGuiHelper.LeftInputInt("活化贤炮", ref SgeSettings.Instance.活化魂灵风息阈值, 0, 100,5);
            ImGuiHelper.LeftInputInt("智慧之爱", ref SgeSettings.Instance.智慧之爱阈值, 0, 100,5);
            ImGuiHelper.Separator();
            ImGuiHelper.TextColor(Color.DarkRed, "*有损奶阈值");
            ImGuiHelper.LeftInputInt("群盾消化", ref SgeSettings.Instance.群盾消化阈值, 0, 100,5);
            ImGuiHelper.LeftInputInt("预后", ref SgeSettings.Instance.预后阈值, 0, 100,5);
            ImGuiHelper.LeftInputInt("群盾（盾）", ref SgeSettings.Instance.群盾阈值, 0, 100,5);
        }
    };

    public static void Build(JobViewWindow instance)
    {
        instance.AddTab("设置", window =>
        {
            if (ImGui.CollapsingHeader("技能设置", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.Dummy(new Vector2(5, 0));
                ImGui.SameLine();
                ImGui.BeginGroup();

                技能设置Box();
                ImGui.Dummy(new Vector2(0, 10));
                自动单奶Box.DrawStretched();
                ImGui.Dummy(new Vector2(0, 10));
                自动群奶Box.DrawStretched();
                
                ImGui.EndGroup();
            }

            if (ImGui.CollapsingHeader("时间轴", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.Dummy(new Vector2(5, 0));
                ImGui.SameLine();
                ImGui.BeginGroup();
                ImGui.Checkbox("启用时间轴debug", ref SgeSettings.Instance.TimeLinesDebug);
                ImGui.Checkbox("启用自动更新", ref SgeSettings.Instance.AutoUpdataTimeLines);
                if (ImGui.Button("手动更新")) TimeLineUpdater.UpdateFiles(Helper.SgeTimeLineUrl);
                ImGui.SameLine();
                if (ImGui.Button("源码"))
                    Process.Start(new ProcessStartInfo(Helper.TimeLineLibraryUrl) { UseShellExecute = true });
                ImGui.EndGroup();
                ImGui.Dummy(new Vector2(0, 10));
            }

            if (ImGui.CollapsingHeader("复活窗口", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.Dummy(new Vector2(5, 0));
                ImGui.SameLine();
                ImGui.BeginGroup();
                ImGui.Checkbox("显示复活窗口", ref SgeSettings.Instance.SgePartnerPanelShow);
                if (SgeSettings.Instance.SgePartnerPanelShow)
                {
                    ImGui.SameLine();
                    ImGui.Checkbox("显示营救按钮", ref SgeSettings.Instance.SgePartner营救);
                }
                ImGui.DragInt("复活窗口大小", ref SgeSettings.Instance.SgePartnerPanelIconSize, 1, 20, 100);
                ImGui.EndGroup();
                ImGui.Dummy(new Vector2(0, 10));
            }

            ImGuiHelper.Separator();

            if (ImGui.Button("获取爆发药情况"))
            {
                _8幻药 = ItemHelper.FindItem((uint)Potion._8级意力之幻药);
                宝药 = ItemHelper.FindItem((uint)Potion.意力之宝药);
                _2宝药 = ItemHelper.FindItem((uint)Potion._2级意力之宝药);
            }

            if (_8幻药 > 0)
            {
                ImGui.Text($"8级意力之幻药：{_8幻药} 瓶");
                DrawPotion(Potion._8级意力之幻药);
            }

            if (宝药 > 0)
            {
                ImGui.Text($"意力之宝药：{宝药} 瓶");
                DrawPotion(Potion.意力之宝药);
            }

            if (_2宝药 > 0)
            {
                ImGui.Text($"2级意力之宝药：{_2宝药} 瓶");
                DrawPotion(Potion._2级意力之宝药);
            }
        });
    }

    private static void DrawPotion(Potion potion)
    {
        var id = (int)potion;
        ImGui.SameLine();
        if (ImGui.Button("复制id###" + id))
        {
            ImGui.SetClipboardText(id.ToString());
        }
    }
}