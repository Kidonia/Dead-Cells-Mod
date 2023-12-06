using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace DeadCells.Dusts
{
    public  class Square : ModDust
    {
        Texture2D itemtexture;
        public override void Unload()
        {
            itemtexture?.Dispose();
        }
        public override void OnSpawn(Dust dust)
        {

            {
                dust.noLight = true;
            }
        }
        public override bool Update(Dust dust)
        {
            float s = dust.scale;
            dust.rotation = 0f;
                if (s > 1f)
                {
                    s = 1f;
                }
                if (!dust.noLight)
                {
                    Lighting.AddLight(dust.position, dust.color.ToVector3() * s);
                }
                if (dust.noGravity)
                {
                    dust.velocity *= 0.93f;
                    if (dust.fadeIn == 0f)
                    {
                        dust.scale += 0.0025f;
                    }
                }
                else
                {
                    dust.velocity *= 0.95f;
                    dust.scale -= 0.0025f;
                }
                if (WorldGen.SolidTile(Framing.GetTileSafely(dust.position)) && dust.fadeIn == 0f && !dust.noGravity)
                {
                    dust.scale *= 0.9f;
                    dust.velocity *= 0.25f;
                }


            return true;
        }



    }
}
