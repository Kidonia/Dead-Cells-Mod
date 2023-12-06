using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using DeadCells.Common.Players;

namespace DeadCells.Items;

public class BrutalityScroll : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.IsLavaImmuneRegardlessOfRarity[Item.type] = true;
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }
    public override void SetDefaults()
    {
        //暴虐卷轴
        Item.width = 48;
        Item.height = 48;
        Item.rare = ItemRarityID.Red;
        Item.material = false;
        Item.useStyle =  ItemUseStyleID.HoldUp;
    }
    public override bool? UseItem(Player player)
    {
        var Br = player.GetModPlayer<PlayerEquipAndScroll>();
        Br.BrutalityNum++;

        return true;
    }

}
//3 scroll