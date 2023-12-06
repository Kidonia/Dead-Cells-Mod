using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using DeadCells.Items.Weapons.BrutalityWeapon;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class QuickSwordAtkC : DC_WeaponAnimation
{
    public override string AnimName => "atkKnifeB";
    public override string fxName => "fxKnifeB";
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
        QuickSetDefault(72, 74, 20, BrutalityDamage.Instance, 0.16f, slowBeginFrame : 2, slowEndFrame : 5);
        Projectile.CritChance = 0;
    }

    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(14f, -11.4f);
        PlayWeaponSound(AssetsLoader.weapon_kunai_release, 4);
        CameraBump(1.65f, 1f, 16);
    }
    public override void PostDraw(Color lightColor)//绘制快刀贴图
    {
        DrawWeaponTexture(WeaponDic, 8, -32, true, new(250, 255, 73));
        DrawfxTexture(fxDic, 8, -32, true, new(249, 190, 75));
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (playerAtk.QuickSwordAtkComboNum == 10)//十段连击后暴击
        {
            modifiers.SetCrit();
            modifiers.CritDamage -= 0.6f;
        }
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        playerAtk.QuickSwordComboBetweenTime = 200;
        playerAtk.QuickSwordAtkComboNum++;
        SoundEngine.PlaySound(AssetsLoader.hit_blade);
    }

}
