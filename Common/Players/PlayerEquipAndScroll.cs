using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DeadCells.Common.Players;

public class PlayerEquipAndScroll : ModPlayer
{
    //处理玩家卷轴数量，卷轴加伤
    public bool Brutality;
    public bool Tactics;
    public bool Survival;

    public int BrutalityNum = 1;
    public int TacticsNum = 1;
    public int SurvivalNum = 1;

    public double BrutalityMul = 1;
    public double TacticsMul = 1;
    public double SurvivalMul = 1;

    private readonly double DamageMul = 1.15;

    public int CurrentShieldID = 0;
    public bool ShieldCoolDownShow = false;
    public override void PreUpdate()
    {
        BrutalityMul = Math.Round(Math.Pow(DamageMul, BrutalityNum) - 1, 2);
        TacticsMul = Math.Round(Math.Pow(DamageMul, TacticsNum) - 1, 2);
        SurvivalMul = Math.Round(Math.Pow(DamageMul, SurvivalNum) - 1, 2);
        ShieldCoolDownShow = Player.shield_parry_cooldown > 0;

    }

    //三者选最大，屎来自CSDN
    public int GetMaxNumScroll()
    {
        return BrutalityNum > TacticsNum ? BrutalityNum > SurvivalNum ? BrutalityNum : SurvivalNum : TacticsNum > SurvivalNum ? TacticsNum : SurvivalNum;
    }
    public float GetMaxScrollMul()
    {
        return (float)(BrutalityMul > TacticsMul ? BrutalityMul > SurvivalMul ? BrutalityMul : SurvivalMul : TacticsMul > SurvivalMul ? TacticsMul : SurvivalMul);
    }

    public float GetMaxScrollMulPlusOne()
    {
        return (float)(BrutalityMul > TacticsMul ? BrutalityMul > SurvivalMul ? BrutalityMul : SurvivalMul : TacticsMul > SurvivalMul ? TacticsMul : SurvivalMul) + 1;
    }

    public override void SaveData(TagCompound tag)
    {
        tag["ShieldEquip"] = CurrentShieldID;
    }
    public override void LoadData(TagCompound tag)
    {
        CurrentShieldID = tag.GetInt("ShieldEquip");
    }


}
//3 scroll