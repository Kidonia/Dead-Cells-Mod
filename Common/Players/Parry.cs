using DeadCells.Core;
using DeadCells.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Capture;

namespace DeadCells.Common.Players;

public class Parry : ModPlayer
{
    //				Player.shield_parry_cooldown = 15;//松开右键后盾反的冷却时间。如果在此期间按右键，只会举盾而不会盾反，且松开右键后冷却同样重置。
    //				Player.shieldParryTimeLeft = 0;//可盾反敌人碰撞的时间
    //				if (Player.shieldRaised)//只要按住右键就是举盾
    //             Player.hasRaisableShield //饰品栏有盾牌存在
    public bool AccHasShield;
    public bool ShieldSlotEquipped;
    public bool SuccessParry = false;
    public bool extraShieldRaised = false;
    public override void PreUpdate()
    {
        if (Player.HeldItem.shieldSlot > 0)
        {
            Player.shield = Player.HeldItem.shieldSlot;
            Player.cShield = 0;
            Player.hasRaisableShield = true;
        }


        if (SuccessParry && Player.shield_parry_cooldown > 0)//成功招架刷新冷却时间
        {
            Player.shield_parry_cooldown = 0;
            for (int i = 0; i < 10; i++)
            {
                int num = Dust.NewDust(Player.Center + new Vector2((float)(Player.direction * 6 + ((Player.direction == -1) ? -10 : 0)), -14f), 10, 16, 45, 0f, 0f, 255, new Color(255, 100, 0, 127), (float)Main.rand.Next(10, 16) * 0.1f);
                Main.dust[num].noLight = true;
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 0.5f;
            }
            SuccessParry = false;
        }

        //以下为盾反实现 以源码为主
        if (extraShieldRaised)//举起模组盾减速
        {
            Player.bodyFrame.Y = Player.bodyFrame.Height * 10;
            Player.moveSpeed /= 3f;
            if (Player.velocity.Y == 0f && Math.Abs(Player.velocity.X) > 3f)
            {
                Player.velocity.X = Player.velocity.X / 2f;
            }
        }

        bool theLeftMouseCheck =//检测在物品栏使用盾
                   Player.inventory[Player.selectedItem].shieldSlot > 0
            && Player.inventory[Player.selectedItem].type != ItemID.BouncingShield
            && Player.selectedItem != 58
            && Player.hasRaisableShield//拥有可举起的盾
            && Player.shield_parry_cooldown == 0//额外内容，保证每次盾反都会先盾反再举盾
            && !Player.mount.Active//未开启矿车
            && !Player.controlUseTile//未按住右键
            && Player.releaseUseTile//松开右键
            && Player.controlUseItem//按住左键
            && !Player.mouseInterface//鼠标不在 可交互UI界面上
            && !CaptureManager.Instance.Active
            && !Main.SmartInteractShowingGenuine;



            bool theGeneralCheck =//检测使用在盾栏或饰品栏的盾
            Player.inventory[Player.selectedItem].type != ItemID.DD2SquireDemonSword
            && Player.inventory[Player.selectedItem].type != ItemID.BouncingShield
            && (ShieldSlotEquipped || AccHasShield)//饰品栏或盾栏有盾
            && Player.selectedItem != 58
            && Player.hasRaisableShield//拥有可举起的盾
            && Player.shield_parry_cooldown == 0//额外内容，保证每次盾反都会先盾反再举盾
            && !Player.mount.Active//未开启矿车
            && Player.controlUseTile//按住右键
            && !Player.tileInteractionHappened//未与开关交互
            && Player.releaseUseItem//松开左键
            && !Player.controlUseItem//未按住左键
            && !Player.mouseInterface//鼠标不在 可交互UI界面上
            && !CaptureManager.Instance.Active
            && !Main.HoveringOverAnNPC//鼠标不在 可交互NPC身上（远距离不可交互的没影响）
            && !Main.SmartInteractShowingGenuine;

        bool shouldGuard = false;//盾反状态
        if ((theLeftMouseCheck || theGeneralCheck) && Player.itemAnimation == 0)
        {
            Player.ChangeDir((Main.MouseWorld - Player.Center).X > 0 ? 1 : -1);
            Player.inventory[Player.selectedItem].useTurn = true;
            shouldGuard = true;
        }
        if (shouldGuard != extraShieldRaised)//与盾反状态不符则执行
        {
            if (shouldGuard)
            {
                SoundEngine.PlaySound(AssetsLoader.weapon_shield_charge, Player.Center);
            }
            extraShieldRaised = shouldGuard;
            if (extraShieldRaised)//只要按住右键就是举盾
            {
                if (Player.shield_parry_cooldown == 0)//松开右键后盾反的冷却时间。
                    Player.shieldParryTimeLeft = 1;//可盾反敌人碰撞的时间。交给原版计算，每帧+1，加到20清零

                Player.itemAnimation = 0;
                Player.itemTime = 0;
                Player.reuseDelay = 0;

                return;
            }
            Player.shield_parry_cooldown = 15;//松开右键后盾反的冷却时间。如果在此期间按右键，只会举盾而不会盾反，且松开右键后冷却同样重置。
            Player.shieldParryTimeLeft = 0;
            Player.ApplyAttackCooldown(20);
        }




    }

