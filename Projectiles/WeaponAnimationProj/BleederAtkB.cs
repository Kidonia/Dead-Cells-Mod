﻿using Microsoft.Xna.Framework;
using DeadCells.Common.Buffs;
using DeadCells.Common.DamageClasses;
using DeadCells.Common.GlobalNPCs;
using DeadCells.Core;
using DeadCells.Items.Weapons.BrutalityWeapon;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace DeadCells.Projectiles.WeaponAnimationProj;
public class BleederAtkB : DC_WeaponAnimation
{
    public override string AnimName => "atkSaberB";
    public override string fxName => "fxSaberB";
    public override int HitFrame => 5;
    public override int fxStartFrame => 2;
    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(108, 70, 22, BrutalityDamage.Instance, knockBack : 1.7f, slowEndFrame : 4);
    }

    public override void AI()
    {
        DrawTheAnimationInAI(20f, -1.8f);
        PlayWeaponSound(AssetsLoader.weapon_saber_release2, 3);
        CameraBump(1.6f, 1f, 17);
        base.AI();
    }
    public override void PostDraw(Color lightColor)//绘制血刀贴图
    {
        DrawWeaponTexture(WeaponDic, 12, -35, true, new(255, 246, 173));
        DrawfxTexture(fxDic, 12, -35, true, new(163, 0, 22));
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        var drawicon = target.GetGlobalNPC<NPCDrawBuff>();
        drawicon.bleednum++;//流血层数加一
        target.AddBuff(ModContent.BuffType<Bleed>(), 720);
        SoundEngine.PlaySound(AssetsLoader.hit_blade);
    }
}
