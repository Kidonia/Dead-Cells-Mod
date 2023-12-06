using DeadCells.Core;
using DeadCells.Items;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeadCells.Common.GlobalNPCs;

    public class NPCLoot : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, Terraria.ModLoader.NPCLoot npcLoot)
        {
            if (!NPCID.Sets.CountsAsCritter[npc.type])
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Cell>(), 10  , 1 , 2));
            }
        }

    public override void HitEffect(NPC npc, NPC.HitInfo hit)
    {
        if (hit.Crit)
        {
            SoundEngine.PlaySound(AssetsLoader.hit_crit);
        }
    }

}
    //cell drop : incomplete

