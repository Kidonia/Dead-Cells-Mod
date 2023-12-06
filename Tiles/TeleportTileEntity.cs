using System.IO;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader.Default;

namespace DeadCells.Tiles;

public class TeleportTileEntity : TEModdedPylon
{
    public override bool IsTileValidForEntity(int x, int y)//避免切换到第二帧的传送阵退出重进就不加载了
    {
        return true;
    }
}
