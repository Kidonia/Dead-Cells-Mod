using DeadCells.Common;
using DeadCells.Common.Players;
using DeadCells.Core;
using DeadCells.Items;
using DeadCells.Projectiles;
using DeadCells.Projectiles.EffectProj;
using DeadCells.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;


namespace DeadCells
{
    public class DeadCells : Mod
	{
        private static DeadCells m_instance;
        public static DeadCells Instance => m_instance;
        public DeadCells()
        {
            m_instance = this;
        }

        internal CellNumUI CLUI;//加载细胞，连杀，金币UI
        internal EquipsAndScrollNumUI SNUI;//加载卷轴UI
		internal UserInterface UC;

        public static RenderTarget2D render;
        public float twistStrength = 0f;

        public override void Load()
		{
            AssetsLoader.LoadAsset();
            AssetsLoader.LoadSound();

            CLUI = new CellNumUI();
            SNUI = new EquipsAndScrollNumUI();
            UC = new UserInterface();
			CLUI.Activate();
            SNUI.Activate();
			UC.SetState(CLUI);
            UC.SetState(SNUI);

            //On_Player.TryTogglingShield
            On_Player.GetItem_FillIntoOccupiedSlot += CellGrabSoundOccupiedSlot;
            On_Player.QuickGrapple += TeleportStunEffect;
            On_Player.SlopingCollision += NoFallThroughPlatform;

            On_FilterManager.EndCapture += FilterManager_EndCapture;//原版绘制场景的最后部分——滤镜。在这里运用render保证不会与原版冲突
            On_TeleportPylonsSystem.SpawnInWorldDust += RemoveDust;
            //On_Main.DrawProj += DrawHeadLine;
            On_Main.TeleportEffect += RemoveTeleportAltarSound;
            Main.OnResolutionChanged += (obj) =>
            {
                render = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
                DCMenu.ClearAll();
            };
            base.Load();
        }
        private void NoFallThroughPlatform(On_Player.orig_SlopingCollision orig, Player self, bool fallThrough, bool ignorePlats)
        {
            if (self.TryGetModPlayer<ShootHead>(out var player) && !player.canplayermove)
            {
                //Main.NewText("fallThrough");
                return;
            }
            orig(self, fallThrough, ignorePlats);

        }

