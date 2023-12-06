﻿using Microsoft.Xna.Framework.Input;
using DeadCells.Common.Buffs;
using DeadCells.Core;
using DeadCells.Utils;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Terraria.DataStructures;
using DeadCells.Common.DamageClasses;
using Terraria.ID;
using DeadCells.Projectiles.EffectProj;

namespace DeadCells.Common.Players;

public class SmashDown : ModPlayer
{
    public static ModKeybind SmashKey { get; private set; } = null;

    private SlotId fallsound;

    public bool IsSmashing { get; set; }//玩家是否在下砸

    private int WasOnGroundTime;

    private int AddHurtCoolDown;

    private bool shortsmash;

    private bool normalsmash;

    public bool justRolled;

    private int ShortSmashTime;

    private int CoolDownTime;

    private readonly int stun = ModContent.BuffType<Stun>();
    public bool CanMove()//自己写的，判断玩家能不能动
    {
        if (Player.CCed || Player.HasBuff(stun) || Player.tongued)
        {
            return false;
        }
        return true;
    }
    public override void Load()
    {
        SmashKey = KeybindLoader.RegisterKeybind(Mod, "Smash Down", Keys.Space);//默认空格下砸
    }


    public override bool PreItemCheck()
    {
        UpdateSmashing();
        
        if (IsSmashing)
        {
            return false;
        }
        return true;
    }
    public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
    {
        AddHurtCoolDown = 7;//玩家受伤后禁止下砸时间
        base.OnHitByNPC(npc, hurtInfo);
    }
    public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
    {
        AddHurtCoolDown = 6;//玩家受伤后禁止下砸时间
        base.OnHitByProjectile(proj, hurtInfo);
    }


    public override void PreUpdate()
    {
        if (CanSmach())
            CanUseJumpHelperAccessary(false);//按下键时取消饰品效果。


        if (WasOnGroundTime > 0)
            WasOnGroundTime--;

        if (AddHurtCoolDown > 0)
            AddHurtCoolDown--;

        if (CoolDownTime == 0)
        {
            IsSmashing = false;
            normalsmash = false;
        }

        if (IsSmashing)
        {
            if (!Main.dedServ)
            {
                Player.GetModPlayer<PlayerTrailEffects>().ForceTrailEffect(2);
            }

            Player.maxFallSpeed = 15.6f;
            Player.fallStart = (int)(Player.position.Y / 16f);

            CanUseJumpHelperAccessary(false);

            if (CoolDownTime > 0)
                CoolDownTime--;

            if (ShortSmashTime > 0)
            {
                ShortSmashTime--;
                shortsmash = true;
                normalsmash = false;
            }
            else
            {
                shortsmash = false;
                normalsmash = true;
            }


        }
        base.PreUpdate();
    }

