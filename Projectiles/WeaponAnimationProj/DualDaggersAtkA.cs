using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class DualDaggersAtkA : DC_WeaponAnimation
{
    public override string AnimName => "atkA";
    public override string fxName => "rFxAtkA";
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
        QuickSetDefault(76, 72, 16, BrutalityDamage.Instance, 1.4f, slowBeginFrame : 5);
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(19.4f, -5.2f);
        PlayChargeSound(AssetsLoader.weapon_dualdg_charge1);
        PlayWeaponSound(AssetsLoader.weapon_saber_release1, 5);
        CameraBump(3.2f, 1.5f, 22);
    }
    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 6, -30, true, new(255, 202, 0));
        DrawfxTexture(fxDic, 6, -30, true, new(255, 202, 0));
    }
}
