using DeadCells.Common.Players;
using DeadCells.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using DeadCells.Utils;
using System.Collections.Generic;

namespace DeadCells.Projectiles;

public class Homunculus : ModProjectile
{
    private int timeCheckAir = 0;
    private bool isInAir = false;
    private float doublejumplimit = 1.6f;
    private int soundplayCooldown = 8;
    private bool isColliding;
    private bool keepSliding;
    private int oldSlideDir = 0;
    public List<Rope_Point> rp1 = new List<Rope_Point>();
    public List<Rope_Line> rl1 = new List<Rope_Line>();
    public List<Rope_Point> rp2 = new List<Rope_Point>();
    public List<Rope_Line> rl2 = new List<Rope_Line>();
    public List<Rope_Point> rp3 = new List<Rope_Point>();
    public List<Rope_Line> rl3 = new List<Rope_Line>();
    public Player player => Main.player[Projectile.owner];
    public ShootHead homoplayer => player.GetModPlayer<ShootHead>();
    private Vector2 HeadPosition => player.Top + new Vector2(2, 11);
    public override string Texture => "DeadCells/Assets/HeadFlesh";
    public override void SetDefaults()
    {
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 14;
        Projectile.width = 14;
        Projectile.height = 14;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        //Projectile.restrikeDelay = 8;
        Projectile.netImportant = true;
        Projectile.timeLeft = 36000;
    }
    public override void SetStaticDefaults()
    {
        //没用
        ProjectileID.Sets.FallingBlockDoesNotFallThroughPlatforms[Type] = true;
    }
    public override void OnSpawn(IEntitySource source)
    {
        for (int i = 0; i < 7; i++)//7个差不多
        {
            RopePhysicalEffects.AddVerletObj(rp1, rl1, Projectile.Left);
            RopePhysicalEffects.AddVerletObj(rp2, rl2, Projectile.Top);
            RopePhysicalEffects.AddVerletObj(rp3, rl3, Projectile.BottomRight);

        }

    }
    public override bool PreDraw(ref Color lightColor)
    {
        //为何不写在AI里更新头线位置？因为有延迟！draw一个更新执行两次
        RopePhysicalEffects.VerletObjPosiUpdate(rp1, rl1, Projectile.Left, HeadPosition);
        RopePhysicalEffects.VerletObjPosiUpdate(rp2, rl2, Projectile.Top, HeadPosition);
        RopePhysicalEffects.VerletObjPosiUpdate(rp3, rl3, Projectile.BottomRight, HeadPosition);
        RopePhysicalEffects.DrawVerletObj(rl1, AssetsLoader.Headline);
        RopePhysicalEffects.DrawVerletObj(rl2, AssetsLoader.Headline);
        RopePhysicalEffects.DrawVerletObj(rl3, AssetsLoader.Headline);

        Texture2D texture = AssetsLoader.HeadFlesh;
        Texture2D texture2 = AssetsLoader.HeadFlesh2;
        Texture2D texture3 = AssetsLoader.HeadFlesh3;


        float globalTimeWrappedHourly = Main.GlobalTimeWrappedHourly;
        globalTimeWrappedHourly %= 4f;
        globalTimeWrappedHourly /= 2f;

        Vector2 vector = new(8, 8);
        Vector2 vector3 = Projectile.Center - Main.screenPosition + Vector2.One / 2;

        if (globalTimeWrappedHourly >= 1f)
        {
            globalTimeWrappedHourly = 2f - globalTimeWrappedHourly;
        }
        globalTimeWrappedHourly = globalTimeWrappedHourly * 0.5f + 0.4f;//控制圆的距离
        //Main.NewText(num4);

        float Xpos = Projectile.Center.X / 54f;//控制旋转
        for (float num5 = 0f; num5 < 1f; num5 += 0.2f)
        {
            Main.spriteBatch.Draw(texture,
                vector3 + new Vector2(-1f + num5 * 8, 8f).RotatedBy((num5 + Xpos) * ((float)Math.PI * 2f)) * globalTimeWrappedHourly, 
                null, Color.GreenYellow, 0, vector, Projectile.scale, SpriteEffects.None, 0f);
        }

        for (float num7 = 0f; num7 < 1f; num7 += 0.25f)
        {
            Main.spriteBatch.Draw(texture2,
                vector3 + new Vector2(0f + num7 * 6, 6f).RotatedBy((num7 + Xpos) * ((float)Math.PI * 2f)) * globalTimeWrappedHourly,
                null, new Color(180, 200, 180, 220), 0, vector, Projectile.scale, SpriteEffects.None, 0f);
        }

        for (float num6 = 0f; num6 < 1f; num6 += 0.34f)
        {
            Main.spriteBatch.Draw(texture3,
                vector3 + new Vector2(0f, 4f).RotatedBy((num6 + Xpos) * ((float)Math.PI * 2f)) * globalTimeWrappedHourly,
                null, new Color(160, 190, 160, 250), 0, vector, Projectile.scale, SpriteEffects.None, 0f);
        }
        return false;
    }


