using DeadCells.Core;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DeadCells.Projectiles.NPCMeleeProj;

public class DC_EnmMeleeAtk : ModProjectile
{
    public override string Texture => AssetsLoader.TransparentImg;
    public override void SetDefaults()
    {
        Projectile.hostile = true;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 10;
        Projectile.hide = true;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
    }
    public override void OnSpawn(IEntitySource source)
    {
        Projectile.width = (int)Projectile.ai[0];
        Projectile.height = (int)Projectile.ai[1];
        base.OnSpawn(source);
    }
    public override void AI()
    {

        base.AI();
    }



}
