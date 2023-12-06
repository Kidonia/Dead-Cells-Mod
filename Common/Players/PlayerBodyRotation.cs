using Microsoft.Xna.Framework;
using DeadCells.Core;
using DeadCells.Utils;
using System;
using Terraria;
using Terraria.ModLoader;

namespace DeadCells.Common.Players
{
    public sealed class PlayerBodyRotation : ModPlayer
    {
        public static readonly ConfigEntry<bool> EnablePlayerTilting = new ConfigEntry<bool>(ConfigSide.ClientOnly, "PlayerVisuals", "EnablePlayerTilting", () => true);

        public float Rotation;

        public float RotationOffsetScale;

        public override void PreUpdate()
        {
            if (!base.Player.dead)
            {
                base.Player.fullRotationOrigin = new Vector2(11f, 22f);
            }
        }

        public override void PostUpdate()
        {
            if (base.Player.sleeping.isSleeping)
            {
                return;
            }
            if (this.RotationOffsetScale != 0f && PlayerBodyRotation.EnablePlayerTilting)
            {
                float movementRotation = ((!base.Player.OnGround()) ? MathHelper.Clamp(base.Player.velocity.Y * (float)Math.Sign(base.Player.velocity.X) * -0.015f, -0.4f, 0.4f) : (base.Player.velocity.X * ((base.Player.velocity.X < Main.MouseWorld.X) ? 1f : (-1f)) * 0.025f));
                if (base.Player.mount.Active)
                {
                    movementRotation *= 0.5f;
                }
                Rotation += movementRotation;
            }
            base.Player.fullRotation = Rotation * base.Player.gravDir;
            Rotation = 0f;
            RotationOffsetScale = 1f;
        }
    }
    //roll
}