    public override bool OnTileCollide(Vector2 oldVelocity)//在AI之前执行
    {
        homoplayer.jumpCount = 0;
        timeCheckAir = 0;
        Projectile.velocity = Collision.TileCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height, false, false, (int)player.gravDir);
        if (Projectile.ai[0] == 0)
        {
            if (isInAir)
            {
                SoundEngine.PlaySound(AssetsLoader.intro_slime_land);
                isInAir = false;
            }
            if ((player.controlLeft || player.controlRight)  && !isInAir)
            {
                if (oldSlideDir == 0)
                {
                    SpawnHomoDust(oldSlideDir);
                }
                if (soundplayCooldown == 0)
                {
                    if (Main.rand.NextBool(3))
                        SoundEngine.PlaySound(AssetsLoader.intro_slime_move1, Projectile.Center);
                    else if (Main.rand.NextBool(2))
                        SoundEngine.PlaySound(AssetsLoader.intro_slime_move2, Projectile.Center);
                    else
                        SoundEngine.PlaySound(AssetsLoader.intro_slime_move3, Projectile.Center);

                    soundplayCooldown += 30;
                }
            }
        }
        return false;
    }



    public override void AI()
    {

        if (!Main.player[Projectile.owner].active)
        {
            Main.NewText(Projectile.owner);
            Projectile.Kill();
        }


        Vector2 hp = player.Top - Projectile.Center;
        float distance = hp.Length();
        if (!Main.dedServ)
        {
            Lighting.AddLight(Projectile.Center, 0.8f, 0.8f, 0.55f);
        }

        if (soundplayCooldown > 0)
            soundplayCooldown--;


        if (homoplayer.jumpCount == 1)//第二次跳跃衰减
            doublejumplimit = 1.6f;
        else doublejumplimit = 0f;

        if (Projectile.ai[0] == 0)//玩家控制头运动
        {

            if (Projectile.velocity.Y > 0)
                timeCheckAir++;
            if (!isInAir && timeCheckAir > 20 && !isColliding)//判断其是否在空中，用于判断播放落地声音
                isInAir = true;

            Vector2 vector = Collision.TileCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height, player.controlDown, false, (int)player.gravDir);

            int hitGround = Math.Sign((vector - Projectile.velocity).Y);//-1为落到地上,1为撞天花板

            if (hitGround == -1)
            {
                oldSlideDir = 0;
            }

            int slideDir = (oldSlideDir != 0) ? oldSlideDir : -Math.Sign((vector - Projectile.velocity).X);//加了负号，所以向左撞墙是-1，向右是1

            //落地时加上微小位移
            if (hitGround == -1)
            {
                float leftside = Projectile.position.X;
                float rightside = leftside + Projectile.width;
                leftside += -1;
                rightside += 1;
                float num2 = Projectile.position.Y + Projectile.height + 1f;
                if (player.gravDir < 0f)
                {
                    num2 = Projectile.position.Y - 1f;
                }
                leftside /= 16f;
                rightside /= 16f;
                num2 /= 16f;
                if (WorldGen.SolidTile((int)leftside, (int)num2) && WorldGen.SolidTile((int)rightside, (int)num2))
                { }
                else if (WorldGen.SolidTile((int)leftside, (int)num2))
                    Projectile.position.X += 2f;
                else if (WorldGen.SolidTile((int)rightside, (int)num2))
                    Projectile.position.X -= 2f;
            }
            if (homoplayer.frozenTime > 0)//刚开始飞头时的效果
            {
                Projectile.velocity.X *= 0.98f;
                Projectile.velocity.Y += 0.2f;
            }

            if (homoplayer.headflying)//处在飞头过程，玩家控制头运动
            {
                Projectile.tileCollide = true;//不可穿墙

                if (slideDir != 0)//碰到了墙
                {
                    oldSlideDir = slideDir;
                    keepSliding = true;
                    WallslideMovement(slideDir);
                }


                else
                {
                    //这一段模拟物理效果
                    if (Projectile.velocity.Y == 0f)
                    {
                        Projectile.velocity.X *= 0.95f;
                    }
                    Projectile.velocity.X *= 0.98f;
                    Projectile.velocity.Y += 0.4f;
                    if (Projectile.velocity.Y > 16f)
                    {
                        Projectile.velocity.Y = 16f;
                    }
                }
            }

        }


        //Main.NewText("0:"+Projectile.ai[0]);
        //Main.NewText("1:"+Projectile.ai[1]);

        if (Projectile.ai[0] == 1f)
        {
            Projectile.tileCollide = false;
            homoplayer.headflying = false;
            homoplayer.attaching = true;
            int count = (int)Projectile.ai[1];
            bool shouldAddDmg = false;
            Projectile.localAI[0]++;
            Projectile.localAI[1]--;
            if (Projectile.localAI[0] % 25f == 0f)
            {
                shouldAddDmg = true;
            }


            if (count < 0 || count >= 200 || Projectile.localAI[1] <= 0 || !Main.npc[count].active || Main.npc[count].dontTakeDamage)
            {
                SoundEngine.PlaySound(AssetsLoader.homunculus_comeback);
                homoplayer.reactiveReturn = true;
                homoplayer.shouldgetback = true;
            }
            else if (Main.npc[count].active)
            {
                Projectile.Center = Main.npc[count].Center - Projectile.velocity * 2f;
                Projectile.gfxOffY = Main.npc[count].gfxOffY;
                if (shouldAddDmg)
                {
                    Main.npc[count].HitEffect(0, 8);
                }
            }
        }
        float limit2 = 0f;
        if (distance <= 512)
        {
            if (player.controlLeft && Projectile.velocity.X > -3.2f)
                Projectile.velocity.X -= 0.14f;
            if (player.controlRight && Projectile.velocity.X < 3.2f)
                Projectile.velocity.X += 0.14f;
            if (player.justJumped && homoplayer.jumpCount < 2)
            {
                SoundEngine.PlaySound(AssetsLoader.jump);
                homoplayer.jumpCount++;
                if (Projectile.velocity.Y > 0)
                    Projectile.velocity.Y *= 0f;
                else
                    limit2 = Projectile.velocity.Y * Projectile.velocity.Y / 25f + MathHelper.Lerp(0f, 1f, distance / 512f);
                Projectile.velocity.Y -= (6.8f - doublejumplimit - limit2);
            }
        }
        if (distance > 400f && distance <= 512f)
        {
            int side = Math.Sign(hp.X);//头在玩家左侧为1，右侧为-1
            float acc = MathHelper.Lerp(0f, 0.1399f, (distance - 400f) / 112f);
            if (Projectile.velocity.X < 3.2f && Projectile.velocity.X > -3.2f)
                Projectile.velocity.X += acc * side;

            if (player.justJumped && homoplayer.jumpCount < 2)
            {
                if(Projectile.velocity.Y > 0)
                    Projectile.velocity.Y *= 0f;

                Projectile.velocity.Y -= 2f * acc;
            }
        }
        if (distance > 512f)
        {
            homoplayer.shouldgetback = true;
            if (homoplayer.attaching)
                homoplayer.reactiveReturn = true;
            if (homoplayer.playcomebacksound)
            {
                SoundEngine.PlaySound(AssetsLoader.homunculus_comeback);
                homoplayer.playcomebacksound = false;
            }
        }
        if(distance > 1000)
        {
            Projectile.Kill();
        }
        if (homoplayer.shouldgetback)//因为由按键控制，所以得用ModPlayer的属性
        {
            Projectile.ai[0] = 2f;
            //再写一遍，万无一失
            Projectile.damage = 0;
            Projectile.friendly = false;
            Projectile.hostile = false;
            homoplayer.headflying = false;
            homoplayer.attaching = false;
            Projectile.tileCollide = false;

            float num = 1.2f;
            float num2 = hp.X;
            float num3 = hp.Y;
            float num4 = 16f / distance;
            num2 *= num4;
            num3 *= num4;
            Projectile.velocity.X = 2 * (Projectile.velocity.X * (num - 1) + num2) / num;
            Projectile.velocity.Y = 2 * (Projectile.velocity.Y * (num - 1) + num3) / num;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            if (Main.myPlayer == Projectile.owner && Projectile.Hitbox.Intersects(player.Hitbox))
            {
                Projectile.Kill();
            }
        }
    }
    public override bool? CanHitNPC(NPC target)
    {
        if (Projectile.ai[0] == 1f)
        {
            if (target.whoAmI != (int)Projectile.ai[1])
                return false;
            else return true;
        }
        else if (Projectile.ai[0] == 2f)
            return false;
        else   
            return null;

    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Projectile.ai[0] == 0 && !target.immortal && !target.dontTakeDamage)
        {
            Projectile.ai[0] = 1f;
            Projectile.ai[1] = target.whoAmI;
            Projectile.localAI[1] = 540;
            Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
        }
        SoundEngine.PlaySound(AssetsLoader.hit_poison);
    }

    public override void PostDraw(Color lightColor)
    {
        /*
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        */
        Main.spriteBatch.Draw(AssetsLoader.HeadSpark,
            Projectile.Center - Main.screenPosition,
            null,
            Color.White,
            0f,
            AssetsLoader.HeadSpark.Size() / 2,
            1f, SpriteEffects.None, 0);
        /*
        Main.spriteBatch.End();
        Main.spriteBatch.Begin();
        */
    }
    public override void OnKill(int timeLeft)
    {
        homoplayer.playcomebacksound = true;
        homoplayer.jumpCount = 0;
        homoplayer.shouldgetback = false;
        homoplayer.headflying = false;
        homoplayer.attaching = false;
        homoplayer.reactiveReturn = false;
        homoplayer.canplayermove = true;
        homoplayer.frozenTime = 16;

        RopePhysicalEffects.KillAllVerletObj(rp1, rl1);
        RopePhysicalEffects.KillAllVerletObj(rp2, rl2);
        RopePhysicalEffects.KillAllVerletObj(rp3, rl3);
    }

    public void WallslideMovement(int slideDir)
    {
        if (player.justJumped)//这里也有问题
        {
            keepSliding = false;
            oldSlideDir = 0;
            Projectile.velocity.X += -slideDir * 1.3f;
            Projectile.velocity.Y -= player.gravDir * 0.32f;
            return;
        }
        bool flag = false;
        float num = Projectile.position.X;
        if (slideDir == 1)
        {
            num += Projectile.width;
        }
        num += slideDir;
        float num2 = Projectile.position.Y + Projectile.height + 1f;
        if (player.gravDir < 0f)
        {
            num2 = Projectile.position.Y - 1f;
        }
        num /= 16f;
        num2 /= 16f;
        if (WorldGen.SolidTile((int)num, (int)num2))
        {
            flag = true;
        }

        if (!flag)
        {
            keepSliding = false;
            oldSlideDir = 0;
            return;
        }
        if ((((player.controlLeft && slideDir == -1) || (player.controlRight && slideDir == 1)) && player.gravDir == 1f) ||
           (((player.controlLeft && slideDir == 1) || (player.controlRight && slideDir == -1)) && player.gravDir == -1f))
        {
            Projectile.velocity.Y = -2.1f * player.gravDir;
            SpawnHomoDust(slideDir);
        }
        else if ((player.controlDown && player.gravDir == 1f) || (player.controlUp && player.gravDir == -1f))
        {
            Projectile.velocity.Y = 4f * player.gravDir;
            SpawnHomoDust(slideDir);
        }
        else
        {
            Projectile.velocity.Y *= 0f;
        }

            oldSlideDir = keepSliding ? slideDir : 0;

    }
    public void SpawnHomoDust(int slideDir)
    {
        int num4 = Dust.NewDust(
            new Vector2(Projectile.position.X + (float)(Projectile.width / 2) + (float)((Projectile.width / 2 - 4) * slideDir), 
            Projectile.position.Y + (float)(Projectile.height / 2) + (float)(Projectile.height / 2 - 4) * player.gravDir), 
            8, 
            8, 
            DustID.t_Slime, 
            0, 
            0, 
            0, 
            Color.LawnGreen);
        if (slideDir < 0)
        {
            Main.dust[num4].position.X -= 10f;
        }
        if (player.gravDir < 0f)
        {
            Main.dust[num4].position.Y -= 12f;
        }
        Main.dust[num4].velocity *= 0.1f;
        Main.dust[num4].scale *= 1.2f;
        Main.dust[num4].noGravity = true;
    }
}
