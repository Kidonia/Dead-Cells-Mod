using DeadCells.Common.Players;
using DeadCells.Core;
using DeadCells.Items;
using DeadCells.Projectiles.EffectProj;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DeadCells.Tiles;
public class TeleportAltar : ModPylon
{
        //if (Main.tile[i, j].TileFrameY < 72)//判断第一帧，才可用
    
    public Asset<Texture2D> mapIcon;
    public Player player => Main.player[Main.myPlayer];
    public PlayerDraw drawplayer => player.GetModPlayer<PlayerDraw>();
    public override void Load()
    {
        mapIcon = ModContent.Request<Texture2D>("DeadCells/Tiles/TeleportAltar_mapIcon");
    }
    public override void SetStaticDefaults()
    {

        // Properties
        Main.tileHammer[Type] = true;
        Main.tileNoSunLight[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileSpelunker[Type] = true;
        Main.tileShine2[Type] = true;
        DustType = 47;
        // Placement
        TileObjectData.newTile.LavaDeath = false;
        TileObjectData.newTile.UsesCustomCanPlace = true;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
        TileObjectData.newTile.StyleHorizontal = false;
        TileObjectData.newTile.StyleLineSkip = 2;
        TeleportTileEntity teleportTile = ModContent.GetInstance<TeleportTileEntity>();
        TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(teleportTile.PlacementPreviewHook_CheckIfCanPlace, 1, 0, true);
        TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(teleportTile.Hook_AfterPlacement, -1, 0, false);
        
        TileObjectData.addTile(Type);

        TileID.Sets.InteractibleByNPCs[Type] = true;
        TileID.Sets.PreventsSandfall[Type] = true;
        TileID.Sets.AvoidedByMeteorLanding[Type] = true;
        TileID.Sets.DrawsWalls[Type] = true;
        
        AddToArray(ref TileID.Sets.CountsAsPylon);

        LocalizedText pylonName = CreateMapEntryName();
        AddMapEntry(new(50, 80, 98), pylonName);
    }
    public override bool CanPlacePylon()//没有放置限制
    {
        return true;
    }
    public override bool RightClick(int i, int j)
    {
        Point16 playerPoint = player.Center.ToTileCoordinates16();

        if (playerPoint.X == i && playerPoint.Y == j && !Main.playerInventory)//玩家站在前面，且 没有打开背包
        {
            Main.mapFullscreen = true;
            SoundEngine.PlaySound(SoundID.MenuOpen);
            return true;
        }
        else return false;
    }

    public override void MouseOver(int i, int j)
    {
        if (drawplayer.can_teleport && !Main.playerInventory && Main.tile[i, j].TileFrameY < 72)//玩家站在前面且 可用，且 没有打开背包，
        {
            Main.LocalPlayer.cursorItemIconEnabled = true;
            Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<TestTeleprot>();
        }
    }
    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        ModContent.GetInstance<TeleportTileEntity>().Kill(i, j);
    }
    public override void DrawMapIcon(ref MapOverlayDrawContext context, ref string mouseOverText, TeleportPylonInfo pylonInfo, bool isNearPylon, Color drawColor, float deselectedScale, float selectedScale)
    {
        if (!TileEntity.ByPosition.TryGetValue(pylonInfo.PositionInTiles, out var te) || te is not TeleportTileEntity)
        {
            return;
        }

        bool unlocked = Main.tile[te.Position.X, te.Position.Y].TileFrameY < 72;

        if (!unlocked)
            drawColor = Color.Transparent;//未激活不绘制
        else if (!drawplayer.can_teleport)//不站在前面变灰
                drawColor = Color.Gray * 0.4f;

        bool mouseOver = DefaultDrawMapIcon(ref context, mapIcon, pylonInfo.PositionInTiles.ToVector2() + new Vector2(2.5f, 2f), drawColor, deselectedScale, selectedScale);
        MapClickHandle(mouseOver && unlocked, pylonInfo, "", ref mouseOverText);
    }




    public override bool ValidTeleportCheck_NPCCount(TeleportPylonInfo pylonInfo, int defaultNecessaryNPCCount)
    {
        return true;//无视npc数量
    }

