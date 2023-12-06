using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class EvilSwordAtkA : DC_WeaponAnimation
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
        QuickSetDefault(128, 68, 20, BrutalityDamage.Instance, 1.4f);
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(18f, 2.89f);
        PlayWeaponSound(AssetsLoader.weapon_saber_release1, 4);
        CameraBump(2.4f, 1f, 19);
    }
    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 12, -35, true, new(255, 79, 0));
        DrawfxTexture(fxDic, 12, -35, true, new(255, 79, 0));
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        modifiers.SetCrit();
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        
        SoundEngine.PlaySound(AssetsLoader.hit_blade);
    }
}
