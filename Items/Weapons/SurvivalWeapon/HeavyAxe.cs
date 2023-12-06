using DeadCells.Projectiles.WeaponAnimationProj;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;

namespace DeadCells.Items.Weapons.SurvivalWeapon;

public class HeavyAxe : DeadCellsItem
{
    public override void SetDefaults()
    {
        SetWeaponDefaults(SurvivalDamage.Instance, 24, 2f, 2, 6, 1750);
    }
    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (FirstAttack())
        {
            type = ModContent.ProjectileType<HeavyAxeAtkA>();
            InitialNextComboAttack(108, 4); //可接上第二段攻击时间间隔， 第二段攻击的前摇
        }
        if (CanNextAttack(2))//能进行第二段攻击
        {
            type = ModContent.ProjectileType<HeavyAxeAtkB>();
            damage = DamageMul(1.05f);
            InitialNextComboAttack(120, 16);
        }
        if (CanNextAttack(3))
        {
            type = ModContent.ProjectileType<HeavyAxeAtkC>();
            damage = DamageMul(1.1f);
            InitialNextComboAttack(124, 12);
        }
        if (CanNextAttack(4))
        {
            type = ModContent.ProjectileType<HeavyAxeAtkD>();
            damage = DamageMul(1.2f);
            playerComboAttack.TimeCanConsistentAttack = 124;
            playerComboAttack.ConsistentLockCtrlAfter = 2;
            
        }
    }
}