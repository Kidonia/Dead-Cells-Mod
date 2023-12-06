﻿using Microsoft.Xna.Framework;
using DeadCells.Common.Buffs;
using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using DeadCells.Items.Weapons.BrutalityWeapon;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.WorldBuilding;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class OilSwordAtkA : DC_WeaponAnimation
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
        QuickSetDefault(132, 72, 20, BrutalityDamage.Instance, 1.5f, slowBeginFrame: 4, slowEndFrame : 2);
    }

    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(18f, 2.89f);
        PlayWeaponSound(AssetsLoader.weapon_saber_release1, 4);
        CameraBump(2.4f, 1f, 20);
    }
    public override void PostDraw(Color lightColor)//绘制油刀贴图
    {

        if (playerAtk.OilSwordHitFireTargetTime > 0)//暴击绘制
        {
            DrawWeaponTexture(WeaponDic, 12, -35, true, new(132, 100, 186), true);
            DrawfxTexture(fxDic, 12, -35, true, new(194, 165, 255));
        }
        else//不暴击绘制
        {
            DrawWeaponTexture(WeaponDic, 12, -35, true, new(101, 79, 219));
            DrawfxTexture(fxDic, 12, -35, true, new(36, 43, 76));
        }
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (target.onFire || target.onFire2 || target.onFire3 || target.onFrostBurn || target.onFrostBurn2)
            playerAtk.OilSwordHitFireTargetTime = 600;
        if (playerAtk.OilSwordHitFireTargetTime > 0)
        {
            modifiers.SetCrit();
            modifiers.CritDamage -= 0.06f;
        }
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(ModContent.BuffType<Oil>(), 1500);
        SoundEngine.PlaySound(AssetsLoader.hit_blade);

    }
}
