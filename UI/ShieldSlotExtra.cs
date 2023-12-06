using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using DeadCells.Common.Players;

namespace DeadCells.UI
{//WingSlotExtra.WingConfig.SlotsNextToAccessories == false 则 在五个装备栏放置盾栏
    //SlotsAlongAccessories == false 则 在饰品栏的左边放置
    /*
    public class ShieldSlotExtra : Mod
    {
        private static ShieldSlotExtra instance;
        //private static bool resourcePackEnabled = false;//使用资源包


        internal static ShieldSlotExtra Instance { get => instance; set => instance = value; }
        //internal static bool ResourcePackEnabled { get => resourcePackEnabled; set => resourcePackEnabled = value; }//使用资源包


        public override void Load() => Instance = this;

        public override void Unload() => Instance = null;
    }
    */
    /*
    internal class ShieldSlotPlayer : ModPlayer //检测是否启用资源包
    {
        internal static bool ResourcePackCheck() // Not fool-proof but probably the next best thing to check for non-default UI
        {
            try
            {
                string[] uiArr = { "UI", "Interface", "Texture", "Overhaul", "Display", "Graphic" }; // Excluded: menu, style
                foreach (var modPack in Main.AssetSourceController.ActiveResourcePackList.EnabledPacks)
                {
                    return uiArr.Any(modPack.Name.Contains) || uiArr.Any(modPack.Description.Contains);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error fetching modpacks: {0}", e);
            }
            return false;
        }
    
        //public override void OnEnterWorld() => ShieldSlotExtra.ResourcePackEnabled = ResourcePackCheck();//启用资源包
    }
    */
    /*
    internal class ShieldSlotExtraGlobalItem : GlobalItem //模组Config：其他饰品栏能否装盾
    {
        public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded) =>
            (item.shieldSlot > 0 && slot < 20 && modded == false) ? ShieldSlotExtra.ShieldConfig.AllowAccessorySlots : base.CanEquipAccessory(item, player, slot, modded);
    }
    */
    internal class ShieldSlotExtraUpdateUI : ModSystem
    {
        private static int posX;
        private static int posY;

        internal static int PosX { get => posX; set => posX = value; }
        internal static int PosY { get => posY; set => posY = value; }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) // UpdateUI(GameTime gameTime) // Render Shield slot interface (no controller compatibility for mod slots yet)
        {
            

            if (Main.gameMenu) // Adjust location of Shield slot depending on current position and setting of other interfaces
                return;

            int mapH = (Main.mapEnabled && !Main.mapFullscreen && Main.mapStyle == 1) ? 256 : 0;
            Main.inventoryScale = 0.85f;//没啥效果，估计是源码

            if (Main.EquipPage == 2)
            {
                mapH = (Main.mapEnabled && (mapH + 600) > Main.screenHeight) ? Main.screenHeight - 600 : mapH;
                PosX = (Main.netMode == NetmodeID.MultiplayerClient) ? (Main.screenWidth - 144 - (47 * 2)) - 47 : Main.screenWidth - 144 - (47 * 2);
                PosY = mapH + 174;
            }
            else // 默认：盾栏在饰品栏页面
            {
                if (Main.mapEnabled)//进游戏都是true
                {
                    mapH = ((mapH + 600) > Main.screenHeight) ? Main.screenHeight - 600 : mapH;
                }

                PosX = Main.screenWidth - 82 - 12 - (47 * 3) - (int)(TextureAssets.InventoryBack.Width() * Main.inventoryScale);
                PosY = (mapH + 174 );
            }
        }
    }

    public class ShieldSlotExtraSlot : ModAccessorySlot // Shield slot mod properties
    {
        public override string Name => "ShieldSlotExtra";
        public override Vector2? CustomLocation => /*ShieldSlotExtra.ShieldConfig.SlotsAlongAccessories ? base.CustomLocation : 不需要*/new Vector2(ShieldSlotExtraUpdateUI.PosX, ShieldSlotExtraUpdateUI.PosY);

        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context) => (checkItem.shieldSlot > 0);

        public override bool ModifyDefaultSwapSlot(Item currItem, int accSlotToSwapTo) => (currItem.shieldSlot > 0);

        public override bool IsVisibleWhenNotEnabled() => false;

        public override string FunctionalTexture => "DeadCells/UI/EquipmentUI/StartShieldSmall";
        public override string FunctionalBackgroundTexture => "DeadCells/UI/Images/BG_Functional";
        public override bool DrawVanitySlot => false;
        public override bool DrawDyeSlot => false;

        public override void OnMouseHover(AccessorySlotType context)
        {
            switch (context) // Text localization for Shield slot
            {
                case AccessorySlotType.FunctionalSlot:
                    Main.hoverItemName = Language.GetTextValue("Mods.ShieldSlotExtra.AccessorySlot.FunctionalSlot");
                    break;
            }
        }
        public override void ApplyEquipEffects()
        {
            base.ApplyEquipEffects();
            var player = Main.LocalPlayer.GetModPlayer<PlayerEquipAndScroll>();
            var parry = Main.LocalPlayer.GetModPlayer<Parry>();
            if (!FunctionalItem.IsAir)//盾栏装备盾牌
            {
                player.CurrentShieldID = FunctionalItem.type;
                parry.ShieldSlotEquipped = true;
            }
            else//盾栏为空
            {
                foreach (Item item in Player.armor)
                {
                    if (item.shieldSlot > 0)//饰品栏装备盾
                    {
                        parry.AccHasShield = true;
                        player.CurrentShieldID = item.type;
                        break;
                    }
                    else//饰品栏未装备盾
                    {
                        player.CurrentShieldID = 0;
                        parry.ShieldSlotEquipped = false;
                        parry.AccHasShield = false;
                    }
                }
            }
        }
    }
}