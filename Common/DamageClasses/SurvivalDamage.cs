using Terraria.ModLoader;
using Terraria;

namespace DeadCells.Common.DamageClasses;

public class SurvivalDamage : DamageClass
{
    internal static SurvivalDamage Instance;

    public override void Load()
    {
        Instance = this;
    }

    public override void Unload()
    {
        Instance = null;
    }
    public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
    {
        if (damageClass == Generic)
            return StatInheritanceData.Full;

        if (damageClass == Melee)
        {
            return new StatInheritanceData(
                damageInheritance: 0.2f,
                critChanceInheritance: 0f,
                attackSpeedInheritance: 0f,
                armorPenInheritance: 0.6f,
                knockbackInheritance: 0f
            );
        };

        if (damageClass == DamageClass.Summon)
        {
            return new StatInheritanceData(
                damageInheritance: 0.8f,
                critChanceInheritance: 0f,
                attackSpeedInheritance: 0f,
                armorPenInheritance: 0.4f,
                knockbackInheritance: 0f
            );
        };
        return StatInheritanceData.None;
    }

    public override bool GetEffectInheritance(DamageClass damageClass)
    {
        return false;
    }

    public override void SetDefaultStats(Player player)
    {
        player.GetCritChance<SurvivalDamage>() = 0;
    }

    public override bool UseStandardCritCalcs => false;


}
