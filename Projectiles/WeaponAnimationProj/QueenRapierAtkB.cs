using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria.Audio;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class QueenRapierAtkB : DC_WeaponAnimation
{
    public override string AnimName => "AtkQueenRapierB";
    public override int HitFrame => 9;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        QuickSetDefault(118, 56, 10, BrutalityDamage.Instance, 1.4f, slowBeginFrame: 6, slowEndFrame: 4);
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(42f, -10f);
        PlayWeaponSound(AssetsLoader.weapon_queensw_release1, 5);
        CameraBump(2.4f, 1f, 19);
        if (Projectile.frame == HitFrame)
        {
            Projectile.NewProjectile(Entity.GetSource_FromAI(), player.Center + new Vector2(Projectile.velocity.X * 130, -36), Projectile.velocity, ModContent.ProjectileType<QueenRapierCut>(), 0, 0, player.whoAmI, MathHelper.ToRadians(-Projectile.velocity.X * 22.5f));
        }
    }

    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 8, -30, true, new(161, 71, 0), true);
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(AssetsLoader.hit_blade);
    }
}