        /*
        private void DrawHeadLine(On_Main.orig_DrawProj orig, Main self, int i)
        {
            float polePositonX = 0f;
            float polePositonY = 0f;
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.type == ModContent.ProjectileType<Homunculus>() && projectile.active)
                    DrawProj_HeadLine(projectile, ref polePositonX, ref polePositonY, Main.player[projectile.owner].MountedCenter);
            }
            orig(self, i);
        }
        //////////////////////////////////////////////飞头部分/////////////////////////////////////
        private static void DrawProj_HeadLine(Projectile proj, ref float polePosX, ref float polePosY, Vector2 mountedCenter)
        {
            Player player = Main.player[proj.owner];
            polePosX = player.MountedCenter.X;
            polePosY = mountedCenter.Y;
            polePosY += player.gfxOffY;
            if (player.mount.Active && player.mount.Type == 52)
            {
                polePosX -= player.direction * 14;
                polePosY -= -10f;
            }
            float gravDir = Main.player[proj.owner].gravDir;


            polePosX += -4f * Main.player[proj.owner].direction - 10f;
            if (Main.player[proj.owner].direction < 0)
            {
                polePosX -= 13f;
            }
            polePosY -= 36f * gravDir;


            if (gravDir == -1f)
            {
                polePosY -= 12f;
            }

            Vector2 vector = new(polePosX, polePosY); //当前头部顶点的坐标
            vector = Main.player[proj.owner].RotatedRelativePoint(vector + new Vector2(8f)) - new Vector2(8f); //头部顶点根据人物旋转略作调整
            float Xdis = proj.position.X + proj.width * 0.5f - vector.X; //弹幕与人头的水平距离
            float Ydis = proj.position.Y + proj.height * 0.5f - vector.Y; //弹幕与人头的垂直距离

            bool flag = true;
            if (Xdis == 0f && Ydis == 0f) //如果 弹幕来到头的位置
            {
                flag = false;
            }
            else
            {
                float num4 = (float)Math.Sqrt(Xdis * Xdis + Ydis * Ydis); //弹幕到人头的距离
                num4 = 12f / num4; //50除以距离 无影响？
                Xdis *= num4; //弹幕与人头的水平距离 乘上 该
                Ydis *= num4;
                vector.X -= Xdis;
                vector.Y -= Ydis;
                Xdis = proj.position.X + proj.width * 0.5f - vector.X - 20f;
                Ydis = proj.position.Y + proj.height * 0.5f - vector.Y - 20f;
            }
            while (flag)
            {
                float num5 = 15f;// 点的密集程度，越大越密集
                float num6 = (float)Math.Sqrt(Xdis * Xdis + Ydis * Ydis);
                float num7 = num6;
                if (float.IsNaN(num6) || float.IsNaN(num7))
                {
                    flag = false;
                    continue;
                }
                if (num6 < 20f)
                {
                    num5 = num6 - 8f;
                    flag = false;
                }
                num6 = 16f / num6;// 两点间的距离，前面分子越大越分散
                Xdis *= num6;
                Ydis *= num6;
                vector.X += Xdis;
                vector.Y += Ydis;
                Xdis = proj.position.X + proj.width * 0.5f - vector.X - 26f; //终点位置
                Ydis = proj.position.Y + proj.height * 0.1f - vector.Y - 18f; //终点位置
                if (num7 > 12f)
                {
                    float num8 = 0.3f;
                    float num9 = Math.Abs(proj.velocity.X) + Math.Abs(proj.velocity.Y);
                    if (num9 > 16f)
                    {
                        num9 = 16f;
                    }
                    num9 = 1f - num9 / 16f;
                    num8 *= num9;
                    num9 = num7 / 80f;
                    if (num9 > 1f)
                    {
                        num9 = 1f;
                    }
                    num8 *= num9;
                    if (num8 < 0f)
                    {
                        num8 = 0f;
                    }
                    num9 = 1f - proj.localAI[0] / 100f;
                    num8 *= num9;
                    if (Ydis > 0f)
                    {
                        Ydis *= 1.2f + num8;//曲率
                        Xdis *= 1.2f - num8;
                    }
                    else
                    {
                        num9 = Math.Abs(proj.velocity.X) / 3f;
                        if (num9 > 1f)
                        {
                            num9 = 1f;
                        }
                        num9 -= 0.5f;
                        num8 *= num9;
                        if (num8 > 0f)
                        {
                            num8 *= 2f;
                        }
                        Ydis *= 1.2f + num8;
                        Xdis *= 1.2f - num8;
                    }
                }

                for (int i = -2; i <= 2; i += 4)
                {
                    float rot2 = (float)Math.Atan2(Ydis + i +1, Xdis + i) - 1.57f;
                    Main.EntitySpriteDraw(AssetsLoader.Headline,
                        new Vector2(vector.X - Main.screenPosition.X + AssetsLoader.Headline.Width * 0.5f + i,
                                             vector.Y - Main.screenPosition.Y + AssetsLoader.Headline.Height * 0.5f + i + 1),
                        new Rectangle(0, 0, AssetsLoader.Headline.Width, (int)num5),
                        Color.White,
                        rot2,
                        new Vector2(AssetsLoader.Headline.Width * 0.5f, 0f),
                        1f,
                        SpriteEffects.None);
                }
                float rot = (float)Math.Atan2(Ydis + 3, Xdis) - 1.57f;
                Main.EntitySpriteDraw(AssetsLoader.Headline,
                    new Vector2(vector.X - Main.screenPosition.X + AssetsLoader.Headline.Width * 0.5f,
                                         vector.Y - Main.screenPosition.Y + AssetsLoader.Headline.Height * 0.5f + 3),
                    new Rectangle(0, 0, AssetsLoader.Headline.Width, (int)num5),
                    Color.White,
                    rot,
                    new Vector2(AssetsLoader.Headline.Width * 0.5f, 0f),
                    1f,
                    SpriteEffects.None);
            }
        }
        */

        private void TeleportStunEffect(On_Player.orig_QuickGrapple orig, Player self)
        {
            if (self.TryGetModPlayer<PlayerDraw>(out var player) && player.teleporting)
                return;
            orig(self);
        }
        private void RemoveTeleportAltarSound(On_Main.orig_TeleportEffect orig, Rectangle effectRect, int Style, int extraInfo, float dustCountMult, TeleportationSide side, Vector2 otherPosition)
        {
            if (Style == 9 && extraInfo == 9)//前面是传送方式，后面是传送塔类型
            {
                effectRect.Inflate(15, 15);
            }
            else
                orig(effectRect, Style, extraInfo, dustCountMult, side, otherPosition);
        }
        private void RemoveDust(On_TeleportPylonsSystem.orig_SpawnInWorldDust orig, int tileStyle, Rectangle dustBox)
        {
            if (tileStyle == 9)
                return;
            else
                orig(tileStyle, dustBox);
        }
        private bool CellGrabSoundOccupiedSlot(On_Player.orig_GetItem_FillIntoOccupiedSlot orig, Player self, int plr, Item newItem, GetItemSettings settings, Item returnItem, int i)
        {
            if (newItem.type == ModContent.ItemType<Cell>())
            {
                SoundEngine.PlaySound(AssetsLoader.absorb, self.Center);
                return true;
            }
            return orig(self, plr, newItem, settings, returnItem, i);
        }


