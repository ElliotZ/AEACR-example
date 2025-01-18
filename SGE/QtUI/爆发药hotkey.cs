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
using ItemHelper = AEAssist.Helper.ItemHelper;

namespace yoyokity.SGE.QtUI;

public class 爆发药hotkey : IHotkeyResolver
{
    private static string ImgPath => @"Resources\Spells\Potion.png";

    public void Draw(Vector2 size)
    {
        var iconSize = size * 0.8f;
        //技能图标
        ImGui.SetCursorPos(size * 0.1f);

        if (Core.Resolve<MemApiIcon>().TryGetTexture(ImgPath, out var textureWrap))
        {
            ImGui.Image(textureWrap.ImGuiHandle, iconSize);
        }
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        SpellHelper.DrawSpellInfo(Spell.CreatePotion(), size, isActive);
    }

    public int Check()
    {
        var potion = SettingMgr.GetSetting<PotionSetting>().GetPotionId(Core.Me.CurrentJob());
        if (!ItemHelper.CheckPotion(potion, true))
        {
            return -1;
        }

        return 0;
    }

    public void Run()
    {
        if (GCDHelper.GetGCDCooldown() <= 0)
        {
            var slot = new Slot();
            slot.Add(Spell.CreatePotion());
            AI.Instance.BattleData.NextSlot = slot;
        }
        else
        {
            //计算爆发药可插入的时间
            var startTime1 = Data.Spells.发炎adaptive.GetSpell().Cooldown.TotalMilliseconds - 30 * 1000;
            var startTime2 = Data.Spells.心神风息.GetSpell().Cooldown.TotalMilliseconds - 30 * 1000;
            var endTime1 = Data.Spells.发炎adaptive.GetSpell().Cooldown.TotalMilliseconds + 3000;
            var endTime2 = Data.Spells.心神风息.GetSpell().Cooldown.TotalMilliseconds + 3000;

            var startTime = startTime1 > startTime2 ? startTime1 : startTime2;
            var endTime = endTime1 < endTime2 ? endTime1 : endTime2;

            var buffTime = Core.Resolve<MemApiBuff>()
                .GetAuraTimeleft(Core.Me.GetCurrTarget(), Data.Buffs.均衡注药adaptive, true);

            if (buffTime > startTime && buffTime < endTime)
            {
                if (SgeHelper.红豆 > 0)
                {
                    if (SgeSettings.Instance.TimeLinesDebug)
                        LogHelper.Print("yoyo贤者", $"[最优] 将在{(buffTime - 5.5 * 1000) / 1000.0:F2}秒后使用爆发药");
                    红豆爆发药((int)(buffTime - 5.5 * 1000));
                }
                else if (SgeSettings.Instance.贤炮爆发药 &&
                         Data.Spells.贤炮.GetSpell().IsReadyWithCanCast())
                {
                    if (SgeSettings.Instance.TimeLinesDebug)
                        LogHelper.Print("yoyo贤者", $"[最优] 将在{(buffTime - 5.5 * 1000) / 1000.0:F2}秒后使用爆发药");
                    贤炮爆发药((int)(buffTime - 5.5 * 1000));
                }
                else
                {
                    if (SgeSettings.Instance.TimeLinesDebug)
                        LogHelper.Print("yoyo贤者", $"[最优] 将在{(buffTime - 3.5 * 1000) / 1000.0:F2}秒后使用爆发药");
                    爆发药((int)(buffTime - 3.5 * 1000));
                }
            }
            else
            {
                var 心神cd = Data.Spells.心神风息.GetSpell().Cooldown.TotalMilliseconds;
                if (心神cd > 0)
                {
                    if (SgeSettings.Instance.TimeLinesDebug)
                        LogHelper.Print("yoyo贤者", $"将在{(心神cd - 2.5 * 1000) / 1000.0:F2}秒后使用爆发药");
                    爆发药强插((int)(心神cd - 2.5 * 1000));
                }
                else
                {
                    AI.Instance.BattleData.NextSlot = new Slot();
                    AI.Instance.BattleData.NextSlot.Add2NdWindowAbility(Spell.CreatePotion());
                }
            }
        }
    }

    private async Task 红豆爆发药(int delay = 0)
    {
        BattleData.Instance.Lock发炎 = true;
        if (delay > 0) await Coroutine.Instance.WaitAsync(delay);
        AI.Instance.BattleData.NextSlot = new Slot();
        AI.Instance.BattleData.NextSlot.Add(Data.Spells.均衡.GetSpell());
        AI.Instance.BattleData.NextSlot.Add(Data.Spells.箭毒adaptive.GetSpell());
        AI.Instance.BattleData.NextSlot.Add2NdWindowAbility(Spell.CreatePotion());
        AI.Instance.BattleData.NextSlot.Add(Data.Spells.均衡注药adaptive.GetSpell());
        BattleData.Instance.Lock发炎 = false;
    }

    private async Task 贤炮爆发药(int delay = 0)
    {
        BattleData.Instance.Lock发炎 = true;
        if (delay > 0) await Coroutine.Instance.WaitAsync(delay);
        AI.Instance.BattleData.NextSlot = new Slot();
        AI.Instance.BattleData.NextSlot.Add(Data.Spells.均衡.GetSpell());
        AI.Instance.BattleData.NextSlot.Add(Data.Spells.贤炮.GetSpell());
        AI.Instance.BattleData.NextSlot.Add2NdWindowAbility(Spell.CreatePotion());
        AI.Instance.BattleData.NextSlot.Add(Data.Spells.均衡注药adaptive.GetSpell());
        BattleData.Instance.Lock发炎 = false;
    }

    private async Task 爆发药(int delay = 0)
    {
        BattleData.Instance.Lock发炎 = true;
        if (delay > 0) await Coroutine.Instance.WaitAsync(delay);
        AI.Instance.BattleData.NextSlot = new Slot();
        AI.Instance.BattleData.NextSlot.Add(Data.Spells.均衡.GetSpell());
        AI.Instance.BattleData.NextSlot.Add(Spell.CreatePotion());
        AI.Instance.BattleData.NextSlot.AddDelaySpell(300, Data.Spells.均衡注药adaptive.GetSpell());
        BattleData.Instance.Lock发炎 = false;
    }

    private async Task 爆发药强插(int delay = 0)
    {
        BattleData.Instance.Lock发炎 = true;
        if (delay > 0) await Coroutine.Instance.WaitAsync(delay);
        AI.Instance.BattleData.NextSlot = new Slot();
        AI.Instance.BattleData.NextSlot.Add(Spell.CreatePotion());
        BattleData.Instance.Lock发炎 = false;
    }
}