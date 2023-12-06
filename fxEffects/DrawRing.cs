using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DeadCells.Core;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace DeadCells.fxEffects;

public class DrawRing : ModProjectile
{
    public override void SetStaticDefaults()
    {
    }
    public override void SetDefaults()
    {
        Projectile.extraUpdates = 1;
        Projectile.width = 24;
        Projectile.height = 24;
        Projectile.hostile = false;
        Projectile.friendly = false;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 160;
        Projectile.aiStyle = -1;
    }
    public override void AI()
    {
        Projectile.hide = true;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        float num = (160 - Projectile.timeLeft) / (float)Projectile.timeLeft * 1.2f;
        if (num < 2.2f&&num >0.1f)
        {
            //ai[0], ai[1]用来调颜色，后续可能会调整。
            DrawCircle(num * 125f, 15f * (1.2f - num) + 4f, new Color(Projectile.ai[1], (Projectile.ai[0] + 0.02f) * (1f - num), (Projectile.ai[0] + 0.04f) * (1f - num), 0f), Projectile.Center - Main.screenPosition);
        }
        return false;
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        behindProjectiles.Add(index);
    }
    private static void DrawCircle(float radious, float width, Color color, Vector2 center)
    {

        List<Vertex2D> list = new List<Vertex2D>();
        for (int i = 0; i < radious / 2f; i++)
        {
            list.Add(new Vertex2D(center + Terraria.Utils.RotatedBy(new Vector2(0f, radious), (double)(i / radious) * Math.PI * 4.0), color, new Vector3(0.5f, 1f, 0f)));
            list.Add(new Vertex2D(center + Terraria.Utils.RotatedBy(new Vector2(0f, radious + width), (double)(i / radious) * Math.PI * 4.0), color, new Vector3(0.5f, 0f, 0f)));
        }

        list.Add(new Vertex2D(center + new Vector2(0f, radious), color, new Vector3(0.5f, 1f, 0f)));
        list.Add(new Vertex2D(center + new Vector2(0f, radious + width), color, new Vector3(0.5f, 0f, 0f)));
        if (list.Count > 0)
        {
            Texture2D texture = ModContent.Request<Texture2D>("DeadCells/fxEffects/Wave").Value;
            Main.graphics.GraphicsDevice.Textures[0] = (Texture)(object)texture;
            Main.graphics.GraphicsDevice.DrawUserPrimitives((PrimitiveType)1, list.ToArray(), 0, list.Count - 2);
        }
    }
    
}
