using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using DeadCells.Common.Players;

namespace DeadCells.UI;

internal class CellNumUI : UIState
{
    private UIText text;
    private UIText money;
    private UIText killnum;
    private UIElement CellArea;
    private UIElement MoneyArea;
    private UIElement KillArea;
    private UIImage cellBG;
    private UIImage moneyBG;
    private UIImage killBG;
    private UIImage killBG_Thirty;
    private UIImage killBG_Perfect;

    public override void OnInitialize()
    {
        KillArea = new UIElement();
        KillArea.Width.Set(120f, 0);
        KillArea.Height.Set(45f, 0);
        KillArea.Left.Set(-288f, 1f);
        KillArea.Top.Set(-301, 1f);
        Append(KillArea);

        CellArea = new UIElement();
        CellArea.Width.Set(120f, 0);
        CellArea.Height.Set(45f, 0);
        CellArea.Left.Set(-288f, 1f);
        CellArea.Top.Set(-266f, 1f);
        Append(CellArea);

        MoneyArea = new UIElement();
        MoneyArea.Width.Set(120f, 0);
        MoneyArea.Height.Set(45f, 0);
        MoneyArea.Left.Set(-288f, 1f);
        MoneyArea.Top.Set(-231, 1f);
        Append(MoneyArea);

        cellBG = new UIImage(ModContent.Request<Texture2D>("DeadCells/UI/Images/cellBG"));
        cellBG.Left.Set(-65, 0f);
        cellBG.Top.Set(-22, 0f);
        cellBG.Width.Set(180, 0f);
        cellBG.Height.Set(60, 0f);

        moneyBG = new UIImage(ModContent.Request<Texture2D>("DeadCells/UI/Images/moneyBG"));
        moneyBG.Left.Set(-65, 0f);
        moneyBG.Top.Set(-22, 0f);
        moneyBG.Width.Set(180, 0f);
        moneyBG.Height.Set(60, 0f);

        killBG = new UIImage(ModContent.Request<Texture2D>("DeadCells/UI/Images/normalBG"));
        killBG.Left.Set(-65, 0f);
        killBG.Top.Set(-22, 0f);
        killBG.Width.Set(180, 0f);
        killBG.Height.Set(60, 0f);

        killBG_Thirty = new UIImage(ModContent.Request<Texture2D>("DeadCells/UI/Images/thirtyBG"));
        killBG_Thirty.Left.Set(-65, 0f);
        killBG_Thirty.Top.Set(-22, 0f);
        killBG_Thirty.Width.Set(180, 0f);
        killBG_Thirty.Height.Set(60, 0f);

        killBG_Perfect = new UIImage(ModContent.Request<Texture2D>("DeadCells/UI/Images/perfectBG"));
        killBG_Perfect.Left.Set(-65, 0f);
        killBG_Perfect.Top.Set(-22, 0f);
        killBG_Perfect.Width.Set(180, 0f);
        killBG_Perfect.Height.Set(60, 0f);

        text = new UIText("0", 1.2f);
        text.Width.Set(145, 0f);
        text.Height.Set(60, 0f);
        text.Top.Set(0, 0f);
        text.Left.Set(-32, 0f);
        text.TextColor = new Color(84, 150, 179);

        money = new UIText("0", 1.2f);
        money.Width.Set(145, 0f);
        money.Height.Set(60, 0f);
        money.Top.Set(0, 0f);
        money.Left.Set(-32, 0f);
        money.TextColor = new Color(166, 153, 97);

        killnum = new UIText("0", 1.2f);
        killnum.Width.Set(145, 0f);
        killnum.Height.Set(60, 0f);
        killnum.Top.Set(0, 0f);
        killnum.Left.Set(-32, 0f);
        killnum.TextColor = new Color(166, 172, 178);

        CellArea.Append(text);
        MoneyArea.Append(money);
        KillArea.Append(killnum);
        CellArea.Append(cellBG);
        MoneyArea.Append(moneyBG);

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        var PLC = Main.LocalPlayer.GetModPlayer<PlayerCell>();
        int Mnm = (int)(Terraria.Utils.CoinsCount(out bool f, Main.LocalPlayer.inventory, new int[0]) / 100);
        int killNum = PLC.num_of_kill;
        text.SetText($"{PLC.CurrentCellNum}");
        money.SetText($"{Mnm}");
        killnum.SetText($"{killNum}");
        if (killNum >= 60)
        {
            KillArea.Append(killBG_Perfect);
        }
        else if (killNum >= 30 && killNum < 60)
        {
            KillArea.Append(killBG_Thirty);
        }
        else
        {
            KillArea.Append(killBG);
        }
        base.Draw(spriteBatch);//！！不能删！！
    }

}

class ExampleResourseUISystem : ModSystem
{
    internal CellNumUI CLUI;
    internal UserInterface UC;
    public override void Load()
    {
        base.Load();
        CLUI = new CellNumUI();
        UC = new UserInterface();
        CLUI.Activate();
        UC.SetState(CLUI);
    }
    public override void UpdateUI(GameTime gameTime)
    {
        UC?.Update(gameTime);
        base.UpdateUI(gameTime);
    }
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        base.ModifyInterfaceLayers(layers);//鼠标放上去可以去wiki链接里看
        int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));//这里是字符串匹配。"Vanilla: Resource Bars" : Draws health, mana, and breath bars, as well as buff icons.
        if (resourceBarIndex != -1)
        {
            layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                "Dead Cells CounterUI",
                delegate {
                    UC.Draw(Main.spriteBatch, new GameTime());
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }

}
//combo kill
// cell num
//money num
