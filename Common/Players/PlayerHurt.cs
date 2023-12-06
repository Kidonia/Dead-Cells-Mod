using DeadCells.Common.Buffs;
using DeadCells.Core;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DeadCells.Common.Players;

public class PlayerHurt : ModPlayer
{
    public int CurseNum;
    /*
     诅咒数目，别忘了。后面开箱子可以用这个加，效果已经写好了。
    		var cursenum = player.GetModPlayer<PlayerHurt>();
			cursenum.CurseNum += 1; 
     */
    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        modifiers.DisableSound();

        if (Player.HasBuff<BeCursed>() || CurseNum > 0)
        {
            Player.KillMe(PlayerDeathReason.ByCustomReason("诅咒致死"), 114514, 0);
        }
    }
   
    public override void PostHurt(Player.HurtInfo info)
    {
        SoundEngine.PlaySound(AssetsLoader.hurt);
    }
    public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
    {
        SoundEngine.PlaySound(AssetsLoader.die);
        CurseNum = 0;//诅咒数清零
    }
    public override void SaveData(TagCompound tag)
    {
        tag["CursedNum"] = CurseNum;
    }
    public override void LoadData(TagCompound tag)
    {
        CurseNum = tag.GetInt("CursedNum");
    }
}
