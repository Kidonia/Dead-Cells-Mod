using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using DeadCells.Items.Weapons.SurvivalWeapon;
using Terraria.DataStructures;
using Terraria.ID;
using System.Collections.Generic;
using DeadCells.Utils;
using DeadCells.Projectiles.EffectProj;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class TickScytheAtkB2 : DC_WeaponAnimation
{
    public override string AnimName => "atkScytheB2";
    public override string fxName => "fxAtkScytheB1";
    public override int HitFrame => 18;
    public override int fxStartFrame => 17;
    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    private bool allTargetNoCrit;

    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(208, 175, 20, SurvivalDamage.Instance, 3f, slowBeginFrame: 9, slowEndFrame: 11);
    }

    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(94f, -55f);
        PlayChargeSound(AssetsLoader.weapon_tickscythe_charge3);
        PlayWeaponSound(AssetsLoader.weapon_tickscythe_release1, HitFrame + 2);
        CameraBump(9f, 5f, 16, -Vector2.UnitY);

        if (Projectile.frame == HitFrame + 1 && player.OnGround())
        {
            for (int i = 0; i < 54; i++)
                Dust.NewDustDirect((Projectile.velocity.X > 0 ? Projectile.Right - new Vector2(340, -130) : Projectile.Left + new Vector2(60, 130)), 270, 50, DustID.Dirt, Main.rand.NextFloat(-1.8f, 2f), -0.4f);
        }

        if (Projectile.frame == TotalFrame - 1 && !playerAtk.TickScytheCheckHit)
            playerAtk.TickScytheCanCrit = false;
    }
    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 0, -30, true, new(255, 212, 45), true);
        DrawfxTexture(fxDic, 0, -30);
    }

    public override void OnSpawn(IEntitySource source)
    {
        if (!playerAtk.TickScytheCanCrit)
            allTargetNoCrit = true;

        playerAtk.TickScytheCheckHit = false;
        base.OnSpawn(source);
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (playerAtk.TickScytheCanCrit && !allTargetNoCrit)
        {
            modifiers.SetCrit();
            modifiers.CritDamage += 0.55f;
        }
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(AssetsLoader.hit_broadsword);
        playerAtk.TickScytheCheckHit = true;
        playerAtk.TickScytheCanCrit = true;

        if (hit.Crit)//改为暴击时生成放大效果
        {
            IEntitySource source = player.GetSource_FromAI();
            Projectile.NewProjectile(source, target.Center, Vector2.Zero, ModContent.ProjectileType<RoundTwist>(), 0, 0);
        }
    }
}
