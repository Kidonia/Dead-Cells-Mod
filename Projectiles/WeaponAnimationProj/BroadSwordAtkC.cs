using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria.Audio;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class BroadSwordAtkC : DC_WeaponAnimation
{
    public override string AnimName => "atkBroadSwordC";
    public override string fxName => "fxAtkBroadSwordC";
    public override int HitFrame => 16;
    public override int fxStartFrame => 17;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(172, 140, 18, BrutalityDamage.Instance, 1.4f, slowBeginFrame: 10, slowEndFrame: 10);
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(8.3f, -22.2f);
        PlayChargeSound(AssetsLoader.weapon_broadsword_charge3);
        PlayWeaponSound(AssetsLoader.weapon_broadsword_release3, 11);
        CameraBump(5.8f, 10.8f, 24, - Vector2.UnitY);
        Bump(1.6f);

        if (Projectile.frame == HitFrame)
        {
            for (int i = 0; i < 18; i++)
                Dust.NewDustDirect((Projectile.velocity.X > 0 ? Projectile.Right - new Vector2(36, -48) : Projectile.Left + new Vector2(22, 48)), 70, 60, DustID.Dirt, Projectile.velocity.X, -3.8f, Scale: Main.rand.NextFloat(1.3f, 2.4f));
        }
    }
    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 6, -30, true, new(255, 157, 0), true);
        DrawfxTexture(fxDic, 6, -30, true, new(255, 249, 180));
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        modifiers.SetCrit();
        modifiers.CritDamage += 1f;
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(AssetsLoader.hit_broadsword);
    }
}

