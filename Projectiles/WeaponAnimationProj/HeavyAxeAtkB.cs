﻿using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Audio;
using DeadCells.Utils;
using Terraria.ID;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class HeavyAxeAtkB : DC_WeaponAnimation
{
    public override string AnimName => "AtkOvenAxeB";
    public override int HitFrame => 27;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        QuickSetDefault(120, 74, 16, BrutalityDamage.Instance, 1.4f,slowBeginFrame : 1, slowEndFrame: 28);
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(52.2f, -8.8f);
        PlayChargeSound(AssetsLoader.weapon_axe_charge2);
        PlayWeaponSound(AssetsLoader.weapon_axe_release2, 26);
        CameraBump(2.4f, 1f, 19);//屏幕震动
        Bump(1.6f);
        if (Projectile.frame == 28 && player.OnGround())
        {
            SoundEngine.PlaySound(AssetsLoader.unstable_platform_break);
            for (int i = 0; i < 10; i++)
                Dust.NewDustDirect((Projectile.velocity.X > 0 ? Projectile.BottomRight + new Vector2(-270, -100) : Projectile.BottomLeft + new Vector2(205, -100)), 40, 50, DustID.Dirt,
                    Projectile.velocity.X * Main.rand.NextFloat(-2f, -0.6f), Main.rand.NextFloat(-1.2f, -3.8f), Scale: Main.rand.NextFloat(1.2f, 1.58f));
        }
    }
    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 2, -17, true, new(80, 51, 162), true);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(AssetsLoader.weapon_axe_hit);
    }
}