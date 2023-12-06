using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;

namespace DeadCells.Utils;

public struct CustomVertexInfo : IVertexType
{
    private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
    {
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
    });
    /// <summary>
    /// 绘制位置(世界坐标)
    /// </summary>
    public Vector2 Position;
    /// <summary>
    /// 绘制的颜色
    /// </summary>
    public Color Color;
    /// <summary>
    /// 前两个是纹理坐标，最后一个是自定义的
    /// </summary>
    public Vector3 TexCoord;

    public CustomVertexInfo(Vector2 position, Color color, Vector3 texCoord)
    {
        this.Position = position;
        this.Color = color;
        this.TexCoord = texCoord;
    }

    public VertexDeclaration VertexDeclaration => _vertexDeclaration;
}
public class Rope_Point
{
    public Vector2 pos, oldpos;
    public Rope_Point(Vector2 vector)
    {
        pos = vector;
        oldpos = vector;
    }

    public bool locked;
}
public class Rope_Line
{
    public Rope_Point startPoint, endPoint;
    public float Length;
    public Rope_Line(Rope_Point startPoint, Rope_Point endPoint, float length)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.Length = length;
    }
}

public class RopePhysicalEffects
{
    public static int RopeRigidity = 4;//绳刚性
    public static Vector2 gravity = new(0, 10);
    public static void VerletObjPosiUpdate(List<Rope_Point> rp, List<Rope_Line> rl, Vector2 endPos, Vector2 headPos)
    {
        for (int k = 1; k < rp.Count - 1; k++)
        {
            rp[k].locked = false;
        }
        if (rp.Count > 1)
        {
            rp[rp.Count - 1].locked = true;
            rp[0].pos = headPos;
            rp[rp.Count - 1].pos = endPos;
        }

        for (int i = 0; i < rp.Count; i++)
        {
            Rope_Point p = rp[i];
            if (!p.locked)
            {
                Vector2 vector = p.pos;
                p.pos += p.pos - p.oldpos + gravity;
                p.oldpos = vector;
            }
        }
        for (int k = 0; k < RopeRigidity; k++)
        {
            for (int i = 0; i < rl.Count; i++)
            {
                Rope_Line rope = rl[i];
                Vector2 endToStart = rope.endPoint.pos - rope.startPoint.pos;
                float length = endToStart.Length();
                length = (length - rope.Length) / length;
                if (!rope.startPoint.locked)
                    rope.startPoint.pos += 0.5f * endToStart * length;
                if (!rope.endPoint.locked)
                    rope.endPoint.pos -= 0.5f * endToStart * length;

            }
        }
    }
    public static void DrawVerletObj(List<Rope_Line> rl, Texture2D texture)
    {
        Vector2 origin = new Vector2(texture.Width, texture.Height) / 2f;
        for (int i = 0; i < rl.Count; i++)
        {
            Rope_Line line = rl[i];
            Vector2 lineVec = line.endPoint.pos - line.startPoint.pos;
            int length = (int)lineVec.Length();
            float rotation = lineVec.ToRotation() + MathHelper.PiOver2;
            Vector2 drawPos = line.startPoint.pos - Main.screenPosition + lineVec / 2;

            Rectangle destinationRectangle = new Rectangle((int)drawPos.X, (int)drawPos.Y, 40, length);

            Main.spriteBatch.Draw(texture, destinationRectangle, null, Color.White, rotation, origin, 0, 0);
            //Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, drawPos, Color.Red);
        }

    }
    public static void DrawPointPos(List<Rope_Point> rp)
    {
        foreach (Rope_Point p in rp)
        {
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, p.pos - Main.screenPosition, Color.White);
        }
    }
    public static void AddVerletObj(List<Rope_Point> rp, List<Rope_Line> rl, Vector2 spawnPos)
    {

        if (rp.Count == 0)
        {
            Rope_Point startPoint = new Rope_Point(Main.LocalPlayer.position);
            startPoint.locked = true;
            rp.Add(startPoint);
        }
        if (rp.Count > 0)
        {
            Rope_Point point = new Rope_Point(spawnPos);
            rp.Add(point);
            Rope_Line line = new Rope_Line(rp[rp.Count - 2], rp[rp.Count - 1], 5f);
            rl.Add(line);
        }

    }
    public static void KillAllVerletObj(List<Rope_Point> rp, List<Rope_Line> rl)
    {
        rp.Clear();
        rl.Clear();
    }

    /*
    public void Update()
    {
        myAddVerletObj();
        for (int i = 0; i < 6; i++)
        {
            myPosUpdate();
        }
    }
    */
}
public class NormalUtils
{
    private List<int> DCnpcID = null;

    public static int Rand1or_1()
    {
        return Main.rand.Next(0, 2) * 2 - 1;
    }

}
