using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DeadCells.UI;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using DeadCells.Core;
using System.Collections.Generic;
using System;

namespace DeadCells.Common;

public class DCMenu : ModMenu
{
    private int boatTime;
    private static int fogtime = 2560;
    private static int fogtime2 = 3200;
    public static List<ScreenElements> EleBird { get; internal set; } = new List<ScreenElements>();
    private static Vector2 birdPos = new Vector2(Main.screenWidth / 2 - 38, Main.screenHeight / 2 - 30);
    public static List<ScreenElements> EleSmallBird { get; internal set; } = new List<ScreenElements>();
    private static Vector2 smbirdPos1 = new Vector2(Main.screenWidth / 2 + 192, 36);//8 中上
    private static Vector2 smbirdPos2 = new Vector2(Main.screenWidth  - 144, Main.screenHeight / 2 + 64);//4 右下
    public static List<ScreenElements> EleFog { get; internal set; } = new List<ScreenElements>();
    public static List<ScreenElements> EleHorz { get; internal set; } = new List<ScreenElements>();
    public static List<ScreenElements> EleWave { get; internal set; } = new List<ScreenElements>();

    public static List<ScreenElements> EleCrow { get; internal set; } = new List<ScreenElements>();
    private int CrowSpawnCoolDown;
    public static List<ScreenElements> EleGraydust { get; internal set; } = new List<ScreenElements>();
    public static List<ScreenElements> EleDustLens { get; internal set; } = new List<ScreenElements>();

    private const string menuAssetPath = "DeadCells/Assets/Backgrounds";

    private Texture2D background = ModContent.Request<Texture2D>($"{menuAssetPath}/Background2", (AssetRequestMode)1).Value;
    private Texture2D whitedot = ModContent.Request<Texture2D>(AssetsLoader.WhiteDotImg, (AssetRequestMode)1).Value;
    private Texture2D boat = ModContent.Request<Texture2D>($"{menuAssetPath}/TitleBoat", (AssetRequestMode)1).Value;

    private Texture2D birdTex = ModContent.Request<Texture2D>($"{menuAssetPath}/bird", (AssetRequestMode)1).Value;
    private Texture2D smallBirdTex = ModContent.Request<Texture2D>($"{menuAssetPath}/smallBird", (AssetRequestMode)1).Value;

    private Texture2D crowTex = ModContent.Request<Texture2D>($"{menuAssetPath}/critterCrowFly", (AssetRequestMode)1).Value;
    private Texture2D grayDust = ModContent.Request<Texture2D>($"{menuAssetPath}/fxCircleWhite", (AssetRequestMode)1).Value;

    private Texture2D dustLensBig = ModContent.Request<Texture2D>($"{menuAssetPath}/dustLensBig", (AssetRequestMode)1).Value;
    private Texture2D dustLensFG = ModContent.Request<Texture2D>($"{menuAssetPath}/dustLensFG", (AssetRequestMode)1).Value;
    private Texture2D dustLensSmall = ModContent.Request<Texture2D>($"{menuAssetPath}/dustLensSmall", (AssetRequestMode)1).Value;

    private Texture2D wave1 = ModContent.Request<Texture2D>($"{menuAssetPath}/fxFlatWave0", (AssetRequestMode)1).Value;
    private Texture2D wave2 = ModContent.Request<Texture2D>($"{menuAssetPath}/fxFlatWave1", (AssetRequestMode)1).Value;
    private Texture2D wave3 = ModContent.Request<Texture2D>($"{menuAssetPath}/fxFlatWave2", (AssetRequestMode)1).Value;

    private Texture2D fog = ModContent.Request<Texture2D>($"{menuAssetPath}/fog", (AssetRequestMode)1).Value;
    
    public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/DC_Main");

    public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{menuAssetPath}/Empty", (AssetRequestMode)2);
    public override Asset<Texture2D> SunTexture => ModContent.Request<Texture2D>($"{menuAssetPath}/Empty", (AssetRequestMode)2);
    public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>($"{menuAssetPath}/Empty", (AssetRequestMode)2);
    public override string DisplayName => "Dead Cells Island";
    public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<DCTitleScreen>();

