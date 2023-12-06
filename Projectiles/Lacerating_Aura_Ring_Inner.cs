using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeadCells.Projectiles;

public class Lacerating_Aura_Ring_Inner : ModProjectile
{
    private double rotation;
    public Vector2 SpawnCenter;
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("杀戮光环");
        ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.width = 34;
        Projectile.height = 34;
        Projectile.DamageType = BrutalityDamage.Instance;

        Projectile.ignoreWater = true;
        Projectile.timeLeft = 210;
        Projectile.tileCollide = false;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 12;
        
    }

    public override bool PreDraw(ref Color lightColor)
    {
        lightColor = new Color(150, 150, 150, 120);
        return true;
    }
    public override void OnSpawn(IEntitySource source)
    {
        
        Player player = Main.player[Projectile.owner];



        Vector2 vector = player.Center - Projectile.Center;
        Projectile.rotation = vector.ToRotation() + (float)Math.PI / 2f;

        rotation = Projectile.Center.ToRotation();
        Projectile.Center = player.Center + Terraria.Utils.RotatedBy(new Vector2(140f, 0f), rotation);

    }

    public override void AI()
    {

        Player player = Main.player[Projectile.owner];
        if (player.dead)
        {
            Projectile.active = false;
        }
        Lighting.AddLight(Projectile.Center, 0.15f, 0.15f, 0.15f);

        Vector2 vector = player.Center - Projectile.Center;
        Projectile.rotation = vector.ToRotation() + (float)Math.PI / 2f;

        // 旋转点速率可以自行调整
        float t = (float)Main.time * 0.07f;
        // 目标位置
        Vector2 targetPos = player.Center + new Vector2(1.5f * (float)Math.Cos(t+ rotation), 2 * (float)Math.Sin(t+ rotation)) * 50f;

        Projectile.velocity = (targetPos - Projectile.Center);

    }




    public override bool PreKill(int timeLeft)
    {
        //周围一圈生成粒子
        for (double r = 0f; r < MathHelper.TwoPi; r += MathHelper.TwoPi / Main.rand.Next(9, 14))
        {
            Vector2 newposition = new Vector2((float)Math.Cos(r), (float)Math.Sin(r)) * 27f + new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-3, 3));
            Dust dust = Dust.NewDustPerfect(Projectile.Center + newposition, DustID.GemDiamond, Vector2.Zero);
            dust.rotation = 0f;
            dust.fadeIn = 0.3f + Main.rand.NextFloat() * 1.2f;
        }
        return true;
    }


}
