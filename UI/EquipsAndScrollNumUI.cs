using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using DeadCells.Common.Players;
using Terraria.ID;
using DeadCells.Core;
using ReLogic.Content;
using Terraria.GameContent;
using System;

namespace DeadCells.UI;

internal class EquipsAndScrollNumUI : UIState
{
    private UIText BrutalityNum;
    private UIText TacticNum;
    private UIText SurvivalNum;
    private UIImage BrutalityIcon;
    private UIImage TacticIcon;
    private UIImage SurvivalIcon;
    private UIPanel BlackShadow;

    private UIElement EquipArea;
    private UIImage EquipBG1;//盾栏
    private UIImage EquipBG2;
    private UIImage EquipBG3;
    private UIImage EquipBG4;
    private UIImage Equip1;//盾栏
    private UIImage Equip2;
    private UIImage Equip3;
    private UIImage Equip4;
    private readonly Texture2D EmptyTex = ModContent.Request<Texture2D>(AssetsLoader.TransparentImg, (AssetRequestMode)1).Value;
    private readonly Color GrayLook = new(100, 100, 100, 90);
    private readonly Color SoftGray = new(160, 160, 160, 125);
    private readonly Texture2D EquipBGTex = ModContent.Request<Texture2D>("DeadCells/UI/Images/BG_Functional", (AssetRequestMode)1).Value;
    private readonly Texture2D EmptyShield = ModContent.Request<Texture2D>("DeadCells/UI/EquipmentUI/StartShield", (AssetRequestMode)1).Value;
    private readonly Texture2D EmptyTalisman = ModContent.Request<Texture2D>("DeadCells/UI/EquipmentUI/PrisonerTalisman", (AssetRequestMode)1).Value;
    private readonly Texture2D EmptySkill = ModContent.Request<Texture2D>("DeadCells/UI/EquipmentUI/SkillEmpty", (AssetRequestMode)1).Value;
    //可以添加一个SoftGray，用于技能冷却显示。
    //可以添加UIText，用于显示技能冷却时间。

