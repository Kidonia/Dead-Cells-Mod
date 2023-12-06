using DeadCells.Core;
using DeadCells.Projectiles.NPCMeleeProj;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace DeadCells.NPCs;

public class macFly : DC_BasicNPC
{
    //ai[0]用于控制当前动作播放，ai[1]用于控制行为

    private Dictionary<int, DCAnimPic> AtkDic = new();
    private Dictionary<int, DCAnimPic> AtkLoadDic = new();
    private Dictionary<int, DCAnimPic> IdleDic = new();
    private int atkCharge;

    private int noticeTime = 32;//初始发现玩家后的僵直时间

    private bool drawFX;
    private bool drawGlow;


    private Dictionary<int, DCAnimPic> fxDic = new();
    public override void SetDefaults()
    {
        AtkDic = AssetsLoader.macFlyDic["atk"];
        IdleDic = AssetsLoader.macFlyDic["fly"];
        AtkLoadDic = AssetsLoader.macFlyDic["atkLoad"];
        fxDic = AssetsLoader.fxEnmAtlas["FXCrossbowManAtkMelee"];

        NPC.lifeMax = 200;
        NPC.width = 32;
        NPC.height = 32;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.damage = 1;
        NPC.defense = 1;
        NPC.scale = 1.8f;
        NPC.friendly = false;
    }

    public override void AI()
    {
        float distance = Vector2.Distance(player.Center, NPC.Center);
        if (NPC.ai[1] == 0)
        {
            NPC.velocity.Y = (float)(1.25 * Math.Sin((double)(MathHelper.Pi / 60f * Main.GameUpdateCount)));
            NPC.velocity.X += Main.rand.NextFloat(-0.3f, 0.31f);
        }
        if (distance < 500f && NPC.ai[1] == 0)//距离小于500 且 处于待机状态，视作发现玩家
        {
            SoundEngine.PlaySound(AssetsLoader.enm_bat_trigger);
            NPC.ai[1] = 1;//发现并追踪玩家
        }
        if (NPC.ai[1] == 1)//追踪玩家
        {
            if (NPC.ai[0] > 0)//如果攻击后继续追踪，返回飞行动作
            { NPC.ai[0] = 0; }

            Vector2 targetVel = Vector2.Normalize(player.Center - NPC.Center);
            targetVel *= 7f;
            // X分量的加速度
            float accX = 0.32f;
            // Y分量的加速度
            float accY = 0.32f;
            if (noticeTime > 0) noticeTime--;//反应时间
            else//反应结束
            {
                NPC.velocity.X += (NPC.velocity.X < targetVel.X ? 1.15f : -1.15f) * accX;
                NPC.velocity.Y += (NPC.velocity.Y < targetVel.Y ? 1.15f : -1.15f) * accY;
            }
            if (distance < 36)//追上玩家
            {
                noticeTime = 0;//直接清零
                SoundEngine.PlaySound(AssetsLoader.enm_fly_charge);
                NPC.ai[1] = 2;//追上玩家
                RePlayFrame();//动作归零
                NPC.ai[0] = 1;//待攻击动作
                atkCharge = 24;//待机时间
                NPC.velocity /= 2f;
            }
        }
        if (NPC.ai[1] == 2)//追上玩家，速度清零，准备攻击
        {
            NPC.velocity /= 2f;
            atkCharge--;//待攻击时间--
            if (atkCharge == 0)//待机时间结束
            {
                NPC.ai[1] = 3;//准备攻击收尾
                RePlayFrame();//动作归零
                NPC.ai[0] = 2;//进入攻击
                SoundEngine.PlaySound(AssetsLoader.enm_fly_release);
                Projectile.NewProjectile(Entity.GetSource_FromAI(), NPC.Center + new Vector2(NPC.direction * 25, 12), Vector2.Zero, ModContent.ProjectileType<DC_EnmMeleeAtk>(), 18, 1f,-1, 92, 124);
            }
        }
        if (NPC.ai[1] == 3)//攻击
        {
            //攻击弹幕写上面了
            if (NPC.ai[3] == AtkDic.Count - 1)//攻击结束
                NPC.ai[1] = 1;//重新追踪玩家
        }


        if (NPC.ai[0] == 0)//飞行动作
        {
            NPC.aiStyle = 0;
            drawGlow = false;
            drawFX = false;
            ChooseCorrectFrame(IdleDic);
        }
        else if (NPC.ai[0] == 1)//攻击前等待动作
        {
            NPC.aiStyle = -1;
            drawGlow = true;
            drawFX = false;
            ChooseCorrectFrame(AtkLoadDic);
        }
        else if (NPC.ai[0] == 2)//攻击
        {
            NPC.aiStyle = -1;
            drawGlow = true;
            drawFX = true;
            ChooseCorrectFrame(AtkDic);
        }
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        spriteBatch.Draw(TextureAssets.Npc[Type].Value, NPC.Center - Main.screenPosition - NPC.frame.Size() * NPC.scale / 2, NPC.frame, Color.White, 0f, Vector2.Zero, NPC.scale, (SpriteEffects)(-(NPC.direction - 1) / 2), 0);
        
        if (drawGlow)
        {
           DrawGlowTexture(AssetsLoader.macFlyGlowTex, spriteBatch, new(1, 252, 187));
        }
        if(drawFX)
        {
            DrawfxTexture(fxDic, 24, -38);
        }
        return false;
    }
    public override void HitEffect(NPC.HitInfo hit)
    {
        AddDCNPCHitEffect(AssetsLoader.enm_zmb_die, 8018405);
    }

}
