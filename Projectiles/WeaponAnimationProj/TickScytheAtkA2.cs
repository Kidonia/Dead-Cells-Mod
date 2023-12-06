using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using System.Collections.Generic;
using DeadCells.Utils;
using DeadCells.Projectiles.EffectProj;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class TickScytheAtkA2 : DC_WeaponAnimation
{
    public override string AnimName => "atkScytheA2";
    public override string fxName => "fxAtkScytheA2";
    public override int HitFrame => 35;
    public override int fxStartFrame => 34;
    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    private bool allTargetNoCrit;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(190, 76, 20, SurvivalDamage.Instance, 3f, slowBeginFrame : 8);
    }

    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(65f, 8f);
        PlayChargeSound(AssetsLoader.weapon_tickscythe_charge2);
        PlayWeaponSound(AssetsLoader.weapon_tickscythe_release1, HitFrame);
        CameraBump(8f, 4.2f, 20);
        Bump(2.6f);

        if (Projectile.frame == HitFrame + 2 && player.OnGround())
        {
            for (int i = 0; i < 32; i++)
                Dust.NewDustDirect((Projectile.velocity.X > 0 ? Projectile.Right - new Vector2(200, 10) : Projectile.Left + new Vector2(60, -10)), 210, 45, DustID.Dirt, Projectile.velocity.X * 2.8f, 0.8f, Scale : Main.rand.NextFloat(1f, 1.7f));
        }

        if (Projectile.frame == TotalFrame - 1 && !playerAtk.TickScytheCheckHit)
            playerAtk.TickScytheCanCrit = false;
    }
    public override void PostDraw(Color lightColor)
    {
        DrawWeaponTexture(WeaponDic, 6, -25, true, new(65, 169, 251), true);
        DrawfxTexture(fxDic, 6, -25);
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
            modifiers.CritDamage += 3.2f;
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
