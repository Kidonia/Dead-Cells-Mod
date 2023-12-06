using Microsoft.Xna.Framework;
using DeadCells.Projectiles.WeaponAnimationProj;
using Terraria;
using Terraria.ModLoader;
using DeadCells.Common.DamageClasses;

namespace DeadCells.Items.Weapons.BrutalityWeapon;

public class Bleeder : DeadCellsItem
{
    public override void SetDefaults()
    {
        SetWeaponDefaults(BrutalityDamage.Instance, 12, 2f, 2, 6, 1500);
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        //大概就这么写。
        if (FirstAttack())
        {
            type = ModContent.ProjectileType<BleederAtkA>();
            InitialNextComboAttack(80, 4); //可接上第二段攻击时间间隔， 第二段攻击的前摇
        }
        if (CanNextAttack(2))//能进行第二段攻击
        {
            type = ModContent.ProjectileType<BleederAtkB>();
            damage = DamageMul(1.05f);
            FinalComboAttack(6); //后摇
        }
    }

}
