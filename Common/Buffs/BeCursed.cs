using Terraria;
using Terraria.ModLoader;

namespace DeadCells.Common.Buffs;

public class BeCursed : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        Main.buffNoSave[Type] = false;
        Main.pvpBuff[Type] = false;
        Main.buffNoTimeDisplay[Type] = true;
    }
    public override void Update(Player player, ref int buffIndex)
    {
        //属性啥的去Player里面写。


        base.Update(player, ref buffIndex);
    }




}
