using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DeadCells.Common.DamageClasses;
using DeadCells.Common.Players;
using DeadCells.Core;
using DeadCells.Projectiles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace DeadCells.Items.Skills;

public class LaceratingAura : DeadCellsItem
{
    public override void SetDefaults()
    {
        AlphaDrawIconRequired = true;
        IsSkill = true;

        SetSkillDefaults(BrutalityDamage.Instance, 20, 20, 1500);
    }
    public override bool? UseItem(Player player)
    {
        SoundEngine.PlaySound(AssetsLoader.active_laceration);

        IEntitySource source = player.GetSource_FromAI();
        float Bdamage = player.GetTotalDamage<BrutalityDamage>().ApplyTo(30); // 初始伤害30
        for (double r = 0f; r < MathHelper.TwoPi; r += MathHelper.TwoPi / 8f)//在玩家周围生成均匀八个角度的方向
        {
            Vector2 newposition =  new Vector2((float)Math.Cos(r), (float)Math.Sin(r)) * 145f;
            Projectile.NewProjectileDirect(source, newposition, Vector2.Zero, ModContent.ProjectileType<Lacerating_Aura_Ring>(), (int)Bdamage, 0f, Main.myPlayer);//生成外环
            Projectile.NewProjectileDirect(source, newposition, Vector2.Zero, ModContent.ProjectileType<Lacerating_Aura_Ring_Inner>(), (int)(Bdamage * 1.2), 0f, Main.myPlayer);//生成内环
        };
        return true;
    }
    public override bool CanUseItem(Player player)
    {
        var able = player.GetModPlayer<PlayerDraw>();//技能冷却
        if (able.draw_lacerating_rings)
        {
            return false;
        }
        else return true;
    }



    //下面两个重写函数用于绘制所有含有透明像素的物品图标。
    public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        Texture2D texture = TextureAssets.Item[Item.type].Value;
        itemColor = new Color(255, 255, 255, 0);
        spriteBatch.Draw(texture, position, frame, itemColor, 0f, origin, scale, SpriteEffects.None, 0f);
        return false;
    }
    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        Texture2D texture = TextureAssets.Item[Item.type].Value;
        Vector2 drawPos = Item.position - Main.screenPosition;
        Color drawColor = new Color(255, 255, 255, 0);
        spriteBatch.Draw(texture,drawPos, drawColor) ;
        return false;
    }
}
