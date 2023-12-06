using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DeadCells.Projectiles.WeaponAnimationProj;
public class DualDaggersAtkB : DC_WeaponAnimation
{
    public override string AnimName => "atkB";
    public override string fxName => "rFxAtkB";
    public override int HitFrame => 7;
    public override int fxStartFrame => 5;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(90, 62, 18, BrutalityDamage.Instance, 1.2f, slowBeginFrame: 2, slowEndFrame : 1);
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(24.4f, -4.2f);
        PlayChargeSound(AssetsLoader.weapon_dualdg_charge2);
        PlayWeaponSound(AssetsLoader.weapon_dualdg_release2, 6);
        CameraBump(3.2f, 1.5f, 22);
        Bump(1.7f);
    }
    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 6, -30, true, new(255, 202, 0));
        DrawfxTexture(fxDic, 6, -30, true, new(255, 202, 0));
    }

}
