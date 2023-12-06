using Microsoft.Xna.Framework;
using DeadCells.Common.DamageClasses;
using DeadCells.Core;
using Terraria;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ID;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class AdeleScytheAtkD : DC_WeaponAnimation
{
    public override string AnimName => "AtkReaperToolD";
    public override string fxName => "FXAtkReaperToolD";
    public override int HitFrame => 5;
    public override int fxStartFrame => 0;

    private Dictionary<int, DCAnimPic> WeaponDic = new();
    private Dictionary<int, DCAnimPic> fxDic = new();
    public override int TotalFrame => WeaponDic.Count;
    public override int fxFrames => fxDic.Count;
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];
        fxDic = AssetsLoader.fxAtlas[fxName];
        QuickSetDefault(194, 158, 18, BrutalityDamage.Instance, 0.16f, slowBeginFrame: 8, slowEndFrame: 10);
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 2;
    }
    public override void AI()
    {
        base.AI();
        DrawTheAnimationInAI(-13.2f, -14f);
        PlayWeaponSound(AssetsLoader.purpleDLC_scythe_AtkD_release, 5);
        PlayChargeSound(AssetsLoader.purpleDLC_scythe_charge);
        CameraBump(2.8f, 4.2f, 18, -Vector2.UnitY);
    }
    public override void PostDraw(Color lightColor)//绘制快刀贴图
    {
        DrawWeaponTexture(WeaponDic, 12, -35, true, new(81, 192, 255), true);
        DrawfxTexture(fxDic, 12, -35);
    }
    public override bool? CanDamage()
    {
        return Projectile.frame == HitFrame || Projectile.frame == 8 || Projectile.frame == 11;
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (!target.boss && target.life <= 0)
        {
            float k = target.type < NPCID.Count ? 1 : 2;
            Projectile.NewProjectile(Entity.GetSource_FromAI(), target.position, Vector2.Zero, ModContent.ProjectileType<SoulProj>(), Projectile.damage, 3f, player.whoAmI, target.type, k);
        }
        if (Projectile.frame < 8)
            SoundEngine.PlaySound(AssetsLoader.purpleDLC_scythe_hit);
    }
}
