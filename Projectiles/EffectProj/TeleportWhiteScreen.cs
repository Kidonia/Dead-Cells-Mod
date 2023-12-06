using DeadCells.Core;
using Terraria;
using Terraria.ModLoader;

namespace DeadCells.Projectiles.EffectProj;

public class TeleportWhiteScreen : ModProjectile
{
    public override string Texture => AssetsLoader.TransparentImg;
    public override void SetDefaults()
    {
        Projectile.width = 100;
        Projectile.height = 100;
        Projectile.aiStyle = -1;
        Projectile.timeLeft = 32;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        base.SetDefaults();
    }

    public override void AI()
    {
        Projectile.position = Main.screenPosition;

        if (Projectile.timeLeft > 22)
            Projectile.ai[0] += 25;
        if (Projectile.timeLeft < 12)
            Projectile.ai[0] -= 20;
    }

}
