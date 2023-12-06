using DeadCells.Projectiles.WeaponAnimationProj;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;

namespace DeadCells.Items.Weapons.BrutalityWeapon;

public class StartSword : DeadCellsItem
{
    public override void SetDefaults()
    {
        SetWeaponDefaults(BrutalityDamage.Instance, 7, 2f, 2, 2, 1);
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (FirstAttack())
        {
            type = ModContent.ProjectileType<StartSwordAtkA>();
            InitialNextComboAttack(40, 1); //可接上第二段攻击时间间隔， 第二段攻击的前摇
        }
        if (CanNextAttack(2))//能进行第二段攻击
        {
            type = ModContent.ProjectileType<StartSwordAtkB>();
            damage = DamageMul(1.12f);
            InitialNextComboAttack(50, 8);
        }
        if (CanNextAttack(3))
        {
            type = ModContent.ProjectileType<StartSwordAtkC>();
            damage = DamageMul(1.3f);
            FinalComboAttack(26); //后摇
        }
    }
}