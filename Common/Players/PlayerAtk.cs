using Terraria;
using Terraria.ModLoader;

namespace DeadCells.Common.Players;

public class PlayerAtk : ModPlayer
{
    public int TimeCanConsistentAttack = 0;
    public int ConsistentLockCtrlAfter = 0;
    public int NextStrikeChainNum = 1;
    public int WeaponCoolDown = 0;

    public int QuickSwordAtkComboNum = 0;
    public int QuickSwordComboBetweenTime = 0;

    public int OilSwordHitFireTargetTime = 0;

    public int KingsSpearComboKillNum = 0;
    public int KingsSpearKillShortPerioud = 0;
    public int KingsSpearCritTime = 0;

    public int PerfectHalberdHurtTime = 0;

    public bool TickScytheCheckHit = false;
    public bool TickScytheCanCrit = false;

    private string LastHoldItemName = "";

    public override void PreUpdate()
    {
        //计时器

        if (LastHoldItemName != Player.HeldItem.Name)//检测玩家是否滚轮切换了背包物品
        {
            TimeCanConsistentAttack = 0;//切换清零可进行下一段攻击的时间间隔，避免直接打出第二段攻击
            NextStrikeChainNum = 1;//切换则清零攻击段数
            WeaponCoolDown = 0;//切换清零后摇
            LastHoldItemName = Player.HeldItem.Name;
        }



        //两段攻击间隔剩余时间，用于判断武器能否进行连贯攻击
        if (TimeCanConsistentAttack > 0)
            TimeCanConsistentAttack--;
        //剩余时间为1帧，刷新使武器回到第一段攻击
        if (TimeCanConsistentAttack == 1)
        {
            NextStrikeChainNum = 1;
            TickScytheCanCrit = false;//巨镰下段攻击不可暴击。
            TickScytheCheckHit = false;//巨镰击中敌人的判定刷新。
        }
        //可进行下一段攻击的剩余时间，或者说多少时间后能进行下一段攻击
        if (ConsistentLockCtrlAfter > 0)
            ConsistentLockCtrlAfter--;
        //武器冷却时间，能够重新使用该武器的时间
        if (WeaponCoolDown > 0)
            WeaponCoolDown--;

        if (NextStrikeChainNum == 0)
            NextStrikeChainNum++;//避免便乘0的情况，正常不应该出现

        //均衡之刃
        if (QuickSwordComboBetweenTime > 0)//均衡之刃计算连击段数的计时器
            QuickSwordComboBetweenTime--;
        if (QuickSwordComboBetweenTime == 0)//计时器为零，段数清零
            QuickSwordAtkComboNum = 0;
        if (QuickSwordAtkComboNum > 10)//段数最高为十
            QuickSwordAtkComboNum = 10;
        
        //油刀
        if (OilSwordHitFireTargetTime > 0)
            OilSwordHitFireTargetTime--;
        
        //化境
        if(PerfectHalberdHurtTime  > 0)
            PerfectHalberdHurtTime--;



        //对称长枪
        if (KingsSpearKillShortPerioud > 0)
            KingsSpearKillShortPerioud--;
        if(KingsSpearCritTime > 0)
            KingsSpearCritTime--;

        if (KingsSpearKillShortPerioud == 1)//如果迅速击杀计时器耗尽
            KingsSpearComboKillNum = 0;//击杀数清零

        if (KingsSpearComboKillNum == 1 && KingsSpearKillShortPerioud == 0)//如果只杀了一个，且，迅速击杀计时器为0
             KingsSpearKillShortPerioud = 75;//迅速击杀计时器设为48
        if (KingsSpearComboKillNum > 1)//如果杀了超过一个
        {
            KingsSpearComboKillNum = 0;//击杀计数清零
            KingsSpearCritTime = 360;//可暴击时间设为360（6秒）
        }

        /*
        if (Main.playerInventory)
            Main.NewText("");
        else
            Main.NewText("");
        */
    }

    public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
    {
        QuickSwordAtkComboNum = 0;//均衡之刃段数清零
        TickScytheCanCrit = false;//巨镰下一段不可暴击
        PerfectHalberdHurtTime = 900;//化境15秒不暴击
        base.OnHitByNPC(npc, hurtInfo);
    }
    public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
    {
        QuickSwordAtkComboNum = 0;//均衡之刃段数清零
        TickScytheCanCrit = false;//巨镰下一段不可暴击
        PerfectHalberdHurtTime = 900;//化境15秒不暴击

        base.OnHitByProjectile(proj, hurtInfo);
    }
}
