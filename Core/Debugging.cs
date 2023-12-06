using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace DeadCells.Core
{
    public sealed class DebugSystem : ModSystem
    {

        public static ILog Logger
        {
            get
            {
                ILog result;
                if ((result = DebugSystem.logger) == null)
                {
                    result = (DebugSystem.logger = LogManager.GetLogger("TerrariaOverhaul"));
                }
                return result;
            }
        }

        public static void Log(object text, bool toChat = false, bool toConsole = false, bool toFile = true)
        {
            string actualText = (text != null) ? text.ToString() : null;
            if (toChat)
            {
                Main.NewText(actualText, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            }
            if (toFile)
            {
                DebugSystem.Logger.Info(actualText);
            }
            if (toConsole || (Main.dedServ && toFile))
            {
                Console.WriteLine(actualText);
                bool dedServ = Main.dedServ;
                return;
            }
        }

        internal static void EnableMonoModDumps()
        {
            string dumpDir = Path.GetFullPath("MonoModDump");
            Directory.CreateDirectory(dumpDir);
            Environment.SetEnvironmentVariable("MONOMOD_DMD_DEBUG", "1");
            Environment.SetEnvironmentVariable("MONOMOD_DMD_DUMP", dumpDir);
        }

        internal static void DisableMonoModDumps()
        {
            Environment.SetEnvironmentVariable("MONOMOD_DMD_DEBUG", "0");
            Environment.SetEnvironmentVariable("MONOMOD_DMD_DUMP", null);
        }

        public static bool EnableDebugRendering { get; set; }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Add(new LegacyGameInterfaceLayer("TerrariaOverhaul/Debug", delegate ()
            {
                if (DebugSystem.EnableDebugRendering)
                {
                    for (int i = 0; i < 255; i++)
                    {
                        Player player = Main.player[i];
                        if (player.active)
                        {
                            DebugSystem.DrawRectangle(player.getRect(), Color.SpringGreen, 1);
                        }
                    }
                    for (int j = 0; j < 200; j++)
                    {
                        NPC npc = Main.npc[j];
                        if (npc.active)
                        {
                            DebugSystem.DrawRectangle(npc.getRect(), Color.IndianRed, 1);
                        }
                    }
                    for (int k = 0; k < 1000; k++)
                    {
                        Projectile projectile = Main.projectile[k];
                        if (projectile.active)
                        {
                            DebugSystem.DrawRectangle(projectile.getRect(), Color.Orange, 1);
                        }
                    }
                    for (int l = 0; l < 400; l++)
                    {
                        Item item = Main.item[l];
                        if (item.active)
                        {
                            DebugSystem.DrawRectangle(item.getRect(), Color.DodgerBlue, 1);
                        }
                    }
                    for (int m = 0; m < 600; m++)
                    {
                        Gore gore = Main.gore[m];
                        if (gore.active)
                        {
                            DebugSystem.DrawRectangle(gore.AABBRectangle, Color.Purple, 1);
                        }
                    }
                    foreach (DebugSystem.Line line in this.LinesToDraw)
                    {
                        Vector2 edge = line.end - line.start;
                        Rectangle rect;
                        rect = new Rectangle((int)Math.Round((double)(line.start.X - Main.screenPosition.X)), (int)Math.Round((double)(line.start.Y - Main.screenPosition.Y)), (int)edge.Length(), line.width);
                        Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, rect, null, line.color, (float)Math.Atan2((double)edge.Y, (double)edge.X), Vector2.Zero, 0, 0f);
                    }
                }
                this.LinesToDraw.Clear();
                return true;
            }, InterfaceScaleType.Game));
        }

        public static void DrawLine(Vector2 start, Vector2 end, Color color, int width = 2)
        {
            ModContent.GetInstance<DebugSystem>().LinesToDraw.Add(new DebugSystem.Line(start, end, color, width));
        }

        public static void DrawRectangle(Rectangle rectangle, Color color, int width = 2)
        {
            List<DebugSystem.Line> linesToDraw = ModContent.GetInstance<DebugSystem>().LinesToDraw;
            linesToDraw.Add(new DebugSystem.Line(rectangle.TopLeft(), rectangle.TopRight(), color, width));
            linesToDraw.Add(new DebugSystem.Line(rectangle.TopRight(), rectangle.BottomRight(), color, width));
            linesToDraw.Add(new DebugSystem.Line(rectangle.BottomRight(), rectangle.BottomLeft(), color, width));
            linesToDraw.Add(new DebugSystem.Line(rectangle.BottomLeft(), rectangle.TopLeft(), color, width));
        }

        public static void DrawCircle(Vector2 center, float radius, Color color, int resolution = 16, int width = 2)
        {
		List<Line> lines = ModContent.GetInstance<DebugSystem>().LinesToDraw;
		float step = (float)Math.PI * 2f / (float)resolution;
		Vector2 offset = new (radius, 0f);
		Line line = default(Line);
		for (int i = 0; i <= resolution; i++)
		{
			line.start = center + offset;
			line.end = center + (offset = offset.RotatedBy(step));
			line.color = color;
			line.width = width;
			lines.Add(line);
            }
        }


        private static ILog logger;

        private readonly List<DebugSystem.Line> LinesToDraw = new List<DebugSystem.Line>();

        private struct Line
        {
            public Line(Vector2 start, Vector2 end, Color color, int width)
            {
                this.start = start;
                this.end = end;
                this.color = color;
                this.width = width;
            }

            public Vector2 start;

            public Vector2 end;

            public Color color;

            public int width;
        }
    }
    //roll
}
