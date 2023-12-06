using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DeadCells.Common.DamageClasses;
using DeadCells.Common.Players;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace DeadCells.Items;

public abstract class DeadCellsItem : ModItem
{
    public Player player => Main.player[Main.myPlayer];
    public PlayerAtk playerComboAttack => player.GetModPlayer<PlayerAtk>();

    public bool IsWeapon = false;
    public bool IsSkill = false;
    public bool IsMutation = false;

    public bool AlphaDrawIconRequired = false;

    public virtual void SetWeaponDefaults(DamageClass damageType, int damage, float knockback, int usetime, int useAnimation, int sellpricefromCDB, int useStyle = 1, int crit = 0, int rare = 10, int shoot = 10, float shootSpeed = 1f, int width = 48, int height = 48, bool material = false, bool noMelee = true, bool autoReuse = false)
    {
        IsWeapon = true;
        Item.DamageType = damageType;//流派
        Item.damage = damage;
        Item.knockBack = knockback;
        Item.useTime = usetime;
        Item.useAnimation = useAnimation;
        Item.value = sellpricefromCDB * 100;
        Item.useStyle = useStyle;//1，即 ItemUseStyleID.Swing    剑挥舞

        Item.crit = crit;
        Item.rare = rare;//10，即 ItemRarityID.Red
        Item.shoot = shoot;//10，即 ProjectileID.PurificationPowder   手持弹幕都用这个，没有为什么。
        Item.shootSpeed = shootSpeed;
        Item.width = width;//icon默认48宽
        Item.height = height;//icon默认48高

        Item.material = material;
        Item.noMelee = noMelee;
        Item.autoReuse = autoReuse;//自动使用
        Item.noUseGraphic = true;//使用时不展示Icon，肯定
    }
    public virtual void SetSkillDefaults(DamageClass damageType, int useTime, int useAnimation, int sellpricefromCDB, int width = 48, int height = 48, int useStyle = 10, int rare = 10)
    {
        IsSkill = true;
        Item.DamageType = damageType;//流派
        Item.useTime = useTime;
        Item.useAnimation = useAnimation;
        Item.value = sellpricefromCDB * 100;

        Item.width = width;
        Item.height = height;
        Item.useStyle = useStyle;//10，即 ItemUseStyleID.HiddenAnimation
        Item.rare = rare;//10，即 ItemRarityID.Red
        Item.noUseGraphic = true;//使用时不展示Icon，肯定
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////                                           图标部分                                                //////////////    
    /*
    public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        if (AlphaDrawIconRequired)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            itemColor = new Color(255, 255, 255, 0);
            spriteBatch.Draw(texture, position, frame, itemColor, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
        return !AlphaDrawIconRequired;
    }
    */
    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        if (AlphaDrawIconRequired)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            Vector2 drawPos = Item.position - Main.screenPosition;
            Color drawColor = new Color(255, 255, 255, 0);
            spriteBatch.Draw(texture, drawPos, drawColor);
            return false;
        }
        return !AlphaDrawIconRequired;
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////
    //






    //
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////                                           武器部分                                                //////////////      
    public override bool CanUseItem(Player player)
    {
        //玩家后摇结束，且世界中没有存留上一次攻击
        return player.ownedProjectileCounts[Item.shoot] < 1 && playerComboAttack.WeaponCoolDown == 0 && playerComboAttack.ConsistentLockCtrlAfter == 0;
    }

    /// <summary>
    /// coolDownTime 武器的冷却时间，该时间过后可再次使用该武器。最后一段攻击使用。攻击间隔清零，攻击段数回退1。
    /// </summary>
    public void FinalComboAttack(int coolDownTime = 0)
    {
        //最终那段攻击后执行
        //攻击间隔清零，攻击段数回退1
        playerComboAttack.TimeCanConsistentAttack = 0;
        playerComboAttack.NextStrikeChainNum = 1;
        playerComboAttack.WeaponCoolDown = coolDownTime;
    }

    /// <summary>
    /// 初始化下一段攻击，包括：① timebetween 初始化玩家两段攻击间隔剩余时间。② lockCtrlBetween 下一段攻击的前摇。
    /// 假设为（60, 14），则表示接下来60帧内可进行第二次攻击，但是14帧以后才能进行。
    /// </summary>
    /// <param name="timebetween"></param>
    /// <param name="lockCtrlBetween"></param>
    public void InitialNextComboAttack(int timebetween, int lockCtrlBetween)
    {
        //初始化下一段攻击，包括：
        //初始化玩家两段攻击间隔剩余时间
        //攻击段数加一，即进行下一段攻击
        //下一段攻击的前摇
        playerComboAttack.TimeCanConsistentAttack = timebetween;//两段攻击间隔剩余时间， 60 == 1秒
        playerComboAttack.ConsistentLockCtrlAfter = lockCtrlBetween;
        playerComboAttack.NextStrikeChainNum++;//段数加一
    }

    /// <summary>
    /// NextAttackChain 下一段攻击是第几段攻击。如果玩家两段攻击间隔剩余时间大于零，且，下一段攻击对得上号（正常都能对的上号），且，间隔冷却时间为零，则返回真
    /// </summary>
    /// <param name="NextAttackChain"></param>
    public bool CanNextAttack(int NextAttackChain)
    {
        //如果玩家两段攻击间隔剩余时间大于零，且，下一段攻击对得上号（正常都能对的上号），且，间隔冷却时间为零，则返回真
        return playerComboAttack.TimeCanConsistentAttack > 0 && playerComboAttack.NextStrikeChainNum == NextAttackChain && playerComboAttack.ConsistentLockCtrlAfter == 0;
    }

    public bool FirstAttack()
    {
        return (playerComboAttack.NextStrikeChainNum == 1);
    }

    /// <summary>
    /// 伤害放大倍数，即伤害为物品基础伤害的对少倍。填的数要大于1，不然伤害会缩小。一定记得写清流派。
    /// </summary>
    /// <param name="mul"></param>
    /// <returns></returns>
    public int DamageMul(float mul = 1f)
    {
        if(Item.DamageType == BrutalityDamage.Instance)
            return (int)player.GetTotalDamage<BrutalityDamage>().ApplyTo(Item.damage * mul);

        if (Item.DamageType == TacticsDamage.Instance)
            return (int)player.GetTotalDamage<TacticsDamage>().ApplyTo(Item.damage * mul);

        if (Item.DamageType == SurvivalDamage.Instance)
            return (int)player.GetTotalDamage<SurvivalDamage>().ApplyTo(Item.damage * mul);

        return (int)(Item.damage * mul);
    }
    /*
     * 这是给多流派判断用的
    public virtual DamageClass CheckDamageClass()
    {
        if(playerScroll.BrutalityNum > playerScroll.TacticsNum)
            if(playerScroll.BrutalityNum > playerScroll.SurvivalNum)
                return BrutalityDamage.Instance;



        return BrutalityDamage.Instance;
    }
    */
}
