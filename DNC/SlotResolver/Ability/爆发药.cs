using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using yoyokity.DNC.QtUI;

namespace yoyokity.DNC.SlotResolver.Ability;

public class 爆发药 : ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("爆发药")) return -1;
        if (!ItemHelper.CheckCurrJobPotion()) return -2;
        if (Qt.Instance.GetQt("爆发药2分") && AI.Instance.BattleData.CurrBattleTimeInMs <= 100 * 1000) return -3;

        //爆发药吃在跳大舞第一下后
        if (Data.Spells.大舞.GetSpell().Cooldown.TotalMilliseconds <= (120 - 2.5) * 1000) return -4;

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Spell.CreatePotion());
    }
}