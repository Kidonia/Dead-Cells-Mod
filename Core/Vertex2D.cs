using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadCells.Core;
public struct Vertex2D : IVertexType
{
    private static VertexDeclaration _vertexDeclaration = new VertexDeclaration((VertexElement[])(object)new VertexElement[3]
    {
        new VertexElement(0, (VertexElementFormat)1, (VertexElementUsage)0, 0),
        new VertexElement(8, (VertexElementFormat)4, (VertexElementUsage)1, 0),
        new VertexElement(12, (VertexElementFormat)2, (VertexElementUsage)2, 0)
    });

    public Vector2 position;

    public Color color;

    public Vector3 texCoord;

    public VertexDeclaration VertexDeclaration => _vertexDeclaration;

    public Vertex2D(Vector2 position, Color color, Vector3 texCoord)
    {
        this.position = position;
        this.color = color;
        this.texCoord = texCoord;
    }
    //draw ring
}

