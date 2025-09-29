using System.Numerics;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Colors;
using Dalamud.Utility;

namespace yoyokity.DNC.QtUI;

public static class ReadmeTab
{
    private static readonly InfoBox Box = new()
    {
        AutoResize = true,
        BorderColor = ImGuiColors.ParsedGold,
        ContentsAction = () =>
        {
            if (ImGui.Button("查看更新日志"))
            {
                Util.OpenLink("https://github.com/yoyokity/AEACR/releases"); 
            }
            
            ImGuiHelper.Separator();
            ImGui.BulletText("推荐复唱：2.5");
            ImGui.BulletText("推荐舞伴：画 > team盘 > 忍 > 龙 > 盘 > 僧 > 镰 > 召 > DK = 舞 > 赤 > 蛇 > 黑");
        }
    };
    
    public static void Build(JobViewWindow instance)
    {
        instance.AddTab("说明", window =>
        {
            ImGui.Dummy(new Vector2(0, 1));
            ImGui.Dummy(new Vector2(5, 0));
            ImGui.SameLine();
            Box.DrawStretched();
        });
    }
}