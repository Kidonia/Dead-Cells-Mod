using DeadCells.Common.Buffs;
using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class BleedCritAtkB : DC_WeaponAnimation
{

    public override string AnimName => "atkKnifeA";
    public override string fxName => "fxKnifeA";
    public override int HitFrame => 5;
    public override int fxStartFrame => 5;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(70, 72, 16, BrutalityDamage.Instance, 0.12f);
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(13.8f, -11.2f);
        PlayWeaponSound(AssetsLoader.weapon_saber_release1, 4);
        CameraBump(1.38f, 1f, 16);
    }
    public override void PostDraw(Color lightColor)//绘制快刀贴图
    {
        DrawWeaponTexture(WeaponDic, 8, -32, true, new(255, 227, 77));
        DrawfxTexture(fxDic, 8, -32, true, new(5, 7, 7));
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (target.HasBuff(ModContent.BuffType<Bleed>()) || target.poisoned)
        {
            modifiers.SetCrit();
            modifiers.CritDamage += 0.1f;
        }
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(AssetsLoader.hit_blade);
    }


}
