using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using DeadCells.Core;

namespace DeadCells.Dusts;

public class LineWhite : ModDust
{
    private double rotation;
    public Vector2 SpawnCenter;
    public override string Texture => AssetsLoader.TransparentImg;
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("环");
    }
    

    public override void OnSpawn(Dust dust)
    {

        Player player = Main.LocalPlayer;
        dust.rotation = dust.position.ToRotation() + (float)Math.PI / 2f;

        rotation = dust.position.ToRotation();
        if (rotation < 0)
        {
            rotation += MathHelper.TwoPi;
        }
        dust.position = player.Center + Terraria.Utils.RotatedBy(new Vector2(146f, 0f), rotation);
        dust.scale *= Main.rand.NextFloat(0.9f, 1.1f);
        base.OnSpawn(dust);
    }

    public override bool PreDraw(Dust dust)
    {
        return true;
    }





}
