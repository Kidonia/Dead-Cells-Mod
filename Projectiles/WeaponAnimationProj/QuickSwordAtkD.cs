using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using DeadCells.Items.Weapons.BrutalityWeapon;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class QuickSwordAtkD : DC_WeaponAnimation
{
    
    public override string AnimName => "atkSaberA";
    public override string fxName => "fxSaberA";
    public override int HitFrame => 8;
    public override int fxStartFrame => 6;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(132, 72, 20, BrutalityDamage.Instance, 1.5f, slowBeginFrame: 4, slowEndFrame: 6);
    }

    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(18f, 2.89f);
        PlayWeaponSound(AssetsLoader.weapon_kunai_release, 4);
        CameraBump(1.75f, 1f, 17);
    }
    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 12, -35, true, new(255, 99, 30));
        DrawfxTexture(fxDic, 12, -35, true, new(255, 61, 19));
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (playerAtk.QuickSwordAtkComboNum == 10)//十段连击后暴击
        {
            modifiers.SetCrit();
            modifiers.CritDamage -= 0.5f;
        }
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        playerAtk.QuickSwordComboBetweenTime = 200;
        playerAtk.QuickSwordAtkComboNum++;
        SoundEngine.PlaySound(AssetsLoader.hit_blade);
    }
}
