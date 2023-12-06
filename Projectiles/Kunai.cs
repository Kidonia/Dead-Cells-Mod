using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DeadCells.Common.Buffs;
using DeadCells.Common.DamageClasses;
using DeadCells.Common.GlobalNPCs;
using DeadCells.Common.Players;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeadCells.Projectiles;

public class Kunai : ModProjectile
{
    private double rotation;
    private Vector2[] oldPosi = new Vector2[16]; //示例中记录16个坐标用于绘制，你可以试着修改这个值，并思考这意味着什么。
    private Texture2D tex;
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("圆舞飞刀");
        ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
    }
    public override void SetDefaults()
    {
        Projectile.width = 8;
        Projectile.height = 8;
        Projectile.DamageType = BrutalityDamage.Instance;
        Projectile.damage = 30;

        Projectile.ignoreWater = true;
        Projectile.timeLeft = 16; //飞行时间
        Projectile.tileCollide = true;
        Projectile.friendly = true;
        Projectile.penetrate = 1;
        Projectile.usesLocalNPCImmunity = false; //无敌帧
        tex = ModContent.Request<Texture2D>("DeadCells/fxEffects/LongLineRed", (AssetRequestMode)2).Value;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Player player = Main.player[Projectile.owner];
        Texture2D texture = ModContent.Request<Texture2D>("DeadCells/fxEffects/BigRedCircle", (AssetRequestMode)2).Value;//刚开始在玩家周围画个圆
        if (Projectile.timeLeft > 14)//在开始的两帧
        {
            Main.spriteBatch.Draw(texture, player.Center - Main.screenPosition - texture.Size() / 2f, new Color(200, 200, 200, 0));//画圆
        }

        lightColor = new Color(220, 220, 210, 220);//给苦无加光照
        return true;
    }
    public override void OnSpawn(IEntitySource source)
    {
        Player player = Main.player[Projectile.owner];
        var ply = player.GetModPlayer<PlayerDraw>();
        ply.draw_knife_dance = true;//需要添加冷却@

        Projectile.rotation = Projectile.Center.ToRotation() + (float)Math.PI / 2f;//确定苦无朝向
        rotation = Projectile.Center.ToRotation();
        if (rotation < 0)
        {
            rotation += MathHelper.TwoPi;
        }
        Projectile.Center = player.Center + Terraria.Utils.RotatedBy(new Vector2(25f, 0f), rotation);//确定苦无位置


    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.velocity = Vector2.Zero;
        Projectile.rotation = oldVelocity.ToRotation() + (float)(0.5 * MathHelper.Pi);
        Projectile.ai[1] = 2;
        return false;
    }

    public override void AI()
    {
        Lighting.AddLight(Projectile.Center, 0.3f, 0.3f, 0.16f);//飞行时作为小型光源

        if (Projectile.ai[1] != 2) //判断是否撞墙，这里是不撞墙
        {
            for (int i = Projectile.oldPos.Length - 1; i > 0; i--) //计数器，绘制拖尾用
            {
                oldPosi[i] = oldPosi[i - 1];
            }
            oldPosi[0] = Projectile.Center;

            Projectile.rotation = Projectile.velocity.ToRotation() + (float)(0.5 * MathHelper.Pi);  //弹幕贴图方向朝上的情况
        }
        if(Projectile.timeLeft==1f)//结束时刻生成红色影子
        {
            IEntitySource source = Entity.GetSource_FromAI();
            Vector2 currentpos = Projectile.Center;
            for (int i = 0; i < 2; i++)
            {
                Projectile.NewProjectileDirect(source, currentpos, Vector2.Zero, ModContent.ProjectileType<LineRed>(), 0, 0f, Main.myPlayer, 0.9f);//生成
            }
        }
    }
    public override void PostDraw(Color lightColor)
    {
        for (int i = oldPosi.Length - 1; i > 0; i--)//绘制拖尾
        {
            if (oldPosi[i] != Vector2.Zero)
            {
                Main.spriteBatch.Draw(tex, oldPosi[i] - Main.screenPosition, null, new Color(230, 230, 230, 0) * 1 * (1 - 0.12f * i), (oldPosi[i - 1] - oldPosi[i]).ToRotation() + (float)(0.5 * MathHelper.Pi), tex.Size() * 0.5f, 1 * (1 - 0.02f * i), SpriteEffects.None, 0);
            }
        }
    }


    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(ModContent.BuffType<Bleed>(), 240);//添加流血DeBuff

        var drawicon = target.GetGlobalNPC<NPCDrawBuff>();
        drawicon.bleednum++;//流血层数加一

    }
}
