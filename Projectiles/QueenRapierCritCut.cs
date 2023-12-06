using DeadCells.Core;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using DeadCells.Common.DamageClasses;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace DeadCells.Projectiles;

public class QueenRapierCritCut : ModProjectile
{
    public override string Texture => AssetsLoader.TransparentImg;
    public Player player => Main.player[Projectile.owner];

    private Dictionary<int, DCAnimPic> fxDic = new();

    private int fxFrame = 0;

    public override void SetDefaults()
    {
        fxDic = AssetsLoader.fxAtlas["fxQueenRapierCut"];
        Projectile.width = 420;
        Projectile.height = 14;
        Projectile.DamageType = BrutalityDamage.Instance;
        Projectile.timeLeft = 40;
        Projectile.tileCollide = false;
        Projectile.friendly = true;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
        Projectile.penetrate = -1;
    }
    public override bool ShouldUpdatePosition()
    {
        return false;
    }
    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = Projectile.ai[1];
    }
    public override void AI()
    {
        if (fxFrame < fxDic.Count - 1)
            fxFrame++;

        if (Projectile.timeLeft > 36)
            Projectile.ai[0] += 0.25f;
        if (Projectile.timeLeft < 16)
            Projectile.ai[0] -= 0.067f;

    }
    public override void PostDraw(Color lightColor)
    {
        SpriteBatch sb = Main.spriteBatch;
        DrawfxEffect(fxDic, sb);
    }

    public void DrawfxEffect(Dictionary<int, DCAnimPic> fxDic, SpriteBatch sb)
    {

        Rectangle rectangle = new(fxDic[fxFrame].x, fxDic[fxFrame].y,
                                                    fxDic[fxFrame].width, fxDic[fxFrame].height);
        Vector2 vector = new Vector2(fxDic[fxFrame].originalWidth / 2 //参考解包图片如果在大图里是如何绘制的
                                                        - fxDic[fxFrame].offsetX,

                                                         fxDic[fxFrame].originalHeight / 2
                                                         - fxDic[fxFrame].offsetY);


        sb.End();
        sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);

        sb.Draw(AssetsLoader.ChooseCorrectAnimPic(fxDic[fxFrame].index, fxWeapon: true),
                Projectile.Center - Main.screenPosition,
                rectangle,
                Color.White,
                Projectile.rotation,
                vector,
                Projectile.scale,
                0,
                0f);

        sb.End();
        sb.Begin();

    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        modifiers.SetCrit();
    }
}
