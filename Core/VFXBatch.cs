#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XPT.Core.Audio.MP3Sharp.Decoding;

namespace DeadCells.Core;

    public class VFXBatch : IDisposable
    {
        private bool hasBegun = false;
        private struct VFX2D : IVertexType
        {
            public Color color;

            public Vector2 position;

            public Vector2 texCoord;

            public VertexDeclaration VertexDeclaration => new VertexDeclaration((VertexElement[])(object)new VertexElement[3]
            {
                new VertexElement(0, (VertexElementFormat)1, (VertexElementUsage)0, 0),
                new VertexElement(8, (VertexElementFormat)4, (VertexElementUsage)1, 0),
                new VertexElement(12, (VertexElementFormat)1, (VertexElementUsage)2, 0)
            });

            public VFX2D(Vector2 position, Color color, Vector2 texCoord)
            {
                this.position = position;
                this.color = color;
                this.texCoord = texCoord;
            }
        }

        private interface IBuffers : IDisposable
            {
                Type VertexType { get; }

                void Clear();

                void DrawPrimitive();
            }


        private static class Buffer<T> where T : struct, IVertexType
        {
            private class Buffers : IBuffers, IDisposable
            {
                public GraphicsDevice graphicsDevice;

                public DynamicIndexBuffer indexBuffer;

                public int indexPosition;

                public int[] indices;

                public Queue<(int index, int vertex)> sameTexture;

                public List<Texture2D> textures;

                public DynamicVertexBuffer vertexBuffer;

                public int vertexPosition;

                public T[] vertices;

                public Type VertexType => typeof(T);

                public void Clear()
                {
                    vertexPosition = (indexPosition = 0);
                    sameTexture.Clear();
                    textures.Clear();
                }



                public void Dispose()
                {
                    ((GraphicsResource)vertexBuffer).Dispose();
                    ((GraphicsResource)indexBuffer).Dispose();
                    GC.SuppressFinalize(this);
                }

                public void DrawPrimitive()
                {
                    Debug.Assert(vertexPosition != 0 && indexPosition != 0, "Should not draw when no vertex");
                    vertexBuffer.SetData<T>(vertices, 0, vertexPosition, (SetDataOptions)0);
                    graphicsDevice.SetVertexBuffer((VertexBuffer)(object)vertexBuffer);
                    indexBuffer.SetData<int>(indices, 0, indexPosition, (SetDataOptions)0);
                    graphicsDevice.Indices = (IndexBuffer)(object)indexBuffer;
                    if (sameTexture.Count == 0)
                    {
                        if (textures.Count != 0)
                        {
                            graphicsDevice.Textures[0] = (Texture)(object)textures[0];
                        }
                        graphicsDevice.DrawIndexedPrimitives((PrimitiveType)0, 0, 0, vertexPosition, 0, indexPosition / 3);
                        return;
                    }
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    sameTexture.Enqueue((indexPosition, vertexPosition));
                    while (sameTexture.Count != 0)
                     {
                        var (num4, num5) = sameTexture.Dequeue();
                        if (num2 != num5 && num != num4)
                        {
                            graphicsDevice.Textures[0] = (Texture)(object)textures[num3++];
                            graphicsDevice.DrawIndexedPrimitives((PrimitiveType)0, 0, 0, num5, num, (num4 - num) / 3);
                            int num6 = num5;
                            num = num4;
                            num2 = num6;
                        }
                     }
                }
            }
            private static Buffers instance;
            public static Texture2D CurrentTexture
            {
                get
                {
                    Debug.Assert(instance.textures.Count != 0);
                    List<Texture2D> textures = instance.textures;
                    return textures[textures.Count - 1];
                }
            }
            public static ref int IndexPosition => ref instance.indexPosition;
            public static IBuffers Instance => instance;
            public static Queue<(int index, int vertex)> SameTexture => instance.sameTexture;
            public static ref int VertexPosition => ref instance.vertexPosition;
            public static List<Texture2D> Textures => instance.textures;
            public static void AddVertex(IEnumerable<T> vertices, PrimitiveType type)
            {
                int vertexPosition = instance.vertexPosition;
                int num = 0;
                foreach (T vertex in vertices)
                {
                    num++;
                    instance.vertices[vertexPosition++] = vertex;
                }
                if ((int)type != 0)
                {
                    if ((int)type != 1)
                    {
                        throw new Exception("Unsupported PrimitiveType");
                    }
                    for (int i = 0; i < num - 2; i++)
                    {
                        instance.indices[instance.indexPosition++] = i + instance.vertexPosition;
                        instance.indices[instance.indexPosition++] = i + 1 + instance.vertexPosition;
                        instance.indices[instance.indexPosition++] = i + 2 + instance.vertexPosition;
                    }
                }
                else
                {
                    for (int j = 0; j < num; j++)
                    {
                        instance.indices[instance.indexPosition++] = j + instance.vertexPosition;
                    }
                }
                instance.vertexPosition = vertexPosition;
            }
        public static bool CheckSize(int vertexSize)
            {
                return instance.vertexPosition + vertexSize < instance.vertices.Length;
            }






        }
       



        private List<bool> needFlush = new List<bool>();
        private List<IBuffers> buffers = new List<IBuffers>();
        public void Flush<T>() where T : struct, IVertexType
        {
            Buffer<T>.Instance.DrawPrimitive();
            Buffer<T>.Instance.Clear();
        }
        public void Draw<T>(IEnumerable<T> vertices, PrimitiveType type) where T : struct, IVertexType
        {
            if (!vertices.Any())
            {
                return;
            }
            Debug.Assert(hasBegun, "Begin not called!");
            if (!Buffer<T>.CheckSize(vertices.Count()))
            {
                if (Buffer<T>.Textures.Count == 0)
                {
                    Flush<T>();
                }
                else
                {
                    Texture2D currentTexture = Buffer<T>.CurrentTexture;
                    Flush<T>();
                    Buffer<T>.Textures.Add(currentTexture);
                }
            }
            needFlush[GetBufferIndex<T>()] = true;
            Buffer<T>.AddVertex(vertices, type);
        }
        public void Dispose()
        {
            foreach (IBuffers buffer in buffers)
            {
                buffer.Dispose();
            }
            GC.SuppressFinalize(this);
        }
        private int GetBufferIndex<T>() where T : struct, IVertexType
        {
            return buffers.IndexOf(Buffer<T>.Instance);
        }
        public VFXBatch BindTexture(Texture2D texture)
        {
            return this.BindTexture<VFX2D>(texture);
        }
        public VFXBatch BindTexture<T>(Texture2D texture) where T : struct, IVertexType
        {
            if (Buffer<T>.Textures.Count == 0)
            {
                Buffer<T>.Textures.Add(texture);
                return this;
            }
            if (Buffer<T>.CurrentTexture == texture)
            {
                return this;
            }
            Buffer<T>.Textures.Add(texture);
            Buffer<T>.SameTexture.Enqueue((Buffer<T>.IndexPosition, Buffer<T>.VertexPosition));
            return this;
        }
    //draw ring
}