    public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
    {
        
        Vector2 drawOffset = Vector2.Zero;
        float xScale = (float)Main.screenWidth / background.Width;
        float yScale = (float)Main.screenHeight / background.Height;
        float scale = xScale;
        if (xScale != yScale)
        {
            if (yScale > xScale)
            {
                scale = yScale;
                drawOffset.X -= (background.Width * scale - Main.screenWidth) * 0.5f;
            }
            else
            {
                drawOffset.Y -= (background.Height * scale - Main.screenHeight) * 0.5f + 20;
            }
        }
        //确定水面的位置，即船的位置，地平线位置，倒影位置
        Vector2 boatPos = drawOffset + new Vector2(background.Width / 2, 1037f) * scale + new Vector2(45, 0);

        //背景绘制
        spriteBatch.Draw(background, drawOffset, null, Color.White, 0f, Vector2.Zero, scale, 0, 0f);
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        int num = Main.screenWidth / fog.Width + 1;//雾气
        if (EleFog.Count == 0)//一开始
        {
            Vector2 vel = new Vector2(0.2f, 0);
            Vector2 vel2 = new Vector2(0.16f, 0);

            for (int m = 0; m <= num; m++)
            {
                Vector2 startingPosition = new Vector2(m * fog.Width, boatPos.Y);
                EleFog.Add(new ScreenElements(0, EleFog.Count, new(0, 0, 8, 0), startingPosition, vel));
                EleFog.Add(new ScreenElements(0, EleFog.Count, new(0, 0, 8, 0), startingPosition, vel2));
            }

        }

        fogtime--;
        if (fogtime == 0)
        {
            fogtime = 2560;
            Vector2 vel = new Vector2(0.2f, 0);
            Vector2 startingPosition = new Vector2(0, boatPos.Y);
            EleFog.Add(new ScreenElements(0, EleFog.Count, new(0, 0, 0, 0), startingPosition, vel));
        }

        fogtime2--;
        if (fogtime2 == 0)
        {
            fogtime2 = 3200;
            Vector2 vel2 = new Vector2(0.16f, 0);
            Vector2 startingPosition = new Vector2(0, boatPos.Y);
            EleFog.Add(new ScreenElements(0, EleFog.Count, new(0, 0, 0, 0), startingPosition, vel2));
        }

        for (int i = 0; i < 3; i++)//添加背景元素属性
        {
            if (Main.rand.NextBool(10) && EleGraydust.Count < 80)//灰尘
            {
                int lifetime = 290;
                float dscale = Main.rand.NextFloat(0.02f, 0.27f);
                int randColor = Main.rand.Next(55, 100) - (int)(12 * dscale);

                Vector2 startingPosition = new Vector2(Main.screenWidth * Main.rand.NextFloat(-0.1f, 1.1f), Main.screenHeight * Main.rand.NextFloat(0.55f, 1f));
                Vector2 startingVelocity = new Vector2(Main.rand.NextFloat(-0.8f, -0.11f), Main.rand.NextFloat(0f, 0.5f)) * 0.72f;
                EleGraydust.Add(new ScreenElements(lifetime + randColor, EleGraydust.Count, default, startingPosition, startingVelocity, dscale));
            }
            ///////////////////////////////////////////////////////////////////////////

            if (Main.rand.NextBool(60) && CrowSpawnCoolDown == 0)//乌鸦
            {
                CrowSpawnCoolDown = Main.rand.Next(475, 640);
                int a = Main.rand.Next(-2, 3);
                int LorR = Main.rand.Next(0, 2) * 2 - 1;
                Vector2 area = new Vector2(Main.screenWidth * Main.rand.NextFloat(0.3f, 0.7f), Main.screenHeight * Main.rand.NextFloat(0.58f, 0.92f));
                Vector2 dirc = Vector2.UnitX.RotatedBy(Main.rand.NextFloat(0.15f) * LorR) * LorR;
                for (int crownum = 0; crownum < 5 + a; crownum++)
                {
                    int lifetime = 180;
                    float escale = Main.rand.NextFloat(0.8f, 1.3f);

                    Vector2 startingPosition = area + new Vector2(Main.rand.Next(-60, 60), Main.rand.Next(-50, 50));
                    Vector2 startingVelocity = (dirc + new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.1f, 0.1f))) * Main.rand.NextFloat(7f, 7.4f);
                    Color color = new(0, 0, 0, 10);
                    EleCrow.Add(new ScreenElements(lifetime, EleCrow.Count, color, startingPosition, startingVelocity, escale, Main.rand.Next(0, 5)));
                }
            }
            ///////////////////////////////////////////////////////////////////////////
            
