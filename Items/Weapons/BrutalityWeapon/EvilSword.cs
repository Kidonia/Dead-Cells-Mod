using DeadCells.Projectiles.WeaponAnimationProj;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using DeadCells.Common.Buffs;
using DeadCells.Common.DamageClasses;

namespace DeadCells.Items.Weapons.BrutalityWeapon;

public class EvilSword : DeadCellsItem
{
    public override void SetDefaults()
    {
        SetWeaponDefaults(BrutalityDamage.Instance, 25, 2.1f, 2, 6, 2500);
    }
    public override void HoldItem(Player player)
    {
        player.AddBuff(ModContent.BuffType<BeCursed>(), 2);
        base.HoldItem(player);
    }
    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (FirstAttack())
        {
            type = ModContent.ProjectileType<EvilSwordAtkA>();
            InitialNextComboAttack(30, 6); //可接上第二段攻击时间间隔， 第二段攻击的前摇
        }
        if (CanNextAttack(2))//能进行第二段攻击
        {
            type = ModContent.ProjectileType<EvilSwordAtkB>();
            InitialNextComboAttack(30, 1);
        }
        if (CanNextAttack(3))//能进行第二段攻击
        {
            type = ModContent.ProjectileType<EvilSwordAtkA>();
            damage = DamageMul(1.48f);
            FinalComboAttack(20); //后摇
        }
    }
}
