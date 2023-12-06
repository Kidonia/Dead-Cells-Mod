using Microsoft.Xna.Framework;
using DeadCells.Common.Players;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeadCells.Projectiles;

public class LineWhiteSharp : ModProjectile
{
    private double rotation;
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("杀戮光环白色特效");
        ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.width = 7;
        Projectile.height = 17;
        Projectile.damage = 0;
        Projectile.DamageType = DamageClass.Default;
        Projectile.ignoreWater = true;
        Projectile.timeLeft = 2;
        Projectile.tileCollide = false;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
    }
    public override bool PreDraw(ref Color lightColor)
    {
            float m = Projectile.ai[1] / 2;
            lightColor = new Color(33 - m, 52 - m, 75 - m, 0);
            return true;
    }
    public override void OnSpawn(IEntitySource source)
    {
        Player player = Main.player[Projectile.owner];
        var ply = player.GetModPlayer<PlayerDraw>();
        ply.draw_lacerating_rings = true; //传参，绘制特效

        Projectile.rotation = Projectile.Center.ToRotation() + (float)Math.PI / 2f; //调整生成时的角度
        rotation = Projectile.Center.ToRotation();
        if (rotation < 0)
        {
            rotation += MathHelper.TwoPi;
        }
        Projectile.Center = player.Center + Terraria.Utils.RotatedBy(new Vector2(146f - Projectile.ai[1], 0f), rotation);
        Projectile.scale *= Main.rand.NextFloat(Projectile.ai[0]-0.1f, Projectile.ai[0]+0.4f); //随机大小
    }
}
