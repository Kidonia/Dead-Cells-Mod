using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria.Audio;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class LowHealthAtkB : DC_WeaponAnimation
{
    public override string AnimName => "atkBackStabberB";
    public override string fxName => "fxAtkBackstabberB";
    public override int HitFrame => 5;
    public override int fxStartFrame => 6;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(52, 54, 20, BrutalityDamage.Instance, 0.12f);
    }

    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(11.8f, -2.4f);
        PlayWeaponSound(AssetsLoader.weapon_shortsw_release, 3);
        CameraBump(1.65f, 1f, 16);
    }
    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 18, -23, true, new(80, 187, 255), true);
        DrawfxTexture(fxDic, 18, -23, true, new(153, 225, 80));
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (player.statLife < player.statLifeMax2 / 2)
        {
            modifiers.SetCrit();
            modifiers.CritDamage -= 0.1f;
        }
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(AssetsLoader.hit_blade);
    }
}

