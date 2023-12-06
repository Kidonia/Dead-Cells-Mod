using Microsoft.Xna.Framework;
using DeadCells.Common.Buffs;
using DeadCells.Common.DamageClasses;
using DeadCells.Common.GlobalNPCs;
using DeadCells.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace DeadCells.Projectiles.WeaponAnimationProj;

public class BleederAtkA : DC_WeaponAnimation
{
    //以此为模板。
    public override string AnimName => "atkSaberA";//武器动画名称
    public override string fxName => "fxSaberA";//fx特效名称
    public override int HitFrame => 7;//击中帧
    public override int fxStartFrame => 6;//fx特效开始播放帧

    private Dictionary<int, DCAnimPic> WeaponDic = new();//新建字典，存放武器动画
    private Dictionary<int, DCAnimPic> fxDic = new();//新建字典，存放fx特效
    public override int TotalFrame => WeaponDic.Count;//总帧图图数，直接复制粘贴
    public override int fxFrames => fxDic.Count;//总fx特效图图数，直接复制粘贴
    public override void SetDefaults()
    {
        WeaponDic = AssetsLoader.AnimAtlas[AnimName];//初始化动画字典，直接复制粘贴
        fxDic = AssetsLoader.fxAtlas[fxName];//初始化fx特效字典，直接复制粘贴
        QuickSetDefault(128, 68, 20, BrutalityDamage.Instance, 1.5f, slowBeginFrame : 4, slowEndFrame : 3);
    }
    public override void AI()
    {
        DrawTheAnimationInAI(18f, 2.89f);//碰撞箱位置
        PlayWeaponSound(AssetsLoader.weapon_saber_release1, 4);//播放攻击声音
        CameraBump(2.4f, 1f, 19);//屏幕震动
        base.AI();
    }
    public override void PostDraw(Color lightColor)//绘制血刀贴图
    {
        DrawWeaponTexture(WeaponDic, 12, -35, true, new(255, 246, 173));//帧图绘制武器动画
        DrawfxTexture(fxDic, 12, -35, true, new(163, 0, 22));//帧图绘制fx特效图
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)//击中敌人效果
    {
        var drawicon = target.GetGlobalNPC<NPCDrawBuff>();
        drawicon.bleednum++;//流血层数加一
        target.AddBuff(ModContent.BuffType<Bleed>(), 720);
        SoundEngine.PlaySound(AssetsLoader.hit_blade);
    }
}
