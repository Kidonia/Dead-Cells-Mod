using Terraria.ModLoader;
using Terraria;

namespace DeadCells.Items
{
    public class TestTeleprot : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.TeleportAltar>());
            Item.consumable = false;
            Item.width = 38;
            Item.height = 24;
            Item.placeStyle = 1;
            Item.value = 150;
        }
    }
}
