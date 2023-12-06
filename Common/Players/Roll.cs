using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using DeadCells.Utils;
using Terraria.ID;
using DeadCells.Common.Buffs;
using DeadCells.Core;
using Terraria.DataStructures;
using DeadCells.Projectiles;

namespace DeadCells.Common.Players;

public class Roll : ModPlayer
{

    public static readonly SoundStyle DodgerollSound = new SoundStyle("DeadCells/Assets/Sounds/PlayerSounds/roll")
    {
        Volume = 0.65f,
        PitchVariance = 0.2f
    };

    public static readonly SoundStyle FailureSound = new SoundStyle("DeadCells/Assets/Sounds/PlayerSounds/roll")
    {
        Volume = 0.65f,
        PitchVariance = 0.2f
    };

    public Timer DodgerollTirednessTimer;

    public Timer DodgerollCooldownTimer;

    public Timer NoDodgerollsTimer;

    public Timer DodgeAttemptTimer;

    public float? ForcedItemRotation;
    public int ForcedDirection { get; set; }
    public Vector2 MouseWorld { get; set; }

    public bool ForceDodgeroll;

    public sbyte WantedDodgerollDirection;//想要翻滚的方向

    public static ModKeybind DodgerollKey { get; private set; } = null;


    public static float DodgeTimeMax => 0.37f;//可翻滚最大时间

    public static uint DefaultDodgeTirednessTime => (uint)66f;

    public static uint DefaultDodgeCooldownTime => DefaultDodgeTirednessTime;

    public static int DefaultDodgeMaxCharges => 1;//默认最大可翻滚次数

    public int MaxCharges { get; set; }//最大次数

    public int CurrentCharges { get; set; }

    public bool IsDodging { get; private set; }//玩家是否在翻滚

    private readonly int stun = ModContent.BuffType<Stun>();
    public bool CanMove()//自己写的，判断玩家能不能动
    {
        if (Player.CCed || 
            Player.HasBuff(stun) || 
            Player.tongued ||
            (Player.TryGetModPlayer<ShootHead>(out var shoothead) && !shoothead.canplayermove) ||
            (Player.TryGetModPlayer<PlayerDraw>(out var teleport) && teleport.teleporting)
            )
        {
            return false;
        }
        return true;
    }
    public float DodgeStartRotation { get; private set; }//翻滚的起始倾斜程度

    public float DodgeItemRotation { get; private set; }//暂存的 用于传递翻滚前后的Player.ItemRotation

    public float DodgeTime { get; private set; }//翻滚时间

    public sbyte DodgeDirection { get; private set; }//翻滚方向

    public sbyte DodgeDirectionVisual { get; private set; }//视觉上的翻滚方向

    public override void Load()
    {
        DodgerollKey = KeybindLoader.RegisterKeybind(Mod, "Roll", Keys.LeftShift);//默认左shift翻滚

        Terraria.IL_Player.Update_NPCCollision += PlayerNpcCollisionInjection;//添加IL钩子，检测玩家与NPC碰撞
        Terraria.IL_Projectile.Damage += ProjectileDamageInjection;//添加IL钩子
    }

    public override void Initialize()//初始化
    {
        int currentCharges = MaxCharges = DefaultDodgeMaxCharges;//默认可翻滚最大次数 赋值给 最大可翻滚次数 再赋值给 当前可翻滚次数
        CurrentCharges = currentCharges;
    }

    public override bool PreItemCheck()//每帧监测，且翻滚时不能用手持弹幕
    {
        SetDirection(false);
        UpdateCooldowns();
        UpdateDodging();
        if (IsDodging && Player.HeldItem.type == ItemID.Umbrella)
        {
            return false;
        }
        return true;
    }

