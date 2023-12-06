using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace DeadCells.Projectiles.WeaponAnimationProj;
public class DualDaggersAtkC : DC_WeaponAnimation
{
    public override string AnimName => "atkC";
    public override string fxName => "rFxAtkC";
    public override int HitFrame => 8;
    public override int fxStartFrame => 4;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(91, 74, 20, BrutalityDamage.Instance, 1.8f, slowBeginFrame: 5, slowEndFrame: 5);
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(24f, 0.8f);
        PlayChargeSound(AssetsLoader.weapon_dualdg_charge3);
        PlayWeaponSound(AssetsLoader.weapon_dualdg_release3, 6);
        CameraBump(3.2f, 1.2f, 19);
        Bump(2.4f);
    }
    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 6, -30, true, new(255, 101, 0));
        DrawfxTexture(fxDic, 6, -30, true, new(255, 101, 0));
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        modifiers.SetCrit();
    }
}
