using DeadCells.Common.Buffs;
using DeadCells.Common.Players;
using DeadCells.Projectiles;
using DeadCells.Projectiles.EffectProj;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeadCells.Items
{
	public class admin : ModItem
	{
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.DeadCells.hjson file.

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<Homunculus>();
            Item.shootSpeed = 12f;
            Item.fishingPole = 30;
            Item.autoReuse = false;
		}
		public override void HoldItem(Player player)
		{
            player.accFishingLine = true;
            player.AddBuff(ModContent.BuffType<BeCursed>(), 2);
			base.HoldItem(player);
		}
		public override bool? UseItem(Player player)
		{
			/* Ìí¼Ó×çÖä·½·¨
            if (player.HasBuff<BeCursed>())
			{
				var cursenum = player.GetModPlayer<PlayerHurt>();
				cursenum.CurseNum += 1;
			
			}
			*/
                return base.UseItem(player);
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float spreadAmount = 75f; // how much the different bobbers are spread out.
			Vector2 bobberSpeed = velocity + new Vector2(Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f, Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f);
            Projectile.NewProjectile(source, position, bobberSpeed, type, 0, 0f, player.whoAmI);

            return false;
		}
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			IEntitySource source = player.GetSource_FromAI();
			//Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<TeleportAvailable>(), 0, 0);
			base.OnHitNPC(player, target, hit, damageDone);
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}