using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;
using DeadCells.Common.Players;
using DeadCells.Core;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace DeadCells.Projectiles;

public class Lacerating_Aura_Ring : ModProjectile
{
    private double rotation;
    private float distance = 25f;
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("杀戮光环外圈");
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
        lightColor = new Color(220, 220, 220, 220);
        return true;
    }
    public override void OnSpawn(IEntitySource source)
    {
        Player player = Main.player[Projectile.owner];
        var ply = player.GetModPlayer<PlayerDraw>();
        ply.draw_lacerating_rings = true;

        Projectile.rotation = Projectile.Center.ToRotation() + (float)Math.PI / 2f;

        rotation = Projectile.Center.ToRotation();
        if (rotation < 0)
        {
            rotation += MathHelper.TwoPi;
        }
        Projectile.Center = player.Center + Terraria.Utils.RotatedBy(new Vector2(distance, 0f), rotation);
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



        if(distance < 145f)
        {
            distance += 10f;
        }
        Projectile.Center = player.Center + Terraria.Utils.RotatedBy(new Vector2(distance, 0f), rotation);//距离
        rotation += 0.065f;//转速
        if (rotation >= 360.0)
        {
            rotation = 0.0;
        }

    }
    public override bool PreKill(int timeLeft)
    {
        Player player = Main.player[Projectile.owner];
        var ply = player.GetModPlayer<PlayerDraw>();
        ply.draw_lacerating_rings = false;


        for (double r = 0f; r < MathHelper.TwoPi; r += MathHelper.TwoPi / Main.rand.Next(9, 14))//消失时生成一圈粒子，@要修改材质
        {
            Vector2 newposition = new Vector2((float)Math.Cos(r), (float)Math.Sin(r)) * 27f + new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-3, 3));
            Dust dust = Dust.NewDustPerfect(Projectile.Center + newposition, DustID.GemDiamond, Vector2.Zero);
            dust.rotation = 0f;
            dust.fadeIn = 0.3f + Main.rand.NextFloat() * 1.2f;
        }

        SoundEngine.PlaySound(AssetsLoader.active_laceration_end);
        return true;
    }


}