    private void UpdateSmashing()
    {

        if(Player.mount.Active || Player.pulley)//玩家在绳子上
        {
            normalsmash = false;
            shortsmash = false;
            CoolDownTime = 0;//下砸冷却清零
            ShortSmashTime = 0;//短距离下砸判断时间清零
            WasOnGroundTime = 15;//离开地面时间恢复从0 开始
            IsSmashing = false;//不在下砸
            if (SoundEngine.TryGetActiveSound(fallsound, out ActiveSound result))
                result.Stop();//停止放送下砸声音
            return;
        }
        if(Player.OnGround() || justRolled)//玩家落地刷新，或，翻滚刷新
        {

            Player.wingTime = Player.wingTimeMax;
            Player.rocketTime = Player.rocketTimeMax;
            CanUseJumpHelperAccessary(true);


            CoolDownTime = 0;//下砸冷却清零
            ShortSmashTime = 0;//短距离下砸判断时间清零
            WasOnGroundTime = 15;//离开地面时间恢复从0 开始
            IsSmashing = false;//不在下砸




            if(normalsmash)//普通下砸
            {
                normalsmash = false;//执行一次即取消
                if(SoundEngine.TryGetActiveSound(fallsound, out ActiveSound result))
                result.Stop();//停止放送下砸声音

                if (!justRolled)//如果不是翻滚取消下砸
                {
                    SoundEngine.PlaySound(AssetsLoader.stomp_char2);//下砸落地声音
                    //左右两侧造成伤害区域
                    IEntitySource source = Player.GetSource_FromAI();
                    int dmg = CalculateDmg(3.1f);
                    Projectile.NewProjectile(source, Player.Bottom - new Vector2(-45f, 21f), Vector2.UnitX, ModContent.ProjectileType<SmashDownArea>(), dmg, 3.8f, Player.whoAmI, 90, 89);//生成伤害区域
                    Projectile.NewProjectile(source, Player.Bottom - new Vector2(45f, 21f), -Vector2.UnitX, ModContent.ProjectileType<SmashDownArea>(), dmg, 3.8f, Player.whoAmI, 90, 89);//生成伤害区域

                    for (int i = 0; i < 22; i++)//生成粒子，可改
                    {
                        Dust.NewDust(Player.Bottom + new Vector2(-30, -5), 60, 6, DustID.Dirt, 0, 0, 0, default, Main.rand.NextFloat(0.9f, 1.5f));
                    }

                    var bump = new PunchCameraModifier(Player.Center, Vector2.UnitY, 7.2f, 4f, 24);
                    Main.instance.CameraModifiers.Add(bump);//生成屏幕震动


                }
                Player.GiveImmuneTimeForCollisionAttack(30);//下砸后无敌时间
            }

            if (shortsmash)//短距离下砸，同上，效果较弱。
            {
                shortsmash = false;
                if (SoundEngine.TryGetActiveSound(fallsound, out ActiveSound result))
                    result.Stop();

                if (!justRolled)
                {
                    SoundEngine.PlaySound(AssetsLoader.stomp_char1);
                    //左右两侧造成伤害区域
                    IEntitySource source = Player.GetSource_FromAI();
                    int dmg = CalculateDmg(1.6f);
                    Projectile.NewProjectile(source, Player.Bottom - new Vector2(-25, 17f), Vector2.UnitX, ModContent.ProjectileType<SmashDownArea>(), dmg, 1.8f, Player.whoAmI, 50, 58);//生成伤害区域
                    Projectile.NewProjectile(source, Player.Bottom - new Vector2(25, 17f), -Vector2.UnitX, ModContent.ProjectileType<SmashDownArea>(), dmg, 1.8f, Player.whoAmI, 50, 58);//生成伤害区域

                    for (int i = 0; i < 6; i++)//生成粒子，可改
                    {
                        Dust.NewDust(Player.Bottom + new Vector2(-12, -3), 24, 6, DustID.Dirt, 0, 0, 0, default, Main.rand.NextFloat(0.9f, 1.2f));
                    }

                    var bump = new PunchCameraModifier(Player.Center, Vector2.UnitY, 4.2f, 3.2f, 24);
                    Main.instance.CameraModifiers.Add(bump);



                }
                Player.GiveImmuneTimeForCollisionAttack(20);//下砸后无敌时间
            }
            justRolled = false;
            return;
        }
        if (!IsSmashing)//如果 玩家没在下砸
        {
            bool isLocal = Player.IsLocal();//检测是否为本地玩家
            bool moveable = CanMove();//检测玩家能否移动
            if (!Player.OnGround() && Player.controlDown && !Player.pulley)
            {
                if (CanSmach() && SmashKey.JustPressed)
                {

                    fallsound = SoundEngine.PlaySound(AssetsLoader.stomp_air);

                    CoolDownTime = 75;
                    ShortSmashTime = 15;

                    Player.velocity.Y += 7.4f;

                    IsSmashing = true;

                }

            }
            return;//结束
        }

    }
    private int CalculateDmg(float basicdmg)
    {
        return (int)(Player.GetTotalDamage<BrutalityDamage>().ApplyTo(basicdmg) + Player.GetTotalDamage<TacticsDamage>().ApplyTo(basicdmg) + Player.GetTotalDamage<SurvivalDamage>().ApplyTo(basicdmg));
    }

    private void CanUseJumpHelperAccessary(bool able)
    {
        Player.GetJumpState(ExtraJump.BlizzardInABottle).Available = able;
        Player.GetJumpState(ExtraJump.CloudInABottle).Available = able;
        Player.GetJumpState(ExtraJump.FartInAJar).Available = able;
        Player.GetJumpState(ExtraJump.TsunamiInABottle).Available = able;
        Player.GetJumpState(ExtraJump.SandstormInABottle).Available = able;
        

        if (able)
        {
            Player.wingTime = Player.wingTimeMax;
            Player.rocketTime = Player.rocketTimeMax;
        }    
        else
        {
            Player.wingTime = 0;
            Player.rocketTime = 0;
        }

    }

    private bool CanSmach()
    {
        return !Player.OnGround() &&!Player.mount.Active&& Player.controlDown && !Player.pulley && AddHurtCoolDown == 0 && WasOnGroundTime == 0 && Player.IsLocal() && CanMove()  && CoolDownTime == 0 && (!Player.mouseInterface || !Main.playerInventory);
    }




    public override void Unload()
    {
        SmashKey = null;
        base.Unload();
    }
}
