using DeadCells.Common.Buffs;
using DeadCells.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace DeadCells.Common.Players;

public class PlayerFall : ModPlayer
{
    public bool stunned = false;
    public override void ResetEffects()
    {
        Player.noFallDmg = true;
    }

    public override void PreUpdate()
    {
        //来自源码的判断掉落伤害。根据伤害确定buff时间。
        if (Player.velocity.Y == 0f)
        {
            int num17 = 15;
            num17 += Player.extraFall;
            int num18 = (int)(Player.position.Y / 16f) - Player.fallStart;
            bool flag12 = false;
            for (int i = 3; i < 10; i++)
            {
                if (Player.armor[i].stack > 0 && Player.armor[i].wingSlot > -1)
                {
                    flag12 = true;
                }
            }
            if (((Player.gravDir == 1f && num18 > num17) || (Player.gravDir == -1f && num18 < -num17)) && !flag12)
            {
                Player.immune = false;
                int num25 = (int)(num18 * Player.gravDir - num17) * 10;
                if (Player.mount.Active)
                {
                    num25 = (int)(num25 * Player.mount.FallDamage);
                }
                if (num25 > 32)
                {
                    if (num25 > 130)//根据伤害确定buff时间。
                    {
                        num25 = 130;
                    }

                    SoundEngine.PlaySound(AssetsLoader.stun);

                    Player.AddBuff(ModContent.BuffType<Stun>(), num25);//添加DeBuff
                    //Player.GiveImmuneTimeForCollisionAttack(num25 / 10);
                    stunned = true;//玩家属性确定
                }

            }
        }
    }
}
