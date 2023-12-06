﻿using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ID;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class AdeleScytheAtkA : DC_WeaponAnimation
{
    public override string AnimName => "AtkReaperToolA";
    public override string fxName => "FXAtkReaperToolA";
    public override int HitFrame => 9;
    public override int fxStartFrame => 0;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(142, 88, 18, BrutalityDamage.Instance, 0.16f, slowBeginFrame: 4);
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(58f, 14f);
        PlayWeaponSound(AssetsLoader.purpleDLC_scythe_AtkA_release, 9);
        PlayChargeSound(AssetsLoader.purpleDLC_scythe_charge);
        CameraBump(8.4f, 1f, 20);
        //CameraBump(15f, 0.2f, 90);次元斩屏幕振幅
    }
    public override void PostDraw(Color lightColor)//绘制快刀贴图
    {
        DrawWeaponTexture(WeaponDic, 12, -35, true, new(81, 192, 255), true);
        DrawfxTexture(fxDic, 12, -35);
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (!target.boss && target.life <= 0)
        {
            float k = target.type < NPCID.Count ? 1 : 2;
            Projectile.NewProjectile(Entity.GetSource_FromAI(), target.position, Vector2.Zero, ModContent.ProjectileType<SoulProj>(), Projectile.damage, 3f, player.whoAmI, target.type, k);
        }
        SoundEngine.PlaySound(AssetsLoader.purpleDLC_scythe_hit);
    }
}