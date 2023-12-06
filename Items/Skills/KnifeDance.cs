using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;
using DeadCells.Common.Players;
using DeadCells.Core;
using DeadCells.Projectiles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeadCells.Items.Skills;

public class KnifeDance : DeadCellsItem
{
    public override void SetDefaults()
    {
        IsSkill = true;//提醒自己用
        SetSkillDefaults(BrutalityDamage.Instance, 20, 20, 1250);
    }
    public override bool? UseItem(Player player)
    {
        SoundEngine.PlaySound(AssetsLoader.active_laceration);

        IEntitySource source = player.GetSource_FromAI();
        int Bdamage = (int)player.GetTotalDamage<BrutalityDamage>().ApplyTo(30); // 初始伤害30
        for (float r = 0f; r < MathHelper.TwoPi; r += MathHelper.TwoPi / 16f)//玩家四周发射十六个苦无
        {
            Vector2 vector = new Vector2((float)Math.Cos(r), (float)Math.Sin(r));
            Projectile.NewProjectileDirect(source, vector*145f, vector*28f, ModContent.ProjectileType<Kunai>(), Bdamage, 0f, Main.myPlayer);
        };

        for (int i = 0; i < 160; i++)//身体周围生成粒子
        {
            int num2 = Dust.NewDust(new Vector2(player.position.X - 20f, player.position.Y), player.width + 40, player.height + 15, DustID.LifeDrain, Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-4f, -1f), 150, new Color(255, 0, 0), 1f);
            Main.dust[num2].scale += Main.rand.Next(-6, 21) * 0.01f;
            Main.dust[num2].noGravity = true;
        }



        return true;
    }
    public override bool CanUseItem(Player player)
    {
        //（未完成）冷却@
        var able = player.GetModPlayer<PlayerDraw>();
        if (able.draw_knife_dance)
        {
            return false;
        }
        else return true;
    }


}
