using System.Numerics;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using Dalamud.Interface.Colors;
using Dalamud.Utility;
using ImGuiNET;

namespace yoyokity.SGE.QtUI;

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
            ImGui.BulletText("推荐复唱：2.5 (网络延迟较高的情况下可以使用2.49)");
            ImGui.BulletText("搭配滑步插件【Orbwalker】使用更爽，并将【提前使用下个GCD时间】设置为 70 ms");
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