    public override void PostUpdate()
    {
        bool theLeftMouseCheck =//检测在物品栏选中盾（不是使用）
           Player.inventory[Player.selectedItem].shieldSlot > 0//当前手持盾
    && Player.inventory[Player.selectedItem].type != ItemID.BouncingShield
    && Player.selectedItem != 58
    && !Player.mount.Active//未开启矿车
    && !Player.controlUseTile//未按住右键
    && Player.releaseUseTile//松开右键
    && !Player.mouseInterface//鼠标不在 可交互UI界面上
    && !CaptureManager.Instance.Active
    && !Main.SmartInteractShowingGenuine;


        if (theLeftMouseCheck)
        {
            Player.shield = Player.HeldItem.shieldSlot;//将手持盾改为当前选中盾
            Player.cShield = 0;
            Player.hasRaisableShield = true;
        }
    }

    public bool CanParryProj(Rectangle blockingPlayerRect, Rectangle projRect, Vector2 enemyVelocity)
    {
        return blockingPlayerRect.Intersects(projRect) 
            && Player.shieldParryTimeLeft > 0 
            && Math.Sign(blockingPlayerRect.Center.X - projRect.Center.X) == Player.direction 
            && enemyVelocity != Vector2.Zero;
    }


    public void CocoonParryProjectile(Entity entity)//全方位盾反弹幕，茧和背包盾可用
    {
        if (entity is Projectile)
        {
            Projectile.NewProjectileDirect(entity.GetSource_FromAI(), Player.Center, Vector2.Normalize(-entity.velocity) * 12.4f, ModContent.ProjectileType<NormalArrow>(), 50, 0, Player.whoAmI);
            int showBoomDmg = CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, Player.width, Player.height), new(12, 218, 255), "招架", true, false);
            Main.combatText[showBoomDmg].lifeTime = 66;
            entity.active = false;
        }
    }

    public void ShieldParryProjectile(Projectile projectile)//正面盾牌盾反弹幕
    {
        if (CanParryProj(Player.getRect(), projectile.Hitbox, projectile.velocity))//**//
        {
            Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), Player.Center, Vector2.Normalize(-projectile.velocity) * 12.4f, ModContent.ProjectileType<NormalArrow>(), 50, 0, Player.whoAmI);
            PopShieldParryText();
            projectile.active = false;
            SoundEngine.PlaySound(AssetsLoader.weapon_shield_block1, Player.Center);
            Player.shield_parry_cooldown = 0;//盾反成功结束冷却时间
            SuccessParry = true;
        }

    }

    public void ShieldParryNPCAddBasicEffects(Entity entity)//添加盾反基础效果
    {
        if(entity is NPC && Player.CanParryAgainst(Player.getRect(), entity.Hitbox, entity.velocity))
        {
            PopShieldParryText();
            SoundEngine.PlaySound(AssetsLoader.weapon_shield_block2, Player.Center);
            Player.shield_parry_cooldown = 0;//盾反成功结束冷却时间
            SuccessParry = true;
        }
    }


    public void PopShieldParryText()//添加 格挡 字幕
    {
        int showBoomDmg = CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, Player.width, Player.height), new(12, 218, 255), "招架", true, false);
        Main.combatText[showBoomDmg].lifeTime = 66;
    }

}