    public override bool CanUseItem(Item item)
    {
        return !IsDodging;//翻滚时不能使用物品
    }
    public override void PreUpdate()
    {
        SetDirection(true);
        base.PreUpdate();
    }
    public override void PostUpdate()
    {
        SetDirection(false);
        if (ForcedItemRotation.HasValue)
        {
            Player.itemRotation = ForcedItemRotation.Value;
            ForcedItemRotation = null;
        }
        base.PostUpdate();
    }
    //正数 最低尝试计时器， 八位有符号整数， force默认否
    public void QueueDodgeroll(uint minAttemptTimer, sbyte direction, bool force = false)
    {
        if (force)
        {
            DodgerollCooldownTimer = 0;
            CurrentCharges = Math.Max(CurrentCharges, 1);
        }
        DodgeAttemptTimer.Set(minAttemptTimer);//将DodgeAttemptTimer值与minAttemptTimer相比较，并把更大的那个赋值给DodgeAttemptTimer
        WantedDodgerollDirection = direction;//将direction（-1、0、1）赋值给 想要翻滚的方向
    }

    private void UpdateCooldowns()//翻滚冷却
    {
        if (!DodgerollTirednessTimer.Active && CurrentCharges < MaxCharges)
        {
            CurrentCharges = MaxCharges;


            if (!Main.dedServ && Player.IsLocal())
            {
                for (int i = 0; i < 5; i++)
                {
                    int d = Dust.NewDust(Player.TopLeft, Player.width, Player.height, DustID.ManaRegeneration, 0f, 0f, 255, default, Main.rand.Next(20, 26) * 0.1f);
                    Main.dust[d].noLight = true;
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0.5f;
                }
            }
        }

    }
    private bool TryStartDodgeroll()//尝试翻滚
    {
        bool isLocal = Player.IsLocal();//检测是否为本地玩家
        bool moveable = CanMove();//检测玩家能否移动
        //本地玩家 && 玩家可移动 && 尝试翻滚计时器关闭 && 刚按下翻滚键 && 没有黏在墙上 && 加上就是了
        if (isLocal && moveable && !DodgeAttemptTimer.Active && DodgerollKey.JustPressed && (!Player.mouseInterface || !Main.playerInventory) && !Player.sliding)
        {
            //(uint)(60f * 0.333f)  三分之一秒
            //(sbyte)Math.Sign(Player.KeyDirection().X)  按下左方向键，返回-1；不按按键，返回0；按下右方向键，返回1。
            //将DodgeAttemptTimer值与三分之一秒相比较，并把更大的那个赋值给DodgeAttemptTimer
            ////将键盘按下的方向 赋值给 想要翻滚的方向（不按按键为玩家朝向）
            QueueDodgeroll((uint)(60f * 0.333f), (sbyte)Math.Sign(Player.KeyDirection().X));
        }
        if (!ForceDodgeroll)
        {
            if (!isLocal)//如果不是本地玩家
            {
                return false;//尝试翻滚失败
            }
            if (!DodgeAttemptTimer.Active || NoDodgerollsTimer.Active || CurrentCharges == 0)//尝试翻滚计时器 || 禁止翻滚计时器开启 || 可翻滚次数为零
            {
                return false;//尝试翻滚失败
            }
            if ((Player.mount != null && Player.mount.Active) || Player.itemAnimation > 0)//玩家在使用矿车、坐骑 || 玩家使用物品动画时间>0
            {
                return false;//尝试翻滚失败
            }
            if(Player.TryGetModPlayer<PlayerDraw>(out var drawteleport) && drawteleport.teleporting)//传送时禁用翻滚
            {
                return false;//尝试翻滚失败
            }
        }
        DodgeAttemptTimer = 0;//尝试翻滚计时器清零

        //下面应该就是翻滚了
        if (!Main.dedServ)// ## 
        {
            SoundEngine.PlaySound(DodgerollSound, Player.Center);
        }
        Player.StopGrappling();
        Player.eocHit = 1;
        IsDodging = true;//玩家在翻滚 为真
        Player.TryGetModPlayer(out SmashDown smashDown);
        if(smashDown.IsSmashing)
        smashDown.justRolled = true;

        DodgeStartRotation = Player.GetModPlayer<PlayerBodyRotation>().Rotation;//玩家当前身体倾斜程度 赋值给 翻滚起始倾斜程度
        DodgeItemRotation = Player.itemRotation;//获取玩家翻滚前的itemRotation，赋值给 暂存的 翻滚ItemRotation
        DodgeTime = 0f;//翻滚时间清零
        DodgeDirectionVisual = (sbyte)Player.direction;//玩家朝向 赋值给 视觉上的翻滚朝向
        DodgeDirection = (WantedDodgerollDirection != 0) ? WantedDodgerollDirection : ((sbyte)Player.direction);//WantedDodgerollDirection由键盘左(-1)右(1)赋值，不按按键为0，采用玩家的朝向
        CurrentCharges = Math.Max(0, CurrentCharges - 1);//现有翻滚剩余段数 - 1（保证大于0）
        uint tirednessTime = (CurrentCharges == 0) ? DefaultDodgeCooldownTime : DefaultDodgeTirednessTime;//如果段数用完，会有更长的冷却时间
        DodgerollTirednessTimer.Set(tirednessTime);
        if (!isLocal)//不是本地玩家
        {
            ForceDodgeroll = false;//强制翻滚 为否
        }
        return true;//尝试翻滚成功，可以翻滚
    }

