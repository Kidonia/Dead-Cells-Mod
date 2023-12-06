using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Audio;
using DeadCells.Utils;
using Terraria.ID;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class HeavyAxeAtkD : DC_WeaponAnimation
{
    public override string AnimName => "AtkOvenAxeSPIN";
    public override int HitFrame => 29;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        QuickSetDefault(208, 58, 16, BrutalityDamage.Instance, 1.4f, slowBeginFrame: 2, slowEndFrame: 19);
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(0, -2.8f);
        PlayChargeSound(AssetsLoader.weapon_axe_charge4);
        PlayWeaponSound(AssetsLoader.weapon_axe_release4, 35);
        CameraBump(2.4f, 1f, 19);//屏幕震动
        Bump(9.2f);
        if (Projectile.frame > 30 && Projectile.frame < 43)
        {
            player.immuneTime++;
            player.immune = true;
        }
        if (Projectile.frame == 39 && player.OnGround())
        {
            SoundEngine.PlaySound(AssetsLoader.unstable_platform_break);
            for (int i = 0; i < 18; i++)
                Dust.NewDustDirect(player.Bottom + new Vector2(-48 - Projectile.velocity.X * 220, -40), 100, 50, DustID.Dirt,
                    Projectile.velocity.X * Main.rand.NextFloat(-2.2f, -1.6f), Main.rand.NextFloat(-1.1f, -0.5f), Scale: Main.rand.NextFloat(1.2f, 1.5f));
        }
    }
    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 2, -17, true, new(219, 108, 64), true);
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        modifiers.SetCrit();
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(AssetsLoader.weapon_axe_hit);
    }
}
