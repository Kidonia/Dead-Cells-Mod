using Terraria;
using Terraria.ModLoader;

namespace DeadCells.Common.DamageClasses;

public class BrutalityDamage : DamageClass
{
    internal static BrutalityDamage Instance;

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
                damageInheritance: 0.4f,
                critChanceInheritance: 0f,
                attackSpeedInheritance: 0f,
                armorPenInheritance: 0.8f,
                knockbackInheritance: 0.1f
            );
        };

        if (damageClass == MeleeNoSpeed)
        {
            return new StatInheritanceData(
                damageInheritance: 0.4f,
                critChanceInheritance: 0f,
                attackSpeedInheritance: 0f,
                armorPenInheritance: 0.8f,
                knockbackInheritance: 0.1f
            );
        };
        return StatInheritanceData.None;
    }

    public override bool GetEffectInheritance(DamageClass damageClass)
    {
        if (damageClass == Melee)
            return true;
        if (damageClass == MeleeNoSpeed)
            return true;

        return false;
    }

    public override void SetDefaultStats(Player player)
    {
        player.GetCritChance<BrutalityDamage>() = 0;
    }

    public override bool UseStandardCritCalcs => false;
}
