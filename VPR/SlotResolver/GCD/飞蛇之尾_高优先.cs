using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;

namespace yoyokity.VPR.SlotResolver.GCD;

public class 飞蛇之尾_高优先 : ISlotResolver
{
    public int Check()
    {
        //豆子小于2的时候不打
        if (VprHelper.蛇尾豆 < 2) return -1;

        //基础连击1的时候打,不然影响身位显示
        if (VprHelper.基础连步骤 != 1) return -2;

        //在蛇剑连中不打
        if (VprHelper.蛇剑连 != 0) return -3;

        //蛇剑有两个豆子先打蛇剑
        if (VprHelper.蛇尾豆 < 3 && Data.Spells.蛇剑1.GetSpell().Charges >= 2) return -4;

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Data.Spells.飞蛇之尾.GetSpell());
    }
}