using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DeadCells.Common.Buffs;
using DeadCells.Projectiles;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Audio;
using DeadCells.Core;
using DeadCells.Projectiles.EffectProj;
using DeadCells.Tiles;

namespace DeadCells.Common.Players;
//当玩家使用技能时，在这里写各种特效的使用。
public class PlayerDraw : ModPlayer
{

    public PlayerHurt CurseCheck => Player.GetModPlayer<PlayerHurt>();
    public Vector2 PlayerPos => Player.Center - Main.screenPosition;

    public bool draw_lacerating_rings = false;
    public bool draw_knife_dance = false;
    public bool can_teleport = false;
    public int teleport_charge = 0;
    public int teleport_stun = 0;
    public int teleport_floattime = 0;
    public bool teleporting;
    public TeleportPylonInfo destinationPylonInfo;

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        drawInfo.drawPlayer.Yoraiz0rEye();
    }

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (CurseCheck.CurseNum > 0)//绘制诅咒
        {
            string Num = CurseCheck.CurseNum.ToString();
            int offset = Num.Length * 8;
            Texture2D texture = ModContent.Request<Texture2D>("DeadCells/Assets/BuffIcons/CurseAffect", (AssetRequestMode)2).Value;
            var font = FontAssets.MouseText.Value;
            Main.spriteBatch.Draw(texture, PlayerPos + new Vector2(4, -72f), Color.White);
            Main.spriteBatch.DrawString(font, Num, PlayerPos + new Vector2(-12 - offset, -70f), Color.White, 0f, default, 1.3f, SpriteEffects.None, default);
        }

        if (Player.HasBuff(ModContent.BuffType<Stun>()))//绘制眩晕Buff
        {
            Texture2D texture = ModContent.Request<Texture2D>("DeadCells/Assets/BuffIcons/StunAffect", (AssetRequestMode)2).Value;
            Rectangle framecut = texture.Frame(verticalFrames: 4, frameY: (int)Main.GameUpdateCount / 6 % 4);
            //4是帧数，6是播放速度

            Main.spriteBatch.Draw(texture,
            PlayerPos + new Vector2(0, -34f),
            framecut,
            Color.White,
            default,
            texture.Size() / new Vector2(1, 4) * 0.5f,
            1f,
            SpriteEffects.None,
            0f);
        }

}




    public override void ResetEffects()
    {
        if (draw_lacerating_rings)//杀戮光环
        {
            IEntitySource source = Player.GetSource_FromAI();
            int m = Main.rand.Next(2, 5);
            for (double r = 0f; r < MathHelper.TwoPi; r += MathHelper.TwoPi / (float)(5 + m))//绘制外圆十字形状（闪电+白线）
            {
                float k = Main.rand.NextFloat(-0.4f, 0.4f);
                Vector2 newposition = new Vector2((float)Math.Cos(r + k), (float)Math.Sin(r + k)) * 146f;
                Projectile.NewProjectileDirect(source, newposition, Vector2.Zero, ModContent.ProjectileType<Lightning>(), 0, 0f, Main.myPlayer, 2f, 0);
                Projectile.NewProjectileDirect(source, newposition, Vector2.Zero, ModContent.ProjectileType<LineWhiteSharp>(), 0, 0f, Main.myPlayer, 2f);
            };
            for (int i = 0; i < 6; i++)//外圆白线
            {
                Vector2 lineposition = new Vector2((float)Math.Cos(Main.rand.NextFloat(0f, MathHelper.TwoPi)), (float)Math.Sin(Main.rand.NextFloat(0f, MathHelper.TwoPi))) * 146f;
                Projectile.NewProjectileDirect(source, lineposition, Vector2.Zero, ModContent.ProjectileType<LineWhiteSharp>(), 0, 0f, Main.myPlayer, 2f);
            }
            for (int i = 0; i < 7; i++)//椭圆周围小一点的白线
            {
                float a = Main.rand.NextFloat(35f, 50f);
                Vector2 innerposition = new Vector2((float)Math.Cos(Main.rand.NextFloat(0f, MathHelper.TwoPi)), (float)Math.Sin(Main.rand.NextFloat(0f, MathHelper.TwoPi))) * a;
                Projectile.NewProjectileDirect(source, innerposition, Vector2.Zero, ModContent.ProjectileType<LineWhiteSharp>(), 0, 0f, Main.myPlayer, 1.1f, a);
            }
        }

        if (draw_knife_dance)//圆舞飞刀
        {
            IEntitySource source = Player.GetSource_FromAI();
            for (int i = 0; i < 30; i++)//玩家身上炸开的红线
            {
                float a = Main.rand.NextFloat(10f, 14f);
                Vector2 position = new Vector2((float)Math.Cos(Main.rand.NextFloat(0f, MathHelper.TwoPi)), (float)Math.Sin(Main.rand.NextFloat(0f, MathHelper.TwoPi))) * 1f;
                Vector2 vel = new Vector2((float)Math.Cos(Main.rand.NextFloat(0f, MathHelper.TwoPi)), (float)Math.Sin(Main.rand.NextFloat(0f, MathHelper.TwoPi))) * (a);

                Projectile.NewProjectileDirect(source, position, vel, ModContent.ProjectileType<LineRed>(), 0, 0f, Main.myPlayer, 1.1f, 1f);
            }
            draw_knife_dance = false;
        }
    }

    public override void PreUpdate()
    {
        Point16 playerCenter = Player.Center.ToTileCoordinates16();
        can_teleport = Main.tile[playerCenter.X, playerCenter.Y].TileType == ModContent.TileType<TeleportAltar>()
            && Main.tile[playerCenter.X, playerCenter.Y].TileFrameY < 72
            && Main.tile[playerCenter.X, playerCenter.Y].TileFrameX > 0
            && Main.tile[playerCenter.X, playerCenter.Y].TileFrameX < 72;
        

        //foreach (Point16 point in unlockedTeleportStation)
        //Main.NewText(point);
        //unlockedTeleportStation.Clear();
    }

    public override void PostUpdate()
    {
        if (teleport_charge > 0)//传送前
        {
            teleporting = true;
            teleport_charge--;
        }
        if (teleport_stun > 0)
        {
            teleporting = true;
            teleport_stun--;
        }
        if (teleport_charge == 12)//生成原神启动效果
            Projectile.NewProjectile(Player.GetSource_FromAI(), Main.screenPosition, Vector2.Zero, ModContent.ProjectileType<TeleportWhiteScreen>(), 0, 0);

        if (teleport_charge == 1)//结束时传送
        {
            Main.PylonSystem.RequestTeleportation(destinationPylonInfo, Main.LocalPlayer);
            SoundEngine.PlaySound(AssetsLoader.portal_use2);
            teleport_stun = 36;//传送后摇
        }

        if (teleport_stun == 8)
        {
            Vector2 pos = destinationPylonInfo.PositionInTiles.ToWorldCoordinates() + new Vector2(30, 24);
            //一圈粒子
            Projectile.NewProjectile(Player.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<TeleportAfter>(), 0, 0);
        }
        if (teleport_stun == 0 && teleport_charge == 0)
        {
            teleporting = false;
            teleport_floattime = 0;
        }

        if (teleporting)//传送时禁止移动
        {
            LockMovement();
        }
        if (teleporting && teleport_stun < 1 && teleport_floattime < 24)//传送时漂浮
        {
            Player.velocity.Y = MathHelper.Lerp(-3.7f, 0, 1f / 24 * teleport_floattime);
            teleport_floattime++;
        }

        if (teleport_floattime >= 24)
        {
            Player.velocity *= Vector2.Zero;
        }
    }
    public override void SetControls()//传送时禁止移动
    {
        if (teleporting)
        {
            Player.controlInv = false;
            Player.controlMap = false;
            Player.controlMount = false;
            Player.controlSmart = false;
            Player.controlTorch = false;
            Player.controlHook = false;
            Player.controlJump = false;
            Player.controlDown = false;
            Player.controlLeft = false;
            Player.controlRight = false;
            Player.controlUp = false;
            Player.controlUseItem = false;
            Player.controlUseTile = false;
            Player.controlThrow = false;
        }
    }
    public void LockMovement()
    {

        Player.gravity = 0f;
        Player.wingTime = 0;
        Player.rocketTime = 0;
        Player.sandStorm = false;
        Player.dash = 0;
        Player.dashType = 0;
        Player.noKnockback = true;
        Player.RemoveAllGrapplingHooks();
        Player.StopVanityActions();

        if (Player.mount.Active)
            Player.mount.Dismount(Player);

        if (Player.pulley)
            Player.pulley = false;

        Player.gravDir = 1f;
    }



}