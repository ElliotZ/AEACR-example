using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Helper;
using AEAssist.JobApi;
using yoyokity.Common;
using yoyokity.DNC.SlotResolver.Data;

namespace yoyokity.DNC.QtUI;

public class 即兴表演hotkey : IHotkeyResolver
{
    private const uint SpellId = Spells.即兴表演;

    public void Draw(Vector2 size)
    {
        HotkeyHelper.DrawSpellImage(size, SpellId);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        SpellHelper.DrawSpellInfo(Helper.GetActionChange(SpellId).GetSpell(), size, isActive);
    }

    public int Check()
    {
        if (Spells.即兴表演.GetSpell().Cooldown.TotalMilliseconds > 0 ||
            Core.Resolve<JobApi_Dancer>().IsDancing)
            return -1;
        return 0;
    }

    public void Run()
    {
        var slot = new Slot();
        slot.Add(Spells.即兴表演.GetSpell());
        slot.Add(Spells.即兴表演结束.GetSpell());
        AI.Instance.BattleData.HighPrioritySlots_OffGCD.Enqueue(slot);
    }
}