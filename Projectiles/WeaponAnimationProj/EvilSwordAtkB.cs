using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using DeadCells.Items.Weapons.BrutalityWeapon;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class EvilSwordAtkB : DC_WeaponAnimation
{
    public override string AnimName => "atkSaberB";
    public override string fxName => "fxSaberB";
    public override int HitFrame => 6;
    public override int fxStartFrame => 6;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(108, 70, 22, BrutalityDamage.Instance, knockBack: 1.7f, slowBeginFrame : 4, slowEndFrame: 2);
    }

    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(20f, -1.8f);
        PlayWeaponSound(AssetsLoader.weapon_saber_release2, 3);
        CameraBump(1.6f, 1f, 17);
    }
    public override void PostDraw(Color lightColor)//绘制血刀贴图
    {
        DrawWeaponTexture(WeaponDic, 12, -35, true, new(255, 79, 0));
        DrawfxTexture(fxDic, 12, -35, true, new(255, 79, 0));
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        modifiers.SetCrit();
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(AssetsLoader.hit_blade);
    }
}
