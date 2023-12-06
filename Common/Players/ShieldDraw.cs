using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace DeadCells.Common.Players;

public class ShieldDraw : PlayerDrawLayer
{

    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Shield);
    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        DrawPlayerShieldExtraParry(ref drawInfo);
    }

    public static void DrawPlayerShieldExtraParry(ref PlayerDrawSet drawinfo)//纯纯的源码
    {
         var parry = drawinfo.drawPlayer.GetModPlayer<Parry>();
        if (parry.extraShieldRaised && drawinfo.drawPlayer.shield > 0 && drawinfo.drawPlayer.shield < 10)
        {
            Vector2 zero = Vector2.Zero;
            if (parry.extraShieldRaised)
            {
                zero.Y -= 4f * drawinfo.drawPlayer.gravDir;
            }
            Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
            Vector2 zero2 = Vector2.Zero;
            Vector2 bodyVect = drawinfo.bodyVect;
            if (bodyFrame.Width != TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value.Width)
            {
                bodyFrame.Width = TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value.Width;
                bodyVect.X += (float)(bodyFrame.Width - TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value.Width);
                if (drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally))
                {
                    bodyVect.X = bodyFrame.Width - bodyVect.X;
                }
            }
            DrawData item;
            if (parry.extraShieldRaised)
            {
                float num = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 6.28318548f);
                float x = 2.5f + 1.5f * num;
                Color color = drawinfo.colorArmorBody;
                color.A = 0;
                color *= 0.45f - num * 0.15f;
                for (float num2 = 0f; num2 < 4f; num2 += 1f)
                {
                    item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value, 
                        zero2 + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (int)(drawinfo.Position.Y - Main.screenPosition.Y + drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2) + zero + new Vector2(x, 0f).RotatedBy((double)(num2 / 4f * 6.28318548f), default), 
                        new Rectangle?(bodyFrame), 
                        color, 
                        drawinfo.drawPlayer.bodyRotation, 
                        bodyVect, 
                        1f, 
                        drawinfo.playerEffect, 
                        0);
                    item.shader = drawinfo.cShield;
                    drawinfo.DrawDataCache.Add(item);
                }
            }
            item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value, 
                zero2 + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (int)(drawinfo.Position.Y - Main.screenPosition.Y + drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2) + zero, 
                new Rectangle?(bodyFrame), 
                drawinfo.colorArmorBody, 
                drawinfo.drawPlayer.bodyRotation, 
                bodyVect, 
                1f, 
                drawinfo.playerEffect, 
                0);
            item.shader = drawinfo.cShield;
            drawinfo.DrawDataCache.Add(item);
            if (parry.extraShieldRaised)
            {
                Color color2 = drawinfo.colorArmorBody;
                float num3 = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3.14159274f);
                color2.A = (byte)(color2.A * (0.5f + 0.5f * num3));
                color2 *= 0.5f + 0.5f * num3;
                item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value, 
                    zero2 + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (int)(drawinfo.Position.Y - Main.screenPosition.Y + drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2) + zero,
                    new Rectangle?(bodyFrame), 
                    color2,
                    drawinfo.drawPlayer.bodyRotation,
                    bodyVect,
                    1f, 
                    drawinfo.playerEffect,
                    0);
                item.shader = drawinfo.cShield;
            }
            if (parry.extraShieldRaised && drawinfo.drawPlayer.shieldParryTimeLeft > 0)
            {
                float num4 = drawinfo.drawPlayer.shieldParryTimeLeft / 20f;
                float num5 = 1.5f * num4;
                Vector2 vector = zero2 + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (int)(drawinfo.Position.Y - Main.screenPosition.Y + drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2) + zero;
                Color color3 = drawinfo.colorArmorBody;
                float num6 = 1f;
                Vector2 value = drawinfo.Position + drawinfo.drawPlayer.Size / 2f - Main.screenPosition;
                Vector2 value2 = vector - value;
                vector += value2 * num5;
                num6 += num5;
                color3.A = (byte)(color3.A * (1f - num4));
                color3 *= 1f - num4;
                item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value, 
                    vector, 
                    new Rectangle?(bodyFrame), 
                    color3, drawinfo.drawPlayer.bodyRotation,
                    bodyVect, 
                    num6, 
                    drawinfo.playerEffect,
                    0);
                item.shader = drawinfo.cShield;
                drawinfo.DrawDataCache.Add(item);
            }
            if (drawinfo.drawPlayer.mount.Cart)
            {
                drawinfo.DrawDataCache.Reverse(drawinfo.DrawDataCache.Count - 2, 2);
            }
        }
    }
}
