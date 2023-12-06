using DeadCells.Core;
using DeadCells.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DeadCells.Common.Players;

public class ShootHead : ModPlayer
{
    public static ModKeybind HeadShootKey { get; private set; } = null;
    public bool canplayermove;
    public bool headflying;
    public bool attaching;
    public bool shouldgetback = false;
    public bool playcomebacksound = true;
    public bool reactiveReturn;
    public int jumpCount = 0;
    public int frozenTime = 16;
    private int currentDir;
    public int cooldown;
    //想要制作连续运动轨迹的屏幕，就必须预存一个坐标，再每帧把这个坐标进行改动，最后赋值到屏幕坐标
    public override void Load()
    {
        HeadShootKey = KeybindLoader.RegisterKeybind(Mod, "Leave / get back in body", Keys.V);//默认V飞头
    }
    public override void ResetEffects()//这个函数是每帧最先执行的，用来重置一些东西
    {
        base.ResetEffects();
    }

    public override void ModifyScreenPosition()
    {
        foreach (Projectile projectile in Main.projectile)
        {
            if (!reactiveReturn && !attaching && projectile.type == ModContent.ProjectileType<Homunculus>() && projectile.active)
            {
                Main.screenPosition = projectile.Center - Main.ScreenSize.ToVector2() / 2;
            }
        }
    }
    public override void PreUpdate()
    {
        /*
        foreach(Point point in Player.TouchedTiles)
        {
            Main.NewText(point);
        }
        */
        //Main.NewText(frozenTime);
        if ((frozenTime > 0 && frozenTime < 16) || headflying)
        {
            canplayermove = false;
            Player.stairFall = false;
        }
        else canplayermove = true;

        if (Player.ownedProjectileCounts[ModContent.ProjectileType<Homunculus>()] < 1 && HeadShootKey.JustPressed)
        {
            currentDir = Player.direction;
            Player.velocity *= 0;
            Vector2 velocity = new(currentDir * 12f, 0f);
            IEntitySource source = Player.GetSource_FromAI();
            SoundEngine.PlaySound(AssetsLoader.homunculus_release);
            Projectile.NewProjectile(source, Player.Top, velocity, ModContent.ProjectileType<Homunculus>(), Damage : 8, 0f, Player.whoAmI);
        }

        foreach (Projectile projectile in Main.projectile)
        {
            if (projectile.type == ModContent.ProjectileType<Homunculus>() && projectile.active)
            {
                if(Player.mount.Active && !attaching)
                {
                    if(Player.controlJump)
                    {
                        Player.velocity.Y = 0f;
                        Player.velocity.Y *= 0f;
                    }
                }

                if (frozenTime > 0 || attaching)
                {
                    headflying = false;
                    frozenTime--;
                }
                else
                    headflying = true;

                if (!shouldgetback && !attaching && headflying && HeadShootKey.JustPressed)
                {
                    shouldgetback = true;
                    if(attaching)
                        reactiveReturn = true;
                    SoundEngine.PlaySound(AssetsLoader.homunculus_comeback);
                }
            }
        }
        if (shouldgetback)
        {
            headflying = false;
            attaching = false;
        }
        if (headflying)
        {
            //Player.direction = dir;
            Player.runAcceleration = 0f;
            Player.maxRunSpeed = 0f;
            Player.jumpSpeed = 0f;
            Player.jumpSpeedBoost = 0f;
            if (Player.velocity.X != 0)
            {
                Player.velocity.X *= 0;
            }

        }



        if (!Player.controlDown && Player.justJumped && !Player.mount.Active && canplayermove)
        {
            SoundEngine.PlaySound(AssetsLoader.jump);
        }
        //Main.NewText(Player.stairFall);
    }
    public override void PostUpdate()
    {
        if (!canplayermove)
        {
            Player.stairFall = false;
            Player.direction = currentDir;
        }
    }
    public override void SetControls()
    {
        if(headflying || (frozenTime > 0 && frozenTime < 16))
        {
            Player.stairFall = false;
            Player.controlMount = false;
            Player.controlSmart = false;
            Player.controlTorch = false;
            Player.controlHook = false;
            Player.controlThrow = false;
            if(Player.mount.Active)
            {
                Player.controlUp = false;
                Player.controlDown = false;
            }
        }

    }
}
