using Microsoft.Xna.Framework;
using DeadCells.Common.Players;
using System;
using System.Numerics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace DeadCells.Projectiles;

    public class LineRed : ModProjectile
{
    private double rotation;
    private readonly int LifetimeOnSpawn = 30 + Main.rand.Next(6, 22);
    private readonly int k = Main.rand.Next(1, 9);
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("红短线");
        ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
    }

    public override void SetDefaults()
    {
        //圆舞飞刀使用时，本弹幕生成玩家身上炸开的红线。（使用ai[1]>0来确定）①
        //飞刀苦无结束飞行时，本弹幕生成红色的残影。
        Projectile.width = 7;
        Projectile.height = 20;
        Projectile.damage = 0;
        Projectile.DamageType = DamageClass.Default;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = true;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.timeLeft = LifetimeOnSpawn;

    }



    public override bool PreDraw(ref Color lightColor)
    {

        lightColor = new Color(255, 0, 0, 0);
        return true;
    }
    public override void OnSpawn(IEntitySource source)
    {
        Player player = Main.player[Projectile.owner];

        //确定生成位置
        Projectile.rotation = Projectile.Center.ToRotation() + (float)Math.PI / 2f;
        rotation = Projectile.Center.ToRotation();
        if (rotation < 0)
        {
            rotation += MathHelper.TwoPi;
        }
        if (Projectile.ai[1] > 0)//参数判断①：生成位置
        {
            Projectile.Center = player.Center + Terraria.Utils.RotatedBy(new Vector2(1f, 0f), rotation);
        }
        Projectile.scale *= Main.rand.NextFloat(Projectile.ai[0], Projectile.ai[0] + 0.7f);


    }
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        int time_after_spawn = LifetimeOnSpawn - Projectile.timeLeft;

        if (time_after_spawn < 3f)//调整初始旋转角度
        { 
            Vector2 vector = player.Center - Projectile.Center;
        Projectile.rotation = vector.ToRotation() + (float)Math.PI / 2f;
        }


        if (time_after_spawn <= 10+ k && time_after_spawn >= 5+ k)//向外炸开时刻
            Projectile.velocity = Vector2.Zero;

        if (time_after_spawn >= 10 + k)//下坠时刻
        {
            if (Projectile.ai[1] > 0)//参数判断①
            {
                Projectile.velocity.Y += 0.18f;
            }
            Projectile.scale *= 0.94f;
        }
    }

    
}