    public override bool ValidTeleportCheck_AnyDanger(TeleportPylonInfo pylonInfo)
    {
        return true;//无视风险
    }
    public override bool ValidTeleportCheck_BiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData)
    {
        return true;//无视地形
    }
    public override void ValidTeleportCheck_DestinationPostCheck(TeleportPylonInfo destinationPylonInfo, ref bool destinationPylonValid, ref string errorKey)
    { 
        if (TileEntity.ByPosition[destinationPylonInfo.PositionInTiles] is TeleportTileEntity entity && Main.tile[entity.Position.X, entity.Position.Y].TileFrameY >= 72)
        {
            destinationPylonValid = false;
            errorKey = "未启动";
        }
    }
    
    public override void ValidTeleportCheck_NearbyPostCheck(TeleportPylonInfo nearbyPylonInfo, ref bool destinationPylonValid, ref bool anyNearbyValidPylon, ref string errorKey)
    {
        destinationPylonValid = true;
    }
    public override void ModifyTeleportationPosition(TeleportPylonInfo destinationPylonInfo, ref Vector2 teleportationPosition)
    {
        teleportationPosition = destinationPylonInfo.PositionInTiles.ToWorldCoordinates(32f, -8f);//传送位置偏移
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)//发光
    {
        if (Main.tile[i, j].TileFrameY < 72)
        {
            r = 1f;
            g = 190 / 255f;
            b = 86 / 255f;
        }
        else
        {
            r = 0f; g = 0f; b = 0f;
        }
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Point16 tilePoint = new(i, j);
        float distance = Vector2.Distance(tilePoint.ToWorldCoordinates(), player.Center);

        if (distance < 220)
        {

            //为什么是72？因为我们的贴图一帧的总宽度为4*18=72，中间空的也需要算上
            short changeFrames = 72;
            //这两个用于表示玩家右键到的时多物块的具体哪一块，通过这个来得到物块左上角
            //同时用X去判断我们的灯笼当前处于第一帧还是第二帧
            //TileFrameX和TileFrameY的单位都是像素，与你的贴图是完全对应的
            int mouse2TopLeftX = Main.tile[i, j].TileFrameX / 18 * -1;
            int mouse2TopLeftY = Main.tile[i, j].TileFrameY / 18 * -1;
            //如果小于-3那么就代表我们的灯笼现在处于第二帧
            if (mouse2TopLeftY < -3)
            {

                mouse2TopLeftY += 4;
                //加上i和j以得到这个多物块的左上角位置
                mouse2TopLeftX += i;
                mouse2TopLeftY += j;
                //从左上角向右下角遍历物块，改变它们的帧图
                for (int x = mouse2TopLeftX; x < mouse2TopLeftX + 5; x++)
                    for (int y = mouse2TopLeftY; y < mouse2TopLeftY + 4; y++)
                    {
                        if (Main.tile[x, y].TileType == Type)
                            Main.tile[x, y].TileFrameY -= changeFrames;

                        if (x - mouse2TopLeftX == 2 && y - mouse2TopLeftY == 2)
                        {
                            Vector2 pos = new Point16(x, y).ToWorldCoordinates();
                            //生成启动时的粒子与粒子
                            Projectile.NewProjectile(player.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<TeleportAvailable>(), 0, 0);
                        }
                    }

            }
        }
            
        if (Main.tile[i, j].TileFrameY < 72)//第一帧
        {
            Tile tile = Main.tile[i, j];

            Texture2D glowTexture = ModContent.Request<Texture2D>("DeadCells/Tiles/TeleportAltar_glow").Value;

            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            int width = 16;
            int offsetY = 0;
            int height = 16;
            short frameX = tile.TileFrameX;
            short frameY = tile.TileFrameY;
            int addFrX = 0;
            int addFrY = 0;

            TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY);
            TileLoader.SetAnimationFrame(Type, i, j, ref addFrX, ref addFrY);
            Rectangle drawRectangle = new(tile.TileFrameX, tile.TileFrameY + addFrY, 16, 16);


            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
            spriteBatch.Draw(
                glowTexture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                drawRectangle,
                Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
    public override void NumDust(int x, int y, bool fail, ref int num)
    {
        num = fail ? 5 : 10;
    }
    private void MapClickHandle(bool mouseIsHovering, TeleportPylonInfo pylonInfo, string hoveringTextKey, ref string mouseOverText)
    {
        if (mouseIsHovering)
        {
            Main.cancelWormHole = true;
            mouseOverText = Language.GetTextValue(hoveringTextKey);
            if (Main.mouseLeft && Main.mouseLeftRelease)
            {
                Main.mouseLeftRelease = false;
                Main.mapFullscreen = false;
                PlayerInput.LockGamepadButtons("MouseLeft");
                if (!drawplayer.can_teleport)
                {
                    Main.NewText("请站在传送阵前。", Color.Yellow);
                    return;
                }
                drawplayer.teleport_charge += 86;
                player.velocity *= Vector2.Zero;
                drawplayer.destinationPylonInfo = pylonInfo;
                //生成屏幕效果
                Projectile.NewProjectile(player.GetSource_FromAI(), Main.screenPosition, Vector2.Zero, ModContent.ProjectileType<TeleportScreenScale>(), 0, 0);

                SoundEngine.PlaySound(AssetsLoader.portal_use1);

            }
        }
    }

}


