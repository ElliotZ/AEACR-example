using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using Data = yoyokity.SGE.SlotResolver.Data;

namespace yoyokity.SGE.QtUI;

public class 单盾hotkey : IHotkeyResolver
{
    private static IBattleChara? Target { get; set; }
    
    public void Draw(Vector2 size)
    {
        HotkeyHelper.DrawSpellImage(size, Data.Spells.均衡诊断);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        if (isActive)
        {
            HotkeyHelper.DrawActiveState(size);
        }
        else
        {
            HotkeyHelper.DrawGeneralState(size);
        }
    }

    public int Check()
    {
        Target= PartyHelper.CastableAlliesWithin30
            .Concat([Core.Me])
            .OrderBy(x=>x.CurrentHpPercent())
            .FirstOrDefault();

        return Target == null ? -1 : 0;
    }

    public void Run()
    {
        if (GCDHelper.GetGCDCooldown() <= 0)
        {
            var slot = new Slot();
            slot.Add(Data.Spells.均衡.GetSpell());
            slot.Add(Data.Spells.均衡诊断.GetSpell(Target!));
            AI.Instance.BattleData.NextSlot = slot;
        }
        else
        {
            if (SgeHelper.均衡中)
                均衡诊断(GCDHelper.GetGCDCooldown() + 100);
            else
                均衡诊断();
        }
    }
    
    private static async Task 均衡诊断(int delay = 0)
    {
        if (delay > 0) await Coroutine.Instance.WaitAsync(delay);
        AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.均衡.GetSpell());
        AI.Instance.BattleData.AddSpell2NextSlot(Data.Spells.均衡诊断.GetSpell(Target!));
    }
}