    private void UpdateDodging()
    {
        if (Player.mount.Active)//玩家在使用坐骑、矿车
        {
            IsDodging = false;//玩家没在翻滚
            return;
        }
        bool onGround = base.Player.OnGround();//玩家在地面
        bool wasOnGround = base.Player.WasOnGround();//玩家曾在地面
        ref float rotation = ref Player.GetModPlayer<PlayerBodyRotation>().Rotation;//玩家身体倾斜程度
        if (!IsDodging && !TryStartDodgeroll())//如果 玩家没在翻滚 && 尝试翻滚失败
        {
            return;//结束
        }
        if (DodgeTime < DodgeTimeMax / 1.5f && onGround && !wasOnGround)
        {
            Player.fallStart = (int)MathHelper.Lerp((float)base.Player.fallStart, (float)(int)(base.Player.position.Y / 16f), 0.35f);
        }
        var tilePos = Player.position.ToTileCoordinates16();
        int x = ((DodgeDirection > 0) ? (tilePos.X + 2) : (tilePos.X - 1));
        for (int y = tilePos.Y; y < tilePos.Y + 3; y++)
        {
            if (Main.tile.TryGet(x, y, out var tile) && tile.TileType == 10)
            {
                WorldGen.OpenDoor(x, y, DodgeDirection);
                for (int i = 0; i < 20; i++)//破门粒子效果
                {
                    int m = Dust.NewDust(new Point16(x, y + Main.rand.Next(-1, 2)).ToVector2() * 16, 8, 14, DustID.WoodFurniture, DodgeDirection * Main.rand.NextFloat(8f, 10f), Scale: Main.rand.NextFloat(0.8f, 1.2f));
                    Main.dust[m].alpha -= i * 6;
                }
                SoundEngine.PlaySound(AssetsLoader.door_break);
            }
        }
        if (DodgeTime < DodgeTimeMax * 0.5f)
        {
            float newVelX = (onGround ? 6f : 4f) * (float)DodgeDirection;
            if (Math.Abs(Player.velocity.X) < Math.Abs(newVelX) || Math.Sign(newVelX) != Math.Sign(Player.velocity.X))
            {
                Player.velocity.X = newVelX;
            }
        }
        if (!Main.dedServ)
        {
            Player.GetModPlayer<PlayerTrailEffects>().ForceTrailEffect(2);
        }
        Player.pulley = false;
        ForcedItemRotation = DodgeItemRotation;
        Player.GetModPlayer<PlayerAnimations>().ForcedLegFrame = PlayerFrames.Jump;
        ForcedDirection = DodgeDirectionVisual;
        rotation = ((DodgeDirection == 1) ? Math.Min((float)Math.PI * 2f, MathHelper.Lerp(DodgeStartRotation, (float)Math.PI * 2f, DodgeTime / (DodgeTimeMax * 1f))) : Math.Max((float)Math.PI * -2f, MathHelper.Lerp(DodgeStartRotation, (float)Math.PI * -2f, DodgeTime / (DodgeTimeMax * 1f))));
        DodgeTime += 1f / 60f;//进行翻滚的时间加六十分之一秒。一秒60帧。
        if (DodgeTime >= DodgeTimeMax)//如果 翻滚时间 大于 可翻滚最大时间
        {
            IsDodging = false;//玩家不再视为翻滚
            Player.eocDash = 0;//  ##
        }
        else
        {
            Player.runAcceleration = 0f;
        }
    }
    private static bool LateCanBeHitByEntity(Player player, Entity entity)
    {
        if (player.TryGetModPlayer(out Roll rolls) && rolls.IsDodging)
        {
            /*
            if (player.TryGetModPlayer(out Parry parry))
            {
                parry.CocoonParryProjectile(entity);

            }
            */
            return false;
        }

        if (player.TryGetModPlayer(out Parry parry) && player.shieldParryTimeLeft > 0)
        {
            if (entity is Projectile)
            {
                Projectile projectile = (Projectile)entity;
                parry.ShieldParryProjectile(projectile);
                return false;
            }
            if(entity is NPC)
            {
                parry.ShieldParryNPCAddBasicEffects(entity);
            }
        }

        if (player.TryGetModPlayer(out SmashDown smash) && smash.IsSmashing)
        {
            return false;
        }
        return true;
    }
    private static void PlayerNpcCollisionInjection(ILContext context)
    {
        var il = new ILCursor(context);
        ILLabel? continueLabel = null;
        int npcIndexLocalId = -1;

        il.Index = context.Instrs.Count - 1;

        il.GotoPrev(
            MoveType.After,
            i => i.MatchLdarg(0),
            i => i.MatchLdfld(typeof(Player), nameof(Terraria.Player.npcTypeNoAggro)),
            i => i.MatchLdsfld(typeof(Main), nameof(Main.npc)),
            i => i.MatchLdloc(out npcIndexLocalId),
            i => i.MatchLdelemRef(),
            i => i.MatchLdfld(typeof(NPC), nameof(NPC.type)),
            i => i.MatchLdelemU1(),
            i => i.MatchBrtrue(out continueLabel)
        );

        il.HijackIncomingLabels();

        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.npc))!);
        il.Emit(OpCodes.Ldloc, npcIndexLocalId);
        il.Emit(OpCodes.Ldelem_Ref);
        il.EmitDelegate((Player player, NPC npc) => LateCanBeHitByEntity(player, npc));
        il.Emit(OpCodes.Brfalse, continueLabel!);
    }

    private static void ProjectileDamageInjection(ILContext context)
    {
        var il = new ILCursor(context);

        ILLabel? skipHitLabel = null;

        // Match the last 'if (!Main.player[myPlayer2].CanParryAgainst(Main.player[myPlayer2].Hitbox, base.Hitbox, velocity))'.
        il.GotoNext(
            MoveType.After,
            i => i.MatchCallvirt(typeof(Player), nameof(Player.CanParryAgainst)),
            i => i.MatchBrtrue(out skipHitLabel)
        );

        il.HijackIncomingLabels();

        int emitLocation = il.Index;

        // Find player local
        int playerIndexLocalId = -1;

        il.GotoPrev(
            i => i.MatchLdsfld(typeof(Main), nameof(Main.player)),
            i => i.MatchLdloc(out playerIndexLocalId),
            i => i.MatchLdelemRef()
        );

        // Go back and emit

        il.Index = emitLocation;

        il.Emit(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.player))!);
        il.Emit(OpCodes.Ldloc, playerIndexLocalId);
        il.Emit(OpCodes.Ldelem_Ref);
        il.Emit(OpCodes.Ldarg_0);
        il.EmitDelegate((Player player, Projectile projectile) => LateCanBeHitByEntity(player, projectile));
        il.Emit(OpCodes.Brfalse, skipHitLabel!);
    }

    public void SetDirection(bool resetForcedDirection)
    {
        if (ForcedDirection != 0)
        {
            base.Player.direction = ForcedDirection;
            if (resetForcedDirection)
            {
                ForcedDirection = 0;
            }
        }
    }
}