            if (EleBird.Count < 3)//鸟
            {
                int lifetime = 18000;
                Vector2 startingPosition = birdPos;
                Vector2 startingVelocity = Vector2.Zero;
                Color cinderColor = new(20, 20, 20, 220);
                EleBird.Add(new ScreenElements(lifetime, EleBird.Count, cinderColor, startingPosition, startingVelocity, 1.2f, EleBird.Count * 60));
            }
            ///////////////////////////////////////////////////////////////////////////

            if (EleSmallBird.Count < 12)//小苍蝇鸟
            {
                //Velocity用来传参用，绘制时用不到。
                int lifetime = 18000;
                Vector2 startingVelocity = Vector2.UnitX.RotatedByRandom(Math.PI * 2);
                if (EleSmallBird.Count < 4)//右下角
                {
                    Vector2 startingPosition = smbirdPos2;
                    Color cinderColor = new(52, 24, 24, 160);
                    EleSmallBird.Add(new ScreenElements(lifetime, EleSmallBird.Count, cinderColor, startingPosition, startingVelocity, 1.32f, EleSmallBird.Count * 60));
                }
                else//中上角
                {
                    Vector2 startingPosition = smbirdPos1;
                    Color cinderColor = new(40, 25, 20, 105);
                    EleSmallBird.Add(new ScreenElements(lifetime, EleSmallBird.Count, cinderColor, startingPosition, startingVelocity, 1f, EleSmallBird.Count * 60));

                }
            }
            ///////////////////////////////////////////////////////////////////////////

            if (Main.rand.NextBool(10) && EleDustLens.Count < 28)//光晕
            {
                int lifetime = Main.rand.Next(640, 720);
                float gscale = Main.rand.NextFloat(0.8f, 1f);

                Vector2 startingPosition = new Vector2(Main.screenWidth * Main.rand.NextFloat(0f, 1f), Main.screenHeight * Main.rand.NextFloat(0f, 1f));
                Vector2 startingVelocity = new Vector2(Main.rand.NextFloat(-0.2f, -0.2f), Main.rand.NextFloat(-0.2f, 0.2f)) * 1.2f;
                Color color = new(0, 0, 0, 0);
                EleDustLens.Add(new ScreenElements(lifetime, EleDustLens.Count, color, startingPosition, startingVelocity, gscale));
            }



            ///////////////////////////////////////////////////////////////////////////

            if (Main.rand.NextBool(4))//水平线
            {
                int lifetime = Main.rand.Next(240, 300);
                float sscale = Main.rand.NextFloat(1f, 1.2f);
                Vector2 vel = new Vector2(Main.rand.NextFloat(-1.4f, 1.4f), 0);
                //boatPos
                Vector2 startingPosition = new Vector2(Main.screenWidth * Main.rand.NextFloat(0.4f, 0.8f), boatPos.Y + Main.rand.Next(-2, 3));
                Color color = new(0, 0, 0, 0);
                EleHorz.Add(new ScreenElements(lifetime, EleHorz.Count, color, startingPosition, vel, sscale));
            }
            ///////////////////////////////////////////////////////////////////////////

            if (Main.rand.NextBool(3))//波纹
            {
                //Scale用于传参，一般波纹为1                    
                int lifetime = Main.rand.Next(260, 320);
                Vector2 startingPosition = new Vector2(Main.screenWidth * Main.rand.NextFloat(0f, 1f), boatPos.Y + Main.screenHeight * Main.rand.NextFloat(0f, 0.18f));
                Vector2 startingVelocity = new Vector2(Main.rand.NextFloat(-0.2f, 0.2f), Main.rand.NextFloat(0f, 0.3f)) * 1.2f;
                EleWave.Add(new ScreenElements(lifetime, EleWave.Count, Color.White, startingPosition, startingVelocity, 1));
            }
            ///////////////////////////////////////////////////////////////////////////

