using DeadCells.Common.Players;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using DeadCells.Core;

namespace DeadCells.Common.GlobalNPCs
{
    public class CountKillNumber : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            var PLC = Main.LocalPlayer.GetModPlayer<PlayerCell>();
            var curseNum = Main.LocalPlayer.GetModPlayer<PlayerHurt>();
            int num = Item.NPCtoBanner(npc.BannerID());
            if (num > 0 && !npc.ExcludedFromDeathTally())
            {
                if (npc.type == NPCID.Bunny || npc.type == NPCID.BunnySlimed || npc.type == NPCID.BunnyXmas || npc.type == NPCID.GoldBunny || npc.type == NPCID.PartyBunny)
                {
                }
                else
                {
                    PLC.num_of_kill += 1;//连杀加一

                    if (curseNum.CurseNum > 0)//诅咒减一
                    {
                        if (curseNum.CurseNum == 1)
                        {
                            SoundEngine.PlaySound(AssetsLoader.curse_end);
                        }
                        curseNum.CurseNum -= 1;
                    }
                }
            }
            base.OnKill(npc);
            //combo kill
        }
    }
}
