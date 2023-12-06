using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria.Audio;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class BroadSwordAtkA : DC_WeaponAnimation
{
    public override string AnimName => "atkBroadSwordA";
    public override string fxName => "fxAtkBroadSwordA";
    public override int HitFrame => 9;
    public override int fxStartFrame => 5;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(168, 65, 16, BrutalityDamage.Instance, 1.4f, slowBeginFrame: 4, slowEndFrame : 6);
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(5.4f, -5.2f);
        PlayChargeSound(AssetsLoader.weapon_broadsword_charge1);
        PlayWeaponSound(AssetsLoader.weapon_broadsword_release1, 13);
        CameraBump(4.1f, 8.9f, 24);
        Bump(1.6f);
    }
    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 6, -30, true, new(118, 112, 217), true);
        DrawfxTexture(fxDic, 6, -30, true, new(0, 159, 255));
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(AssetsLoader.hit_broadsword);
    }
}

