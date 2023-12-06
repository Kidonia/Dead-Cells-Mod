using DeadCells.Projectiles.WeaponAnimationProj;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;

namespace DeadCells.Items.Weapons.SurvivalWeapon;

public class TickScythe : DeadCellsItem
{
    public override void SetDefaults()
    {
        SetWeaponDefaults(SurvivalDamage.Instance, 24, 2f, 2, 6, 1500);
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        //大概就这么写。
        if (FirstAttack())
        {
            type = ModContent.ProjectileType<TickScytheAtkB1>();//暴击原理写在B1里了
            damage = DamageMul(0.75f);
            InitialNextComboAttack(120, 8); //可接上第二段攻击时间间隔， 第二段攻击的前摇
        }
        if (CanNextAttack(2))//能进行第二段攻击
        {
            type = ModContent.ProjectileType<TickScytheAtkA2>();
            InitialNextComboAttack(160, 4);
        }
        if (CanNextAttack(3))
        {
            type = ModContent.ProjectileType<TickScytheAtkB2>();
            damage = DamageMul(0.88f);
            InitialNextComboAttack(120, 4);
        }
        if (CanNextAttack(4))
        {
            type = ModContent.ProjectileType<TickScytheAtkA1>();
            damage = DamageMul(1.2f);
            FinalComboAttack(8); //后摇
        }
    }


}