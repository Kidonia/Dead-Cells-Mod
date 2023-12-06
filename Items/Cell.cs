using System;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using DeadCells.Common.Players;
using Terraria;
using DeadCells.fxEffects;

namespace DeadCells.Items;
public class Cell : ModItem
{
    public override void SetStaticDefaults()
    {
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 1));
        ItemID.Sets.ItemIconPulse[Item.type] = true;
        ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        ItemID.Sets.CommonCoin[Item.type] = true;
        ItemID.Sets.IgnoresEncumberingStone[Item.type] = true;
        ItemID.Sets.IsLavaImmuneRegardlessOfRarity[Item.type] = true;
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }
    public override void SetDefaults()
    {
        Item.width = 44;
        Item.height = 52;
        Item.rare = ItemRarityID.Cyan;
        Item.material = false;
        Item.consumable = true;
    }

    public override bool GrabStyle(Player player)
    {
        Vector2 vector = new Vector2(Item.position.X + (float)(Item.width / 2), Item.position.Y + (float)(Item.height / 2));
        float num = player.Center.X - vector.X;
        float num2 = player.Center.Y - vector.Y;
        float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
        num3 = 17f / num3;
        num *= num3;
        num2 *= num3;
        Item.velocity.X = (Item.velocity.X * (float)(5 - 1) + num) / (float)5;
        Item.velocity.Y = (Item.velocity.Y * (float)(5 - 1) + num2) / (float)5;
        return false;
    }

    public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.LightBlue.ToVector3() * 0.35f * Main.essScale); 

        }

    public override void OnSpawn(IEntitySource source)
    {
        Item.noGrabDelay = 66;
        
        Item.velocity.Y = -5.6f;
        base.OnSpawn(source);
    }
    public override void Update(ref float gravity, ref float maxFallSpeed)
    {
        gravity *= 2.4f;
        base.Update(ref gravity, ref maxFallSpeed);
    }


    public override bool OnPickup(Player player)
        {        
            var cnm = player.GetModPlayer<PlayerCell>();
            cnm.CurrentCellNum += 1;
            Vector2 vect = player.position + new Vector2(10f, 6f);
            Projectile.NewProjectile(Entity.GetSource_FromAI(), vect, Vector2.Zero, ModContent.ProjectileType<DrawRing>(), 0, 0f, default, 0.34f, 0);

            int h = Main.rand.Next(4, 7);
            for (int k = 0; k < h; k++)
            {
                for (int i = 0; i < 3; i++)
                {
                    var unit = Main.rand.NextVector2Unit();
                    var celcolor = new Color(60+ Main.rand.Next(-10, 30), 255, 255, 60+ Main.rand.Next(-20, 30));
                    Dust dust = Dust.NewDustPerfect(player.Center +80* (0.8f + Main.rand.NextFloat(-0.9f, 0.9f)) * -unit, 172, -unit*1.2f, 75, celcolor, 1f);
                    dust.scale = (0.6f + Main.rand.NextFloat(-0.1f, 0.1f)) * 0.65f;
                    dust.fadeIn = 0.72f + Main.rand.NextFloat() * 1.2f;
                    dust.fadeIn *= 1.15f;
                    dust.noGravity = true;
                    dust.velocity -= unit * (2f + Main.rand.NextFloat(-0.2f, 2f) * 3f) * 0.4f;
                }
            }
            return true;
        }



        /*
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player player = Main.player[Main.myPlayer];
            Texture2D texture = ModContent.Request<Texture2D>("DeadCells/fxEffects/st").Value;
            if (check > 0) {
                spriteBatch.Draw(texture, player.Center, null, Color.Aqua, 0, texture.Size() / 2f, 1f, 0, 0);
                check = 0;
            };
            return false;
        }
        */


        public override void GrabRange(Player player, ref int grabRange) 
            {
                grabRange += 850; 
            }

        public override bool ItemSpace(Player player) 
        {
            return true; 
        }




   
}