        private void Main_OnResolutionChanged(Vector2 obj)//在分辨率更改时，重建render防止某些bug
        {
            CreateRender();
        }
        private void FilterManager_EndCapture(On_FilterManager.orig_EndCapture orig, FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {

            render ??= new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            SpriteBatch sb = Main.spriteBatch;

            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);//用透明清除
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();


            gd.SetRenderTarget(render);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);

            foreach (Projectile proj in Main.projectile)
            {
                if (proj.type == ModContent.ProjectileType<RoundTwist>() && proj.active)
                {
                    Texture2D texture = ModContent.Request<Texture2D>("DeadCells/Effects/imgtwist", (AssetRequestMode)1).Value;
                    sb.Draw(texture, proj.position - Main.screenPosition, null, Color.White, 0, texture.Size() / 2, proj.ai[0], SpriteEffects.None, 0);
                    twistStrength = 0.012f;
                }
                if (proj.type == ModContent.ProjectileType<RoundTwist2>() && proj.active)
                {
                    Texture2D texture = ModContent.Request<Texture2D>("DeadCells/Effects/imgtwist2", (AssetRequestMode)1).Value;
                    sb.Draw(texture, proj.position - Main.screenPosition, null, Color.White, 0, texture.Size() / 2, proj.ai[0], SpriteEffects.None, 0);
                    twistStrength = 0.012f;
                }
                if (proj.type == ModContent.ProjectileType<QueenRapierCritCut>() && proj.active)
                {
                    Texture2D texture = ModContent.Request<Texture2D>(AssetsLoader.WhiteDotImg, (AssetRequestMode)1).Value;
                    sb.Draw(texture, proj.Center + (proj.Left - proj.Center).RotatedBy(proj.rotation) - Main.screenPosition, new(0, 0, 1, 1), new(GetCorrectRadian(proj.rotation), proj.ai[0], 0f), proj.rotation, Vector2.Zero, new Vector2(400, 400), SpriteEffects.None, 0);
                    sb.Draw(texture, proj.Center + (proj.Left - proj.Center).RotatedBy(proj.rotation) - Main.screenPosition, new(0, 0, 1, 1), new(GetCorrectRadian(proj.rotation) + Math.Sign(proj.rotation + 0.001f) * 0.5f, proj.ai[0], 0f), proj.rotation, new(0, 1), new Vector2(400, 400), SpriteEffects.None, 0);
                    twistStrength = 0.004f;
                }

            }
            sb.End();


            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            AssetsLoader.offsetEffect.Parameters["tex0"].SetValue(render);
            AssetsLoader.offsetEffect.Parameters["i"].SetValue(twistStrength);
            AssetsLoader.offsetEffect.CurrentTechnique.Passes[0].Apply();
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            foreach (Projectile proj in Main.projectile)
            {
                if(proj.type == ModContent.ProjectileType<TeleportScreenScale>() && proj.active)
                {
                    Vector2 middle = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
                    AssetsLoader.screenColorMess.Parameters["offsetStrength"].SetValue(proj.ai[1]);
                    AssetsLoader.screenColorMess.CurrentTechnique.Passes[0].Apply();
                    sb.Draw(Main.screenTargetSwap, middle, null, Color.White, 0, middle, proj.ai[0], SpriteEffects.None, 0);
                }

                if (proj.type == ModContent.ProjectileType<TeleportWhiteScreen>() && proj.active)
                {
                    Texture2D texture = ModContent.Request<Texture2D>(AssetsLoader.WhiteDotImg, (AssetRequestMode)1).Value;
                    sb.End();
                    sb.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                    sb.Draw(texture, Vector2.Zero, null, new(255, 255, 255, (int)proj.ai[0]), 0, Vector2.Zero, new Vector2(Main.screenWidth, Main.screenHeight), SpriteEffects.None, 0);
                }

            }
            //sb.Draw(Main.screenTargetSwap, Vector2.Zero + new Vector2(200, 200), Color.White);

            sb.End();

            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }
        public static void CreateRender()
        {
            render = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            DCMenu.ClearAll();
        }

        private float GetCorrectRadian(float minusRadian)
        {
            if (minusRadian < 0)
            {
                return (MathHelper.TwoPi + minusRadian) / MathHelper.TwoPi;
            }
            else
                return minusRadian / MathHelper.TwoPi;
        }
        public override void Unload()
        {
            m_instance = null;
            On_FilterManager.EndCapture -= FilterManager_EndCapture;
            On_Player.GetItem_FillIntoOccupiedSlot -= CellGrabSoundOccupiedSlot;
            On_Player.QuickGrapple -= TeleportStunEffect;
            On_Player.SlopingCollision -= NoFallThroughPlatform;

            On_TeleportPylonsSystem.SpawnInWorldDust -= RemoveDust;
            On_Main.TeleportEffect -= RemoveTeleportAltarSound;
            //On_Main.DrawProj -= DrawHeadLine;
            Main.OnResolutionChanged -= Main_OnResolutionChanged;
            AssetsLoader.UnloadAsset();
            base.Unload();
        }

    }
}