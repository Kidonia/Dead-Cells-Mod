﻿using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace DeadCells.UI
{
    public class DCTitleScreen : ModSurfaceBackgroundStyle
    {
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for (int i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    if (fades[i] > 1f)
                    {
                        fades[i] = 1f;
                    }
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    if (fades[i] < 0f)
                    {
                        fades[i] = 0f;
                    }
                }
            }
        }
        private static readonly string TexPath = "DeadCells/Assets/Backgrounds/Empty";
        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            return BackgroundTextureLoader.GetBackgroundSlot(TexPath);
        }

        public override int ChooseFarTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot(TexPath);
        }

        public override int ChooseMiddleTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot(TexPath);
        }

        public override bool PreDrawCloseBackground(SpriteBatch spriteBatch)
        {
            return false;
        }
    }
}
