using DeadCells.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DeadCells.Common.Players
{
    public class PlayerCell : ModPlayer
    {

        public int CurrentCellNum = 0; 
        public int CellGet;
        public int k;
        public int num_of_kill = 0;



        public override void Initialize()
        {
            CellGet = CurrentCellNum; 
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            k = CurrentCellNum / 4;
            for (int u = -12; u <= 4; u += 8)
            {
                for (int q = 0; q < k; q++)
                {
                    int num = Item.NewItem(Player.GetSource_Misc("Cell"), (int)Player.position.X, (int)Player.position.Y, Player.width * (u), Player.height, ModContent.ItemType<Cell>(), 1, false, 0, false, false);

                    Main.item[num].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
                    Main.item[num].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
                    Main.item[num].noGrabDelay = 100;
                    Main.item[num].newAndShiny = false;
                }

            }
            for (int r = 0; r < CurrentCellNum - 3*k; r++)
            {
                int num1 = Item.NewItem(Player.GetSource_Misc("Cell"), (int)Player.position.X, (int)Player.position.Y, Player.width * (12), Player.height, ModContent.ItemType<Cell>(), 1, false, 0, false, false);

                Main.item[num1].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
                Main.item[num1].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
                Main.item[num1].noGrabDelay = 100;
                Main.item[num1].newAndShiny = false;
            }

        }

        public override void OnHurt(Player.HurtInfo info)
        {
            num_of_kill = 0;
            info.SoundDisabled = true;
        }

        public override void UpdateDead()
        {
            CurrentCellNum = 0;
            num_of_kill = 0;
        }

        public override void SaveData(TagCompound tag)
        {
            tag["MyCells"] = CurrentCellNum;
            tag["ComboKill"] = num_of_kill;
        }
        public override void LoadData(TagCompound tag)
        {
            CurrentCellNum = tag.GetInt("MyCells");
            num_of_kill = tag.GetInt("ComboKill");
        }


    }
    //cell num
    //combo kill
}
