using DeadCells.Projectiles.WeaponAnimationProj;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;

namespace DeadCells.Items.Weapons.BrutalityWeapon;

public class DualDaggers : DeadCellsItem
{
    public override void SetDefaults()
    {
        SetWeaponDefaults(BrutalityDamage.Instance, 15, 2.4f, 2, 6, 1800);
    }
    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (FirstAttack())
        {
            type = ModContent.ProjectileType<DualDaggersAtkA>();
            InitialNextComboAttack(30, 1); //可接上第二段攻击时间间隔， 第二段攻击的前摇
        }
        if (CanNextAttack(2))//能进行第二段攻击
        {
            type = ModContent.ProjectileType<DualDaggersAtkB>();
            InitialNextComboAttack(30, 1);
        }
        if (CanNextAttack(3))//能进行第二段攻击
        {
            type = ModContent.ProjectileType<DualDaggersAtkC>();
            damage = DamageMul(1.2f);
            FinalComboAttack(34); //后摇
        }
    }
}
