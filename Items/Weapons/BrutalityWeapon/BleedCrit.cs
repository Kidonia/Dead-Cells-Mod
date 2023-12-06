using Microsoft.Xna.Framework;
using DeadCells.Projectiles.WeaponAnimationProj;
using Terraria;
using Terraria.ModLoader;
using DeadCells.Common.DamageClasses;

namespace DeadCells.Items.Weapons.BrutalityWeapon;

public class BleedCrit : DeadCellsItem
{
    public override void SetDefaults()
    {
        SetWeaponDefaults(BrutalityDamage.Instance, 20, 2f, 2, 6, 1500);
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (FirstAttack())
        {
            type = ModContent.ProjectileType<BleedCritAtkA>();
            damage = DamageMul(0.8f);
            InitialNextComboAttack(74, 18); //可接上第二段攻击时间间隔， 第二段攻击的前摇
        }
        if (CanNextAttack(2))//能进行第二段攻击
        {
            type = ModContent.ProjectileType<BleedCritAtkB>();
            damage = DamageMul(0.65f);
            InitialNextComboAttack(40, 10); 
        }
        if (CanNextAttack(3))
        {
            type = ModContent.ProjectileType<BleedCritAtkB>();
            damage = DamageMul(0.65f);
            InitialNextComboAttack(60, 15);
        }
        if (CanNextAttack(4))
        {
            type = ModContent.ProjectileType<BleedCritAtkC>();
            damage = DamageMul(1.2f);
            FinalComboAttack(30); //后摇
        }
    }


}
