using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Audio;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class PerfectHalberdA : DC_WeaponAnimation
{
    public override string AnimName => "perfectHalberdAtkA";
    public override string fxName => "fxPerfectHalberdAtkA";
    public override int HitFrame => 13;
    public override int fxStartFrame => 11;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(108, 128, 16, BrutalityDamage.Instance, 1.4f, slowBeginFrame: 6, slowEndFrame: 8);
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(28.4f, -21.8f);
        PlayChargeSound(AssetsLoader.weapon_broadsword_charge3);
        PlayWeaponSound(AssetsLoader.weapon_perfectsw_release1, 8);
        CameraBump(2.4f, 1f, 19);//屏幕震动
    }
    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 8, -25, true, new(255, 255, 255), true);
        DrawfxTexture(fxDic, 8, -25);
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (playerAtk.PerfectHalberdHurtTime == 0)//可暴击
        {
            modifiers.SetCrit();
            modifiers.CritDamage -= 0.15f;
        }
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(AssetsLoader.hit_broadsword);
        //该函数在击中敌人减少生命后执行
        if (target.life <= 0)//判断杀死敌人
            playerAtk.KingsSpearComboKillNum++;//每杀一个击杀计数加一
    }
}