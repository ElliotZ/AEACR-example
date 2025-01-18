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

public class 发炎hotkey : IHotkeyResolver
{
    public void Draw(Vector2 size)
    {
        var id = Core.Resolve<MemApiSpell>().CheckActionChange(Data.Spells.发炎adaptive);
        var size1 = size * 0.8f;
        ImGui.SetCursorPos(size * 0.1f);
        if (!Core.Resolve<MemApiIcon>().GetActionTexture(id, out var textureWrap))
            return;
        ImGui.Image(textureWrap.ImGuiHandle, size1);
    }

    private static int _check()
    {
        if (!Data.Spells.发炎adaptive.GetSpell().IsReadyWithCanCast()) return -1;
        if (Core.Me.GetCurrTarget() == null) return -2;
        if (Core.Me.Distance(Core.Me.GetCurrTarget()!) >
            SettingMgr.GetSetting<GeneralSettings>().AttackRange + 3) return -3;
        return 0;
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        var spell = Data.Spells.发炎adaptive.GetSpell();
        var cd = spell.Cooldown.TotalSeconds;
        ImGui.SetCursorPos(new Vector2(0, 0));
        if (_check() >= 0)
        {
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
        }
        else
        {
            //变黑
            ImGui.SetCursorPos(new Vector2(0, 0));
            if (Core.Resolve<MemApiIcon>().TryGetTexture(@"Resources\Spells\Icon\icona_frame_disabled.png",
                    out var textureWrapBlack))
                ImGui.Image(textureWrapBlack.ImGuiHandle, size);
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
        
        //Charge
        if (spell.MaxCharges > 1)
        {
            var charge = (int)spell.Charges;
            var chargeSize = size * 0.3f;
            ImGui.SetCursorPos(new Vector2(size.X - chargeSize.X - 3, size.Y - chargeSize.Y - 5));
            if (Core.Resolve<MemApiIcon>().TryGetTexture($@"Resources\Spells\Icon\Charge{charge}.png",
                    out var textureWrap_normal))
                ImGui.Image(textureWrap_normal.ImGuiHandle, chargeSize);
        }
    }

    public int Check()
    {
        return _check();
    }

    public void Run()
    {
        AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.发炎adaptive.GetSpell());
    }
}