            if (Main.rand.NextBool(7))//波纹
            {
                //Scale用于传参，中间亮的为3                    
                int lifetime = Main.rand.Next(260, 320);
                float range = Main.rand.NextFloat(0.42f, 0.82f);
                Vector2 startingPosition = new Vector2(Main.screenWidth * range, boatPos.Y + Main.screenHeight * Main.rand.NextFloat(0f, 0.1f));
                Vector2 startingVelocity = new Vector2(Main.rand.NextFloat(-0.1f, 0.1f) * 1.2f - range, Main.rand.NextFloat(0.32f, 0.5f));
                EleWave.Add(new ScreenElements(lifetime, EleWave.Count, Color.White, startingPosition, startingVelocity, 3));
            }
        }


        //元素内容随时间变化：
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        for (int j = 0; j < EleDustLens.Count; j++)//光晕
        {
            EleDustLens[j].Time++;
            int t = EleDustLens[j].Time;
            EleDustLens[j].Center += EleDustLens[j].Velocity;


            if (EleDustLens[j].Time < 32)
            {
                EleDustLens[j].DrawColor = new(2 * t, t, 0, 7 * t);
            }
            if (EleDustLens[j].Lifetime - EleDustLens[j].Time < 35)
            {
                EleDustLens[j].DrawColor.R = (byte)MathHelper.Lerp(EleDustLens[j].DrawColor.R, 0, 0.0285f);
                EleDustLens[j].DrawColor.G = (byte)MathHelper.Lerp(EleDustLens[j].DrawColor.G, 0, 0.0285f);
                EleDustLens[j].DrawColor.B = (byte)MathHelper.Lerp(EleDustLens[j].DrawColor.B, 0, 0.0285f);
                EleDustLens[j].DrawColor.A = (byte)MathHelper.Lerp(EleDustLens[j].DrawColor.A, 0, 0.0285f);
            }

        }
        EleDustLens.RemoveAll((ScreenElements c) => c.Time >= c.Lifetime);
        /////////////////////////////////////////////////////////////
        
        for (int j = 0; j < EleFog.Count; j++)//雾气
        {
            EleFog[j].Time++;
            int t = EleFog[j].Time;

            EleFog[j].Center += EleFog[j].Velocity;

            if (EleFog[j].Time < 52)
            {
                EleFog[j].DrawColor = new Color(t * 3, t , 8, 150);
            }

            if (EleFog[j].Center.X < 50 && EleFog[j].DrawColor.A < 150)
            {
                EleFog[j].DrawColor.A += 3;
            }
            else if (EleFog[j].Center.X > 150)
                EleFog[j].DrawColor.A = 150;

        }
        EleFog.RemoveAll((ScreenElements c) => c.Center.X > Main.screenWidth + fog.Size().X);

        /////////////////////////////////////////////////////////////
        for (int j = 0; j < EleHorz.Count; j++)//地平线
        {
            EleHorz[j].Time++;
            EleHorz[j].Lifetime--;
            int t = EleHorz[j].Time;
            EleHorz[j].Center += EleHorz[j].Velocity;

            if (EleHorz[j].Time < 32)
            {
                EleHorz[j].DrawColor = new Color(t * 2, t , 8, 140);
            }
            if (200 - EleHorz[j].Time < 35)
            {
                EleHorz[j].DrawColor.R = (byte)MathHelper.Lerp(EleHorz[j].DrawColor.R, 0, 0.0285f);
                EleHorz[j].DrawColor.G = (byte)MathHelper.Lerp(EleHorz[j].DrawColor.G, 0, 0.0285f);
                EleHorz[j].DrawColor.B = (byte)MathHelper.Lerp(EleHorz[j].DrawColor.B, 0, 0.0285f);
                EleHorz[j].DrawColor.A = (byte)MathHelper.Lerp(EleHorz[j].DrawColor.A, 0, 0.0285f);
            }

        }
        EleHorz.RemoveAll((ScreenElements c) => c.Lifetime < 0);

        ///////////////////////////////////////////////////////////
        for (int j = 0; j < EleWave.Count; j++)//波纹
        {
            EleWave[j].Time++;
            EleWave[j].Lifetime--;
            int t = EleWave[j].Time;
            EleWave[j].Center += EleWave[j].Velocity;
            //Scale用于传参，大的更亮    

            if (EleWave[j].Time < 32)
                if (EleWave[j].Scale < 2)
                    EleWave[j].DrawColor = new Color(t * 2, t, 8, 140);
                else
                    EleWave[j].DrawColor = new Color(t * 4, t * 3, 24, 160);
            

            if (280 - EleWave[j].Time < 35)
            {
                EleWave[j].DrawColor.R = (byte)MathHelper.Lerp(EleWave[j].DrawColor.R, 0, 0.0285f);
                EleWave[j].DrawColor.G = (byte)MathHelper.Lerp(EleWave[j].DrawColor.G, 0, 0.0285f);
                EleWave[j].DrawColor.B = (byte)MathHelper.Lerp(EleWave[j].DrawColor.B, 0, 0.0285f);
                EleWave[j].DrawColor.A = (byte)MathHelper.Lerp(EleWave[j].DrawColor.A, 0, 0.0285f);
            }
        }
        EleWave.RemoveAll((ScreenElements c) => c.Lifetime < 0);

        ///////////////////////////////////////////////////////////
        for (int j = 0; j < EleGraydust.Count; j++)//灰尘
        {
            EleGraydust[j].Time++;
            EleGraydust[j].Center += EleGraydust[j].Velocity;

            if (EleGraydust[j].Time < 32)
                EleGraydust[j].DrawColor.A = (byte)MathHelper.Lerp(EleGraydust[j].DrawColor.A, EleGraydust[j].Lifetime - 290, 0.035f);
            if (EleGraydust[j].Lifetime - EleGraydust[j].Time < 35)
                EleGraydust[j].DrawColor.A = (byte)MathHelper.Lerp(EleGraydust[j].DrawColor.A, 0, 0.0285f);
        }
        EleGraydust.RemoveAll((ScreenElements c) => c.Time >= c.Lifetime);

        ///////////////////////////////////////////////////////////
        for (int j = 0; j < EleCrow.Count; j++)//乌鸦
        {
            EleCrow[j].Time++;
            EleCrow[j].Center += EleCrow[j].Velocity;
            if (EleCrow[j].Time < 11)
                EleCrow[j].DrawColor.A = (byte)MathHelper.Lerp(EleCrow[j].DrawColor.A, 200 + (int)(EleCrow[j].Scale * 38), 0.12f);
        }
        EleCrow.RemoveAll((ScreenElements c) => c.Time >= c.Lifetime);

        /////////////////////////////////////////////////////////
        for (int j = 0; j < EleBird.Count; j++)//鸟
        {
            EleBird[j].Time++;
            float t = EleBird[j].Time * 0.0075f;
            EleBird[j].Center = birdPos + new Vector2((float)Math.Sin(2 * t) / 2f, -(float)Math.Cos(3 * t) / 3f) * 36f + new Vector2(15, 15) * j;
        }

        //////////////////////////////////////////////////////////
        for (int j = 0; j < EleSmallBird.Count; j++)//小苍蝇鸟
        {
            EleSmallBird[j].Time++;
            float t = EleSmallBird[j].Time * 0.0075f;
            if (j < 4)
                EleSmallBird[j].Center = smbirdPos2 + new Vector2((float)Math.Sin(2 * t) / 2f, -(float)Math.Cos(3 * t) / 3f) * 36f + EleSmallBird[j].Velocity * j * new Vector2(14, 14);
            else
                EleSmallBird[j].Center = smbirdPos1 + new Vector2((float)Math.Sin(2 * t) / 2f, -(float)Math.Cos(3 * t) / 3f) * 36f + EleSmallBird[j].Velocity * j * new Vector2(4, 5);

        }
        //////////////////////////////////////////////////////////







        //绘制部分
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        for (int k = 0; k < EleBird.Count; k++)//鸟绘制
        {
            Vector2 drawPosition = EleBird[k].Center;
            Rectangle framecut = birdTex.Frame(verticalFrames: 5, frameY: EleBird[k].Time / 5 % 5);
            spriteBatch.Draw(birdTex, drawPosition, framecut, EleBird[k].DrawColor, 0f, birdTex.Size() / new Vector2(1, 5) * 0.5f, 1.2f, 0, 0f);
        }

        //////////////////////////////////////////////////////////
        for (int k = 0; k < EleSmallBird.Count; k++)//小苍蝇鸟绘制
        {
            Vector2 drawPosition = EleSmallBird[k].Center;
            Rectangle framecut = smallBirdTex.Frame(verticalFrames: 4, frameY: EleSmallBird[k].Time / 5 % 4);
            spriteBatch.Draw(smallBirdTex, drawPosition, framecut, EleSmallBird[k].DrawColor, 0f, smallBirdTex.Size() / new Vector2(1, 4) * 0.5f, EleSmallBird[k].Scale, 0, 0f);
        }

        //////////////////////////////////////////////////////////
        Vector2 pos = logoDrawCenter + new Vector2(0, 625f);
        for (int i = 0; i < 45; i++)//绘制丁达尔光
        {
            spriteBatch.Draw(whitedot, pos + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-650, -180)), new Rectangle(0, 0, 1, 1), new(2, 2, 2, 2), -MathHelper.Pi / 9f, new(0.25f, 0.5f), new Vector2(2000f, 5f), SpriteEffects.None, 0);
            //new(1, 1, 1, 1)
        }

        //////////////////////////////////////////////////////////
        //Vector2 boatPos = drawOffset + new Vector2(texture.Width / 2, 1035) * scale + new Vector2(45, 0);

        //spriteBatch.Draw(boat, boatPos, null, new(255, 255, 255, 114),//倒影
        //    -0.035f * (float)Math.Sin(Math.PI /120 *boatTime), boat.Size() * new Vector2(0.5f, 0f),  scale, SpriteEffects.FlipVertically, 0f);
        //spriteBatch.Draw(boat, boatPos, null, new(255, 255, 255, 255),
        //    0.035f * (float)Math.Sin(Math.PI / 120 * boatTime), boat.Size() * new Vector2(0.5f, 1f), scale, 0, 0f);

        boatTime++;//船正弦函数计时器
        //船倒影绘制
        spriteBatch.Draw(boat, new Rectangle((int)(boatPos.X - boat.Size().X / 2 * (Math.Sin(Math.PI / 270 * boatTime) * 0.25 + 2) / 2),
                                                                       (int)boatPos.Y,
                                                                        (int)(boat.Size().X * (Math.Sin(Math.PI / 270 * boatTime) * 0.25 + 2) / 2),
                                                                        (int)boat.Size().Y),
           new Rectangle(0, 0, (int)boat.Size().X, (int)boat.Size().Y), new(255, 255, 255, 114), -0.018f * (float)Math.Sin(Math.PI / 140 * boatTime), Vector2.Zero, SpriteEffects.FlipVertically, 0);
        
        //////////////////////////////////////////////////////////
        for (int k = 0; k < EleGraydust.Count; k++)//灰尘绘制
        {
            Vector2 drawPosition = EleGraydust[k].Center;
            spriteBatch.Draw(grayDust, drawPosition, null, EleGraydust[k].DrawColor, 0f, grayDust.Size() * 0.5f, EleGraydust[k].Scale, 0, 0f);
        }

        //////////////////////////////////////////////////////////
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate,
            BlendState.Additive,
            SamplerState.LinearWrap,
            DepthStencilState.None,
            RasterizerState.CullNone,
            null,
            Main.UIScaleMatrix);


        for (int k = 0; k < EleFog.Count; k++)//雾气绘制
        {
            Vector2 drawPosition = EleFog[k].Center;
            spriteBatch.Draw(fog, drawPosition, null, EleFog[k].DrawColor, 0f, fog.Size() * new Vector2(1f, 0.5f), EleFog[k].Scale, 0, 0f);
            //spriteBatch.Draw(fog, drawPosition + new Vector2(-35, 0), null, EleFog[k].DrawColor, 0f, fog.Size() * new Vector2(1f, 0.5f), EleFog[k].Scale, 0, 0f);

        }

        for (int k = 0; k < EleWave.Count; k++)//波纹绘制
        {
            Vector2 drawPosition = EleWave[k].Center;
            Rectangle rectangle;
            if (EleWave[k].Scale < 2)
                rectangle = new Rectangle((int)EleWave[k].Center.X, (int)EleWave[k].Center.Y, EleWave[k].Lifetime / 3, 4);
            else//大的
                rectangle = new Rectangle((int)EleWave[k].Center.X, (int)EleWave[k].Center.Y, EleWave[k].Lifetime / 2 + 27, 4);

            spriteBatch.Draw(ChooseTex(EleWave[k].IdentityIndex % 3, wave1, wave2, wave3), rectangle, new(0, 0, 23, 4), EleWave[k].DrawColor, 0f, wave2.Size() * 0.5f, 0, 0f);
        }

        for (int k = 0; k < EleDustLens.Count; k++)//光晕绘制
        {
            Vector2 drawPosition = EleDustLens[k].Center;
            spriteBatch.Draw(ChooseTex(EleDustLens[k].IdentityIndex % 3, dustLensBig, dustLensFG, dustLensSmall), drawPosition, null, EleDustLens[k].DrawColor, 0f, Vector2.Zero, EleDustLens[k].Scale, 0, 0f);
        }

        for (int k = 0; k < EleHorz.Count; k++)//地平线绘制
        {
            Vector2 drawPosition = EleHorz[k].Center;
            Rectangle rectangle = new Rectangle((int)EleHorz[k].Center.X, (int)EleHorz[k].Center.Y, EleHorz[k].Lifetime, 4);
            spriteBatch.Draw(ChooseTex(EleHorz[k].IdentityIndex % 3, wave1, wave2, wave3), drawPosition, null, EleHorz[k].DrawColor, 0f, wave2.Size() * 0.5f, EleHorz[k].Scale, 0, 0f);
            spriteBatch.Draw(ChooseTex(EleHorz[k].IdentityIndex % 3, wave1, wave2, wave3), rectangle, new(0, 0, 23, 4), EleHorz[k].DrawColor, 0f, wave2.Size() * 0.5f, SpriteEffects.None, 0f);
        }

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
        
        /////////////////////////////////////////////////////////
        //船绘制
        spriteBatch.Draw(boat, new Rectangle((int)(boatPos.X - boat.Size().X / 2 * (Math.Sin(Math.PI / 270 * boatTime) * 0.25 + 2) / 2),
                                                            (int)(boatPos.Y - boat.Size().Y),
                                                             (int)(boat.Size().X * (Math.Sin(Math.PI / 270 * boatTime) * 0.25 + 2) / 2),
                                                             (int)boat.Size().Y),
            new Rectangle(0, 0, (int)boat.Size().X, (int)boat.Size().Y), Color.White, 0.018f * (float)Math.Sin(Math.PI / 140 * boatTime), Vector2.Zero, SpriteEffects.None, 0);

        //////////////////////////////////////////////////////////
        for (int k = 0; k < EleCrow.Count; k++)//乌鸦绘制
        {
            Vector2 drawPosition = EleCrow[k].Center;
            Rectangle framecut = crowTex.Frame(verticalFrames: 5, frameY: EleCrow[k].Time / 3 % 5);
            int flip = (Math.Sign(EleCrow[k].Velocity.X) - 1) / 2;
            spriteBatch.Draw(crowTex, drawPosition, framecut, EleCrow[k].DrawColor, 0f, crowTex.Size() / new Vector2(1, 5) * 0.5f, EleCrow[k].Scale, (SpriteEffects)flip, 0f);
        }
        if (CrowSpawnCoolDown > 0)//计时器
            CrowSpawnCoolDown--;


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //绘制顺序：背景 鸟 苍蝇鸟 丁达尔光 船倒影  雾气 灰尘 波纹 光晕 地平线  船 乌鸦 
        
        return true;
    }
    public override void PostDrawLogo(SpriteBatch spriteBatch, Vector2 logoDrawCenter, float logoRotation, float logoScale, Color drawColor)
    {
        base.PostDrawLogo(spriteBatch, logoDrawCenter, logoRotation, logoScale, drawColor);
    }


    public override void OnSelected()
    {
        SoundStyle selected = new($"{nameof(DeadCells)}/Assets/Sounds/menu_select");
        SoundEngine.PlaySound(selected);
    }

    public static void ClearAll()
    {
        EleSmallBird.Clear();
        EleGraydust.Clear();
        EleCrow.Clear();
        EleBird.Clear();
        EleDustLens.Clear();
        EleFog.Clear();
        EleHorz.Clear();
        EleWave.Clear();
        fogtime = 2560;
        fogtime2 = 3200;
    }

    public static Texture2D ChooseTex(int num, Texture2D tex1, Texture2D tex2, Texture2D tex3)
    {
        return num switch
        {
            0 => tex1,
            1 => tex2,
            2 => tex3,
            _ => tex1,
        };
    }
}

public class ScreenElements
{
    public ScreenElements(int lifetime, int identity, Color color, Vector2 startingPosition, Vector2 startingVelocity, float scale = 1f, int time = 0)
    {
        Lifetime = lifetime;
        IdentityIndex = identity;
        DrawColor = color;
        Center = startingPosition;
        Velocity = startingVelocity;
        Scale = scale;
        Time = time;
    }
    public int Time;

    public int Lifetime;

    public int IdentityIndex;

    public float Scale;

    public Color DrawColor;

    public Vector2 Velocity;

    public Vector2 Center;

}
