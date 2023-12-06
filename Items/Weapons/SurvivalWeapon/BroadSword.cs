using DeadCells.Projectiles.WeaponAnimationProj;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;

namespace DeadCells.Items.Weapons.SurvivalWeapon;

public class BroadSword : DeadCellsItem
{
    public override void SetDefaults()
    {
        SetWeaponDefaults(SurvivalDamage.Instance, 16, 2.4f, 2, 6, 1750);
    }
    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (FirstAttack())
        {
            type = ModContent.ProjectileType<BroadSwordAtkA>();
            InitialNextComboAttack(90, 18); //可接上第二段攻击时间间隔， 第二段攻击的前摇
        }
        if (CanNextAttack(2))//能进行第二段攻击
        {
            type = ModContent.ProjectileType<BroadSwordAtkB>();
            InitialNextComboAttack(85, 26);
        }
        if (CanNextAttack(3))//能进行第二段攻击
        {
            type = ModContent.ProjectileType<BroadSwordAtkC>();
            FinalComboAttack(48); //后摇
        }
    }
}
