using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeadCells.Common.GlobalItems;

public class GlobalShields : GlobalItem
{
    public override bool InstancePerEntity => true;

    public override void SetDefaults(Item entity)
    {
        if (entity.type == ItemID.BouncingShield)//美国队长盾
        {
            entity.accessory = true;
            entity.defense = 5;
            entity.shieldSlot = 9;
        }





        base.SetDefaults(entity);
    }

    public override void UpdateAccessory(Item item, Player player, bool hideVisual)
    {
        base.UpdateAccessory(item, player, hideVisual);
        if (item.type == ItemID.BouncingShield)//美国队长盾
        {
            player.noKnockback = true;
        }
    }
}
