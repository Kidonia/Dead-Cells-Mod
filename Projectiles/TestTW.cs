using DeadCells.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DeadCells.Projectiles;

public class TestTW : ModProjectile
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
    public override void OnSpawn(IEntitySource source)
    {
        //Projectile.ai[0] = 255;
        base.OnSpawn(source);
    }
    public override void AI()
    {
        Projectile.position = Main.screenPosition;
        
        if (Projectile.timeLeft > 22)
            Projectile.ai[0] += 25;
        if (Projectile.timeLeft < 12)
            Projectile.ai[0] -= 25;
    }

    public override void PostDraw(Color lightColor)
    {
        base.PostDraw(lightColor);
        /*
        SpriteBatch sb = Main.spriteBatch;
        Texture2D whitedot = ModContent.Request<Texture2D>(AssetsLoader.WhiteDotImg, (AssetRequestMode)1).Value;
;
        sb.Draw(whitedot,
    Projectile.Center - Main.screenPosition,
    null,
    new(255, 255, 255, 255 - Projectile.alpha),
     0,
    new Vector2(0.5f, 0.5f),
    new Vector2(800, 800),
    SpriteEffects.None,
    0);

        base.PostDraw(lightColor);
 */
    }
       
}
