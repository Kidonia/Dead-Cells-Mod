
using Terraria;
using Terraria.ModLoader;

namespace DeadCells.Common.DamageClasses;

public class TacticsDamage : DamageClass
{
    internal static TacticsDamage Instance;

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

        if (damageClass == Ranged)
        {
            return new StatInheritanceData(
                damageInheritance: 0.4f,
                critChanceInheritance: 0f,
                attackSpeedInheritance: 0f,
                armorPenInheritance: 0.6f,
                knockbackInheritance: 0.8f
            );
        };

        if (damageClass == DamageClass.Magic)
        {
            return new StatInheritanceData(
                damageInheritance: 0.2f,
                critChanceInheritance: 0f,
                attackSpeedInheritance: 0f,
                armorPenInheritance: 0.4f,
                knockbackInheritance: 0.1f
            );
        };
        return StatInheritanceData.None;
    }

    public override bool GetEffectInheritance(DamageClass damageClass)
    {
        if (damageClass == Ranged)
            return true;

        return false;
    }

    public override void SetDefaultStats(Player player)
    {
        player.GetCritChance<TacticsDamage>() = 0;
    }

    public override bool UseStandardCritCalcs => false;



}
