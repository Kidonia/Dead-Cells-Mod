using DeadCells.Common.DamageClasses;
using DeadCells.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeadCells.Items.Armors;
[AutoloadEquip(EquipType.Head)]
public class Beheaded : ModItem
{
    public override void SetStaticDefaults()
    {
        // If your head equipment should draw hair while drawn, use one of the following:
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
        ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = false; // Draw hair as if a hat was covering the top. Used by Wizards Hat
        ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = false; // Draw all hair as normal. Used by Mime Mask, Sunglasses
        ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = false; 
    }
    public override void SetDefaults()
    {
        Item.width = 48; // Width of the item
        Item.height = 48; // Height of the item
        Item.rare = ItemRarityID.Purple; // The rarity of the item
    }
    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        // 这里没有判定头盔类型，因为玩家要穿着这个头盔才会执行这里的判定
        return true;
    }
    public override void UpdateEquip(Player player)
    {
        var bru = player.GetModPlayer<PlayerEquipAndScroll>();
        player.GetDamage<BrutalityDamage>() += (float)bru.BrutalityMul;
        player.GetDamage<TacticsDamage>() += (float)bru.TacticsMul;
        player.GetDamage<SurvivalDamage>() += (float)bru.SurvivalMul;
    }
}
