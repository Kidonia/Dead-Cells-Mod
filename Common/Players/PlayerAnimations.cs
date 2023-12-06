using Microsoft.Xna.Framework;
using DeadCells.Utils;
using Terraria.ModLoader;
using Terraria;

namespace DeadCells.Common.Players
{
    public sealed class PlayerAnimations : ModPlayer
    {
        public PlayerFrames? ForcedHeadFrame;

        public PlayerFrames? ForcedBodyFrame;

        public PlayerFrames? ForcedLegFrame;
        public override void PostUpdate()
        {
            TryForceFrame(ref base.Player.headFrame, ref ForcedHeadFrame);
            TryForceFrame(ref base.Player.bodyFrame, ref ForcedBodyFrame);
            TryForceFrame(ref base.Player.legFrame, ref ForcedLegFrame);
            static void TryForceFrame(ref Rectangle frame, ref PlayerFrames? newFrame)
            {
                if (newFrame.HasValue)
                {
                    frame = new Rectangle(0, (int)newFrame.Value * 56, 40, 56);
                    newFrame = null;
                }
            }
        }
    }
    //roll
}
