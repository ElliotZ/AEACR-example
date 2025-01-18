using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.Ability;

public class 心神风息: ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("心神")) return -1;
        if (!Data.Spells.心神风息.GetSpell().IsReadyWithCanCast()) return -2;
        
        //开局3个gcd内不打
        if (AI.Instance.BattleData.CurrBattleTimeInMs <= 2500 * 3) return -3;
        
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.心神风息.GetSpell());
    }
}