using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadCells.Core;
public static class VFXBatchExtension
{

    public static void Draw<T>(this VFXBatch spriteBatch, Texture2D texture, IEnumerable<T> vertices, PrimitiveType type) where T : struct, IVertexType
    {
        spriteBatch.BindTexture<T>(texture).Draw<T>(vertices, type);
    }
    //darw ring
}

