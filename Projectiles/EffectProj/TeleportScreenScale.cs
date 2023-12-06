using DeadCells.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DeadCells.Projectiles.EffectProj;

public class TeleportScreenScale : ModProjectile
{
    public override string Texture => AssetsLoader.TransparentImg;
    public override void SetDefaults()
    {
        Projectile.width = 50;
        Projectile.height = 50;
        Projectile.aiStyle = -1;
        Projectile.timeLeft = 122;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        base.SetDefaults();
    }

    public override void AI()
    {
        Projectile.position = Main.screenPosition;

        if (Projectile.timeLeft > 100)
        { }
        else if (Projectile.timeLeft > 60)
            Projectile.ai[0] = MathHelper.Lerp(1f, 1.18f, 0.02f * (100 - Projectile.timeLeft));
        else if (Projectile.timeLeft > 28)
            Projectile.ai[0] += 0.022f;
        else if (Projectile.timeLeft > 12)
        {
            Projectile.ai[0] = MathHelper.Lerp(2f, 1f, (28 - Projectile.timeLeft) / 15f);
            Projectile.ai[1] = MathHelper.Lerp(0.005f, 0f, (28 - Projectile.timeLeft) / 16f);
        }

        if(Projectile.timeLeft >28)
            Projectile.ai[1] = MathHelper.Lerp(0f, 0.005f, 0.0106f * (122 - Projectile.timeLeft));

    }
}
