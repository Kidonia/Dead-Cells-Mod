using DeadCells.Core;
using Terraria.ModLoader;

namespace DeadCells.Projectiles.EffectProj;

public class RoundTwist2 : ModProjectile
{
    public override string Texture => AssetsLoader.TransparentImg;
    public override void SetDefaults()
    {
        Projectile.width = 100;
        Projectile.height = 100;
        Projectile.aiStyle = -1;
        Projectile.timeLeft = 22;
        Projectile.tileCollide = false;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 1;
        Projectile.penetrate = -1;
        base.SetDefaults();
    }

    public override void AI()
    {
        Projectile.ai[0] += 0.21f;
    }
}
