using DeadCells.Projectiles.WeaponAnimationProj;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;

namespace DeadCells.Items.Weapons.SurvivalWeapon;

public class KingsSpear : DeadCellsItem
{
    public override void SetDefaults()
    {
        SetWeaponDefaults(SurvivalDamage.Instance, 18, 2f, 2, 6, 2000);
    }
    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (FirstAttack())
        {
            type = ModContent.ProjectileType<KingsSpearAtkA>();
            InitialNextComboAttack(80, 12); //可接上第二段攻击时间间隔， 第二段攻击的前摇
        }
        if (CanNextAttack(2))//能进行第二段攻击
        {
            type = ModContent.ProjectileType<KingsSpearAtkB>();
            damage = DamageMul(1.3f);
            InitialNextComboAttack(80, 14);
        }
        if (CanNextAttack(3))
        {
            type = ModContent.ProjectileType<KingsSpearAtkC>();
            damage = DamageMul(1.95f);
            FinalComboAttack(48); //后摇
        }
    }
}
