
using DeadCells.Common.DamageClasses;
using DeadCells.Projectiles.WeaponAnimationProj;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace DeadCells.Items.Weapons.BrutalityWeapon;

public class LowHealth : DeadCellsItem
{
    public override void SetDefaults()
    {
        SetWeaponDefaults(BrutalityDamage.Instance, 14, 2f, 2, 6, 1500);
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        //大概就这么写，一段一段从下往上。
        if (FirstAttack())
        {
            type = ModContent.ProjectileType<LowHealthAtkA>();
            InitialNextComboAttack(80, 14); //可接上第二段攻击时间间隔， 第二段攻击的前摇
        }
        if (CanNextAttack(2))//能进行第二段攻击
        {
            type = ModContent.ProjectileType<LowHealthAtkB>();
            InitialNextComboAttack(80, 20);
        }
        if (CanNextAttack(3)) 
        {
            type = ModContent.ProjectileType<LowHealthAtkC>();
            damage = DamageMul(1.25f);
            InitialNextComboAttack(80, 17);
        }
        if (CanNextAttack(4))
        {
            type = ModContent.ProjectileType<LowHealthAtkD>();
            damage = DamageMul(0.875f);
            InitialNextComboAttack(80, 18);
        }
        if (CanNextAttack(5))
        {
            type = ModContent.ProjectileType<LowHealthAtkE>();
            damage = DamageMul(1.6f);
            FinalComboAttack(24); //后摇
        }
    }
}
