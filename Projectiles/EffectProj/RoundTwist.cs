using DeadCells.Core;
using Terraria.ModLoader;

namespace DeadCells.Projectiles.EffectProj;

public class RoundTwist : ModProjectile
{
    public override string Texture => AssetsLoader.TransparentImg;
    public override void SetDefaults()
    {
        Projectile.width = 100;
        Projectile.height = 100;
        Projectile.aiStyle = -1;
        Projectile.timeLeft = 24;
        Projectile.tileCollide = false;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 1;
        Projectile.penetrate = -1;
    }

    public override void AI()
    {
        if (Projectile.timeLeft > 10)
            Projectile.ai[0] += 0.2f;
        else
        {
            Projectile.ai[0] -= 0.3f;
        }
    }
}
