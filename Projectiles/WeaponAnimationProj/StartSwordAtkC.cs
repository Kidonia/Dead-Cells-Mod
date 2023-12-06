using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria;
using Terraria.Audio;
using System.Collections.Generic;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class StartSwordAtkC : DC_WeaponAnimation
{
    public override string AnimName => "atkSaberA";
    public override string fxName => "fxSaberA";
    public override int HitFrame => 7;
    public override int fxStartFrame => 6;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(132, 72, 20, BrutalityDamage.Instance, 1.5f, slowBeginFrame: 3, slowEndFrame : 2);
        Projectile.CritChance = 0;
    }

    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(18f, 2.89f);
        PlayWeaponSound(AssetsLoader.weapon_shortsw_release, 4);
        CameraBump(1.7f, 1f, 16);
    }
    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 12, -35, true, new(187, 198, 231), true);
        DrawfxTexture(fxDic, 12, -35, true, new(250, 255, 73));
        
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(AssetsLoader.hit_blade);
    }
}
