using DeadCells.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace DeadCells.Projectiles.EffectProj;


public class TeleportAfter : ModProjectile
{
    public override string Texture => AssetsLoader.TransparentImg;

    public override void SetDefaults()
    {
        Projectile.width = 2;
        Projectile.height = 2;
        Projectile.aiStyle = -1;
        Projectile.timeLeft = 2;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
    }
    public override void AI()
    {
        Projectile.velocity = Vector2.Zero;
        for (float r = 0f; r < MathHelper.TwoPi; r += MathHelper.TwoPi / 32f)
        {
            Vector2 vector = Vector2.UnitX.RotatedBy(r);
            int type = Dust.NewDust(Projectile.position + vector * 20, 1, 1, 88, 0, 0, 0, default, 0.96f);
            Main.dust[type].noGravity = true;
            Main.dust[type].velocity = vector * 1.2f;
        };
        base.AI();
    }
}