    public override void OnInitialize()
    {
        //卷轴面板背景
        BlackShadow = new UIPanel();
        BlackShadow.Width.Set(150f, 0);
        BlackShadow.Height.Set(40f, 0);
        BlackShadow.Left.Set(-460f, 1f);
        BlackShadow.Top.Set(80f, 0f);
        Append(BlackShadow);

        //装备背景范围（测试用）
        EquipArea = new UIElement();
        EquipArea.Width.Set(136f, 0);
        EquipArea.Height.Set(136f, 0);
        EquipArea.Left.Set(-453f, 1f);
        EquipArea.Top.Set(128f, 0f);
        Append(EquipArea);

        EquipBG1 = new UIImage(EquipBGTex);//盾栏
        EquipBG1.Left.Set(0, 0f);
        EquipBG1.Top.Set(0, 0f);
        EquipBG1.Width.Set(52, 0f);
        EquipBG1.Height.Set(52, 0f);
        EquipArea.Append(EquipBG1);

        EquipBG2 = new UIImage(EquipBGTex);
        EquipBG2.Left.Set(60, 0f);
        EquipBG2.Top.Set(0, 0f);
        EquipBG2.Width.Set(52, 0f);
        EquipBG2.Height.Set(52, 0f);
        EquipArea.Append(EquipBG2);

        EquipBG3 = new UIImage(EquipBGTex);
        EquipBG3.Left.Set(0, 0f);
        EquipBG3.Top.Set(60, 0f);
        EquipBG3.Width.Set(52, 0f);
        EquipBG3.Height.Set(52, 0f);
        EquipArea.Append(EquipBG3);

        EquipBG4 = new UIImage(EquipBGTex);
        EquipBG4.Left.Set(60, 0f);
        EquipBG4.Top.Set(60, 0f);
        EquipBG4.Width.Set(52, 0f);
        EquipBG4.Height.Set(52, 0f);
        EquipArea.Append(EquipBG4);

        Equip1 = new UIImage(EmptyTex);//盾栏的盾
        Equip1.Left.Set(0, 0f);
        Equip1.Top.Set(0, 0f);
        Equip1.Width.Set(52, 0f);
        Equip1.Height.Set(52, 0f);
        EquipArea.Append(Equip1);

        Equip2 = new UIImage(EmptyTex);
        Equip2.Left.Set(60, 0f);
        Equip2.Top.Set(0, 0f);
        Equip2.Width.Set(52, 0f);
        Equip2.Height.Set(52, 0f);
        EquipArea.Append(Equip2);

        Equip3 = new UIImage(EmptyTex);
        Equip3.Left.Set(60, 0f);
        Equip3.Top.Set(0, 0f);
        Equip3.Width.Set(52, 0f);
        Equip3.Height.Set(52, 0f);
        EquipArea.Append(Equip3);

        Equip4 = new UIImage(EmptyTex);
        Equip4.Left.Set(60, 0f);
        Equip4.Top.Set(0, 0f);
        Equip4.Width.Set(52, 0f);
        Equip4.Height.Set(52, 0f);
        EquipArea.Append(Equip4);

        BrutalityNum = new UIText("0", 1.2f);
        BrutalityNum.Left.Set(0, 0f);
        BrutalityNum.Top.Set(-2, 0f);
        BrutalityNum.Width.Set(18, 0f);
        BrutalityNum.Height.Set(18, 0f);
        BlackShadow.Append(BrutalityNum);

        BrutalityIcon = new UIImage(ModContent.Request<Texture2D>("DeadCells/UI/Images/BrutalityIcon"));
        BrutalityIcon.Left.Set(20, 0f);
        BrutalityIcon.Top.Set(0, 0f);
        BrutalityIcon.Width.Set(18, 0f);
        BrutalityIcon.Height.Set(18, 0f);
        BlackShadow.Append(BrutalityIcon);

        TacticNum = new UIText("0", 1.2f);
        TacticNum.Left.Set(44, 0f);
        TacticNum.Top.Set(-2, 0f);
        TacticNum.Width.Set(18, 0f);
        TacticNum.Height.Set(18, 0f);
        BlackShadow.Append(TacticNum);

        TacticIcon = new UIImage(ModContent.Request<Texture2D>("DeadCells/UI/Images/TacticIcon"));
        TacticIcon.Left.Set(64, 0f);
        TacticIcon.Top.Set(0, 0f);
        TacticIcon.Width.Set(18, 0f);
        TacticIcon.Height.Set(18, 0f);
        BlackShadow.Append(TacticIcon);

        SurvivalNum = new UIText("0", 1.2f);
        SurvivalNum.Left.Set(88, 0f);
        SurvivalNum.Top.Set(-2, 0f);
        SurvivalNum.Width.Set(18, 0f);
        SurvivalNum.Height.Set(18, 0f);
        BlackShadow.Append(SurvivalNum);

        SurvivalIcon = new UIImage(ModContent.Request<Texture2D>("DeadCells/UI/Images/SurvivalIcon"));
        SurvivalIcon.Left.Set(108, 0f);
        SurvivalIcon.Top.Set(0, 0f);
        SurvivalIcon.Width.Set(18, 0f);
        SurvivalIcon.Height.Set(18, 0f);
        BlackShadow.Append(SurvivalIcon);
    }




