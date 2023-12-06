using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using DeadCells.Projectiles.WeaponAnimationProj;
using DeadCells.Common.DamageClasses;

namespace DeadCells.Items.Weapons.BrutalityWeapon;

public class AdminWeapon : DeadCellsItem

{
    public override void SetDefaults()
    {
        SetWeaponDefaults(BrutalityDamage.Instance, 114514, 0f, 2, 6, 1500);
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (FirstAttack())
        {
            type = ModContent.ProjectileType<AdminAttack>();
            FinalComboAttack(0); //后摇
        }
    }
}

