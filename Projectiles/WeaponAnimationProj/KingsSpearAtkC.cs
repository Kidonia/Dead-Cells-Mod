using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Audio;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class KingsSpearAtkC : DC_WeaponAnimation
{
    public override string AnimName => "halberdAtkC";
    public override string fxName => "halberdFxAtkC";
    public override int HitFrame => 9;
    public override int fxStartFrame => 8;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;

    public override int OnionSkinFrame => 9;
    public override float OnionSkinOffX => 12.2f;

    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(188, 65, 16, BrutalityDamage.Instance, 1.4f, slowBeginFrame: 8, slowEndFrame: 9);
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(68f, -5.2f);
        PlayChargeSound(AssetsLoader.weapon_broadsword_charge3);
        PlayWeaponSound(AssetsLoader.weapon_doublelance_release3, 8);
        CameraBump(2.6f, 1f, 19);//屏幕震动
        Bump(3f);
    }
    public override void PostDraw(Color lightColor)
    {
        if (playerAtk.KingsSpearCritTime > 0)
            DrawWeaponTexture(WeaponDic, 6, -30, true, new(217, 67, 0), true);
        else
            DrawWeaponTexture(WeaponDic, 6, -30, true, new(137, 151, 176), true);
        DrawfxTexture(fxDic, 6, -30, true, new(244, 255, 73));
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (playerAtk.KingsSpearCritTime > 0)//可暴击
            modifiers.SetCrit();
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(AssetsLoader.hit_blade);
        //该函数在击中敌人减少生命后执行
        if (target.life <= 0)//判断杀死敌人
            playerAtk.KingsSpearComboKillNum++;//每杀一个击杀计数加一
    }
}