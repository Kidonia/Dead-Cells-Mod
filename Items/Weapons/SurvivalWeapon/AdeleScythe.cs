using DeadCells.Projectiles.WeaponAnimationProj;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;

namespace DeadCells.Items.Weapons.SurvivalWeapon;

public class AdeleScythe : DeadCellsItem
{
    public override void SetDefaults()
    {
        SetWeaponDefaults(SurvivalDamage.Instance, 14, 1.6f, 2, 6, 2000);
    }
    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (FirstAttack())
        {
            type = ModContent.ProjectileType<AdeleScytheAtkA>();
            InitialNextComboAttack(90, 8); //可接上第二段攻击时间间隔， 第二段攻击的前摇
        }
        if (CanNextAttack(2))//能进行第二段攻击
        {
            type = ModContent.ProjectileType<AdeleScytheAtkB>();
            damage = DamageMul(1.05f);
            InitialNextComboAttack(90, 10);
        }
        if (CanNextAttack(3))
        {
            type = ModContent.ProjectileType<AdeleScytheAtkC>();
            damage = DamageMul(1.2f);
            InitialNextComboAttack(90, 12);
        }
        if (CanNextAttack(4))
        {
            type = ModContent.ProjectileType<AdeleScytheAtkD>();
            damage = DamageMul(2f);
            FinalComboAttack(36); //后摇
        }
    }
}

