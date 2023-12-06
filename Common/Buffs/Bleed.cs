using Microsoft.Xna.Framework;
using DeadCells.Common.GlobalNPCs;
using DeadCells.Common.Players;
using DeadCells.fxEffects;
using DeadCells.Projectiles;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeadCells.Common.Buffs;

public class Bleed : ModBuff
{

    /// <summary>
    /// 血爆基础伤害
    /// </summary>
    private float boomDmgOffset;
    public Player player => Main.player[Main.myPlayer];
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Bleed");
        // Description.SetDefault("Keep losing health.");
        Main.debuff[Type] = true;
        Main.buffNoSave[Type] = false;
        Main.pvpBuff[Type] = false;
    }
    public override void Update(NPC npc, ref int buffIndex)
    {
        int d = npc.direction;
        var drawicon = npc.GetGlobalNPC<NPCDrawBuff>();
        var DPScheck = npc.GetGlobalNPC<CheckBuffDPS>();

        if(DPScheck.CheckBleedDps > 80)//限制DPS上限@@需要添加卷轴伤害@@无所谓了
        {
            DPScheck.CheckBleedDps = 80;
        }



        if (d == 0) d--;//根据NPC朝向确定血液粒子喷射方向

        for (int i = 0; i < 2; i++)//身体周围生成粒子
        {
            int num2 = Dust.NewDust(npc.Center, 1, 1, DustID.Blood, d * Main.rand.NextFloat(7f, 12f), Main.rand.NextFloat(-2f, 8f), 0, new Color(Main.rand.Next(230, 256), 0, 0), Main.rand.NextFloat(0.8f, 1.3f));
            Main.dust[num2].scale += (float)Main.rand.Next(-6, 21) * 0.01f;
        }

        if (npc.lifeRegen > 0)
        {
            npc.lifeRegen = 0;
        }
        npc.lifeRegen -= DPScheck.CheckBleedDps;//初始一层血每秒扣血, 需要添加卷轴伤害@完成？


        if (drawicon.bleednum < 5)
            npc.lifeRegen -= 4 * drawicon.bleednum;//每层叠加增加扣血, 需要添加卷轴伤害@完成？

        //五层叠满血爆
        if (drawicon.bleednum >= 5)
        {
            if (npc.defense > 80)
                boomDmgOffset = 54;//要微调
            else
                boomDmgOffset = npc.defense * 0.7f;
            int boomdmg = CheckBoomDMG() + DPScheck.CheckBleedDps;

            if (npc.life > boomdmg) npc.life -= boomdmg;
            else npc.life = 1;

            //生成文本
            int showBoomDmg = CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), new(222, 11, 16), $"{boomdmg}", true, false);
            Main.combatText[showBoomDmg].lifeTime = 90;

            //下面绘制小血爆
            Vector2 vect = npc.Center;//血爆绘制圆
            IEntitySource source = npc.GetSource_FromAI();
            Projectile.NewProjectile(source, vect, Vector2.Zero, ModContent.ProjectileType<DrawRing>(), 0, 0f, default, 0, 0.36f);

            for (int i = 0; i < 4; i++)
            {
                int m = Main.rand.Next(2, 4);
                for (double r = 0f; r < MathHelper.TwoPi; r += MathHelper.TwoPi / (float)(5 + m))//血爆绘制外圆边
                {
                    float k = Main.rand.NextFloat(-0.4f, 0.4f);
                    Vector2 newposition = new Vector2((float)Math.Cos(r + k), (float)Math.Sin(r + k)) * 60f;
                    float rot = newposition.ToRotation() + (float)Math.PI / 2f;
                    float rotation = newposition.ToRotation();
                    if (rotation < 0)
                    {
                        rotation += MathHelper.TwoPi;
                    }
                     Vector2 Center = npc.Center + Terraria.Utils.RotatedBy(new Vector2(60f + Main.rand.NextFloat(-10f, 3f), 0f), rotation);
                    Projectile.NewProjectile(source, Center, Vector2.Zero, ModContent.ProjectileType<RedLightning>(), 0, 0f, default, rot);
                }
            };

            for (int k = 0; k < 160; k++)//血爆NPC身体周围生成粒子
            {
                int num2 = Dust.NewDust(npc.Center - new Vector2(35f, 35f), 70, 70, DustID.Blood, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, -0.2f), 210, new Color(255, 0, 0), 3f);
                Main.dust[num2].scale *= 1 - Main.rand.NextFloat(0.011f, 0.01f);
                Main.dust[num2].noGravity = true;
            }

            for (int i = 0; i < 25; i++)//血爆NPC身体两侧溅血
            {
                int num2 = Dust.NewDust(npc.Center, 1, 1, DustID.Blood, d * Main.rand.NextFloat(7f, 12f), Main.rand.NextFloat(-2f, 8f), 0, new Color(Main.rand.Next(230, 256), 0, 0), Main.rand.NextFloat(0.8f, 1.3f));
                Main.dust[num2].scale += (float)Main.rand.Next(-3, 6) * 0.01f;
            }
            for (int i = 0; i < 25; i++)//血爆NPC身体两侧溅血
            {
                int num2 = Dust.NewDust(npc.Center, 1, 1, DustID.Blood, -d * Main.rand.NextFloat(7f, 12f), Main.rand.NextFloat(-2f, 8f), 0, new Color(Main.rand.Next(230, 256), 0, 0), Main.rand.NextFloat(0.8f, 1.3f));
                Main.dust[num2].scale += (float)Main.rand.Next(-3, 6) * 0.01f;
            }

            npc.buffTime[buffIndex] = 2;
            drawicon.bleednum = 0;//清零
            DPScheck.CheckBleedDps = 0;
        }

        //层数随时间减少
        if(npc.buffTime[buffIndex] < 20 && npc.buffTime[buffIndex] > 0)
        {
            if (drawicon.bleednum > 0)
            {
                drawicon.bleednum --;
                if(DPScheck.CheckBleedDps > 0)
                {
                    DPScheck.CheckBleedDps -= DPScheck.CheckBleedDps / 4;
                }
                npc.buffTime[buffIndex] += 70;
            }

            if(drawicon.bleednum == 0)
            {
                DPScheck.CheckBleedDps = 0;
                npc.buffTime[buffIndex] = 0;
            }
        }

    }
    public override bool ReApply(NPC npc, int time, int buffIndex)
    {
        npc.buffTime[buffIndex] = time;
        return true;
    }

    private int CheckBoomDMG() //计算血爆伤害
    {
        var mul = player.GetModPlayer<PlayerEquipAndScroll>();//需要微调
        return (int)Math.Round((64 - boomDmgOffset) * mul.GetMaxScrollMulPlusOne()) + Main.rand.Next(-3, 7);//用卷轴最高的加伤
    }

}
