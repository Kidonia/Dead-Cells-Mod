using Microsoft.Xna.Framework;
using DeadCells.Common.GlobalNPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeadCells.Common.Buffs;

public class Oil : ModBuff
{
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Oil");
        // Description.SetDefault("Covered by oil.");
        Main.debuff[Type] = true;
        Main.buffNoSave[Type] = false;
        Main.pvpBuff[Type] = false;
    }

    public override void Update(NPC npc, ref int buffIndex)
    {
        var drawicon = npc.GetGlobalNPC<NPCDrawBuff>();
        drawicon.oil = true;
        for (int i = 0; i < 1 + npc.width * npc.height / 1200; i++)
        {
            int num2 = Dust.NewDust(npc.TopLeft, npc.width+Main.rand.Next(-2, 2), npc.height + Main.rand.Next(-2, 2), DustID.Ash, 0, 0, 0, default, 1.5f);
            Main.dust[num2].alpha += Main.rand.Next(120, 140);
            Main.dust[num2].velocity.X = 0;
            Main.dust[num2].velocity.Y = 0.1f;
            Main.dust[num2].noGravity = true;
        }
        if (npc.buffTime[buffIndex]==0)
        {
            drawicon.oil = false;
        }
    }
    public override bool ReApply(NPC npc, int time, int buffIndex)
    {
        if (npc.onFire || npc.onFire2 || npc.onFire3 || npc.onFrostBurn || npc.onFrostBurn2)
        {
            npc.buffTime[buffIndex] = 300;
        }
        else
            npc.buffTime[buffIndex] = time;
        return true;
    }
}
