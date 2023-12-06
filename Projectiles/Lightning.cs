using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeadCells.Projectiles;

public class Lightning : ModProjectile
{
    private double rotation;

    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("环");
        ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
    }

    public override void SetDefaults()
    {
        //ai[0]用于判断是不是玩家生成的。确定环的半径，颜色, 时间
        Projectile.width = 52;
        Projectile.height = 34;
        Projectile.DamageType = DamageClass.Default;
        Projectile.damage = 0;
        Projectile.ignoreWater = true;
        Projectile.timeLeft = 2;
        Projectile.tileCollide = false;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;

    }



    public override bool PreDraw(ref Color lightColor)
    {
        lightColor = new Color(37, 62, 85, 0);
        return true;
    }
    public override void OnSpawn(IEntitySource source)
    {

        Player player = Main.player[Projectile.owner];

        Projectile.rotation = Projectile.Center.ToRotation() + (float)Math.PI / 2f;

        rotation = Projectile.Center.ToRotation();
        if (rotation < 0)
        {
            rotation += MathHelper.TwoPi;
        }
            Projectile.Center = player.Center + Terraria.Utils.RotatedBy(new Vector2(146f, 0f), rotation);
            Projectile.scale *= Main.rand.NextFloat(0.9f, 1.1f);
    }

}