    public override void Draw(SpriteBatch spriteBatch)
    {
        var GetScrollNum = Main.LocalPlayer.GetModPlayer<PlayerEquipAndScroll>();
        BrutalityNum.SetText($"{GetScrollNum.BrutalityNum}");
        TacticNum.SetText($"{GetScrollNum.TacticsNum}");
        SurvivalNum.SetText($"{GetScrollNum.SurvivalNum}");

        //面板背景颜色
        BlackShadow.BackgroundColor = new Color(0, 0, 0, 100);
        BlackShadow.BorderColor.A = 0;

        //上色，后面可以加上无障碍模式
        BrutalityNum.TextColor =  new Color(172, 77, 82);
        BrutalityIcon.Color = new Color(158, 41, 46);
        TacticNum.TextColor = new Color(125, 105, 181);
        TacticIcon.Color = new Color(110, 71, 186);
        SurvivalNum.TextColor = new Color(44, 173, 109);
        SurvivalIcon.Color = new Color(25, 152, 98);

        if (!Main.playerInventory)//未打开背包
        {
            EquipBG1.SetImage(EquipBGTex);
            EquipBG2.SetImage(EquipBGTex);
            EquipBG3.SetImage(EquipBGTex);
            EquipBG4.SetImage(EquipBGTex);

            UpdateEquipImage(Equip1, GetScrollNum.CurrentShieldID, 1, GetScrollNum.ShieldCoolDownShow);//盾
            UpdateEquipImage(Equip2, 0, 2);//项链
            UpdateEquipImage(Equip3, 0, 3);//技能1
            UpdateEquipImage(Equip4, 0, 4);//技能2


            base.Draw(spriteBatch); //！！不能删！！
        }
        else
        {
            Equip1.SetImage(EmptyTex);
            Equip2.SetImage(EmptyTex);
            Equip3.SetImage(EmptyTex);
            Equip4.SetImage(EmptyTex);
            EquipBG1.SetImage(EmptyTex);
            EquipBG2.SetImage(EmptyTex);
            EquipBG3.SetImage(EmptyTex);
            EquipBG4.SetImage(EmptyTex);
            base.Draw(spriteBatch); //！！不能删！！
        }
        
        

    }
    public Texture2D GetIDTex(int EquipItemID, int SlotNum)//根据 ID 和 第几栏 获取材质
    {
        if (EquipItemID == 0 || EquipItemID > ItemID.Count)
        {
            if (SlotNum == 1)//盾栏
                return EmptyShield;
            else if (SlotNum == 2)//项链栏
                return EmptyTalisman;
            else if (SlotNum == 3)//技能栏1
                return EmptySkill;
            else if (SlotNum == 4)//技能栏2
                return EmptySkill;
            else
                return EmptyTex;
        }
        else
            return TextureAssets.Item[EquipItemID].Value;
    }

    public Color PickCorrectColor(int ItemID, bool coolDownColor)//根据 ID 和 是否是冷却颜色 决定 绘制颜色
    {
        if (ItemID == 0)
            return GrayLook;
        if(coolDownColor)
            return SoftGray;
        return Color.White;
    }

    public void UpdateEquipImage(UIImage image, int ItemID, int SlotNum, bool coolDownColor = false)//决定装备的图像显示
    {
        Texture2D tex = GetIDTex(ItemID, SlotNum);
        int i = 0, j = 0;
        if (SlotNum == 2 || SlotNum == 4)
            i = 1;
        if (SlotNum == 3 || SlotNum == 4)
            j = 1;
        float scalefit = 48f / Math.Max(tex.Width, tex.Height);
        image.ImageScale = scalefit * 2 / 3;
        image.Left.Set((48 - tex.Width) / 2 + i * 60f +1.92f, 0);
        image.Top.Set((48 - tex.Height) / 2 +j * 60f + 1.92f, 0);
        image.SetImage(tex);
        image.Color = PickCorrectColor(ItemID, coolDownColor);
    }

    class ShowScrollNumUISystem : ModSystem
    {
        //这些东西照搬就好，不要碰
        internal EquipsAndScrollNumUI ESUI;
        internal UserInterface DUI;
        public override void Load()
        {
            base.Load();
            ESUI = new EquipsAndScrollNumUI();
            DUI = new UserInterface();
            ESUI.Activate();
            DUI.SetState(ESUI);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            DUI?.Update(gameTime);
            base.UpdateUI(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            base.ModifyInterfaceLayers(layers);//鼠标放上去可以去wiki链接里看
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));//这里是字符串匹配。"Vanilla: Resource Bars" : Draws health, mana, and breath bars, as well as buff icons.
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "Dead Cells ScrollUI",
                    delegate
                    {
                        DUI.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

    }
}
//3 scroll