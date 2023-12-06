using DeadCells.Common.Players;
using DeadCells.Projectiles;
using DeadCells.Projectiles.WeaponAnimationProj;
using System;
using Terraria;
using Terraria.ModLoader;

namespace DeadCells.Common.GlobalNPCs;

public class CheckBuffDPS : GlobalNPC
{
    //这个必须写，没有为什么，不然加载不了
    public override bool InstancePerEntity => true;

    public int CheckBleedDps = 1;
    public Player player => Main.player[Main.myPlayer];

    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    { 
        int type = projectile.type;
        var mul = player.GetModPlayer<PlayerEquipAndScroll>();
        if(type == ModContent.ProjectileType<BleederAtkA>() || type == ModContent.ProjectileType<BleederAtkB>())//血刀
        {
            CheckBleedDps += (int)Math.Round(2*(mul.BrutalityMul +1));//@@需要添加卷轴伤害@ 完成？
        }
        if(type == ModContent.ProjectileType<Kunai>())//圆舞飞刀
        {
            CheckBleedDps += (int)Math.Round(10 * (mul.BrutalityMul + 1));//@@需要添加卷轴伤害@ 完成？
        }
    }


}
