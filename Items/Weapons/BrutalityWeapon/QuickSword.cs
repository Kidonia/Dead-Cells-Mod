using DeadCells.Projectiles.WeaponAnimationProj;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;

namespace DeadCells.Items.Weapons.BrutalityWeapon;

public class QuickSword : DeadCellsItem
{
    public override void SetDefaults()
    {
        SetWeaponDefaults(BrutalityDamage.Instance, 4, 2f, 2, 2, 1);
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (FirstAttack())
        {
            type = ModContent.ProjectileType<QuickSwordAtkA>();
            damage = DamageMul(playerComboAttack.QuickSwordAtkComboNum * 0.1f + 1.1f);
            InitialNextComboAttack(40, 12); 
        }
        if (CanNextAttack(2))
        {
            type = ModContent.ProjectileType<QuickSwordAtkB>();
            damage = DamageMul(playerComboAttack.QuickSwordAtkComboNum * 0.1f + 1.4f);
            InitialNextComboAttack(50, 15);
        }
        if (CanNextAttack(3))
        {
            type = ModContent.ProjectileType<QuickSwordAtkA>();
            damage = DamageMul(playerComboAttack.QuickSwordAtkComboNum * 0.1f + 1.2f);
            InitialNextComboAttack(50, 12);
        }
        if (CanNextAttack(4))
        {
            type = ModContent.ProjectileType<QuickSwordAtkB>();
            damage = DamageMul(playerComboAttack.QuickSwordAtkComboNum * 0.1f + 1.4f);
            InitialNextComboAttack(50, 17);
        }
        if (CanNextAttack(5))
        {
            type = ModContent.ProjectileType<QuickSwordAtkC>();
            damage = DamageMul(playerComboAttack.QuickSwordAtkComboNum * 0.1f + 1f);
            InitialNextComboAttack(50, 10);
        }
        if (CanNextAttack(6))
        {
            type = ModContent.ProjectileType<QuickSwordAtkD>();
            damage = DamageMul(playerComboAttack.QuickSwordAtkComboNum * 0.1f + 1.6f);
            FinalComboAttack(30);
        }
    }






}
