using System;
using Terraria;
using Terraria.ModLoader;

namespace DeadCells.Common.Players
{
    [Autoload(true, Side = ModSide.Client)]
    public sealed class PlayerTrailEffects : ModPlayer
    {
        private int forceTrailEffectTime;

        public override void Load()
        {
            Terraria.On_Player.SetArmorEffectVisuals += delegate (Terraria.On_Player.orig_SetArmorEffectVisuals orig, Terraria.Player player, Terraria.Player drawPlayer)
            {
                orig(player, drawPlayer);
                PlayerTrailEffects modPlayer = drawPlayer.GetModPlayer<PlayerTrailEffects>();
                if (modPlayer.forceTrailEffectTime > 0)
                {
                    player.armorEffectDrawShadow = true;
                    modPlayer.forceTrailEffectTime--;
                }
            };
        }
        public void ForceTrailEffect(int forTicks)
        {
            forceTrailEffectTime = Math.Max(forceTrailEffectTime, forTicks);
        }
    }
    //roll
}
