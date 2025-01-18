using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using yoyokity.SGE.QtUI;

namespace yoyokity.SGE.SlotResolver.奶;

public class 输血: ISlotResolver
{
    public int Check()
    {
        if (!Qt.Instance.GetQt("单奶")) return -1;
        if (!Data.Spells.输血.GetSpell().IsReadyWithCanCast()) return -2;

        //检测心关对象
        if (SgeHelper.心关目标 == null) return -3;
        if (SgeHelper.心关目标.CurrentHpPercent() > SgeSettings.Instance.输血阈值 / 100f) return -5;
        
        //活死人不给
        if (SgeHelper.心关目标.HasAura(Data.Buffs.活死人)) return -6;
        
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(new Spell(Data.Spells.输血, SgeHelper.心关目标!));
    }
}