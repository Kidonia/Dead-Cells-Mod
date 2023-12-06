﻿using Microsoft.Xna.Framework;
using DeadCells.Common.Buffs;
using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class BleedCritAtkA : DC_WeaponAnimation
{
    public override string AnimName => "atkKnifeB";
    public override string fxName => "fxKnifeB";
    public override int HitFrame => 5;
    public override int fxStartFrame => 4;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(72, 74, 18, BrutalityDamage.Instance, 0.16f, slowBeginFrame: 2, slowEndFrame: 4);
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(14f, -11.4f);
        PlayWeaponSound(AssetsLoader.weapon_saber_release1, 4);
        CameraBump(1.65f, 1f, 16);
    }
    public override void PostDraw(Color lightColor)//绘制快刀贴图
    {
        DrawWeaponTexture(WeaponDic, 12, -35, true, new(255, 227, 77));
        DrawfxTexture(fxDic, 12, -35, true, new(5, 7, 7));
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (target.HasBuff(ModContent.BuffType<Bleed>()) || target.poisoned)
        {
            modifiers.SetCrit();
            modifiers.CritDamage += 0.2f;
        }
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(AssetsLoader.hit_blade);
    }
}
