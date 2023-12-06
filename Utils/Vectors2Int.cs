using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace DeadCells.Utils
{
    public struct Vector2Int
    {

        public Vector2Int(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override int GetHashCode()
        {
            return this.X ^ this.Y << 2;
        }


        public override bool Equals(object other)
        {
            if (other is Vector2Int)
            {
                Vector2Int point = (Vector2Int)other;
                if (this.X == point.X)
                {
                    return this.Y == point.Y;
                }
            }
            return false;
        }

        public override string ToString()
        {
            DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 2);
            defaultInterpolatedStringHandler.AppendLiteral("X: ");
            defaultInterpolatedStringHandler.AppendFormatted<int>(this.X);
            defaultInterpolatedStringHandler.AppendLiteral(", Y: ");
            defaultInterpolatedStringHandler.AppendFormatted<int>(this.Y);
            return defaultInterpolatedStringHandler.ToStringAndClear();
        }


        public static Vector2Int Max(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
        }

        public static Vector2Int Min(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
        }


        public static Vector2Int operator *(Vector2Int a, int d)
        {
            return new Vector2Int(a.X * d, a.Y * d);
        }

        public static Vector2Int operator *(int d, Vector2Int a)
        {
            return new Vector2Int(a.X * d, a.Y * d);
        }


        public static Vector2Int operator /(Vector2Int a, int d)
        {
            return new Vector2Int(a.X / d, a.Y / d);
        }


        public static Vector2 operator *(Vector2Int a, float d)
        {
            return new Vector2((float)a.X * d, (float)a.Y * d);
        }


        public static Vector2 operator *(float d, Vector2Int a)
        {
            return new Vector2(d * (float)a.X, d * (float)a.Y);
        }

        public static Vector2 operator /(Vector2Int a, float d)
        {
            return new Vector2((float)a.X / d, (float)a.Y / d);
        }


        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X - b.X, a.Y - b.Y);
        }


        public static Vector2Int operator *(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X * b.X, a.Y * b.Y);
        }

        public static Vector2Int operator /(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X / b.X, a.Y / b.Y);
        }

        public static Vector2Int operator -(Vector2Int a)
        {
            return new Vector2Int(-a.X, -a.Y);
        }

        public static bool operator ==(Vector2Int a, Vector2Int b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2Int a, Vector2Int b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public static Vector2 operator +(Vector2 a, Vector2Int b)
        {
            return new Vector2(a.X + (float)b.X, a.Y + (float)b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2Int b)
        {
            return new Vector2(a.X - (float)b.X, a.Y - (float)b.Y);
        }

        public static Vector2 operator *(Vector2 a, Vector2Int b)
        {
            return new Vector2(a.X * (float)b.X, a.Y * (float)b.Y);
        }

        public static Vector2 operator /(Vector2 a, Vector2Int b)
        {
            return new Vector2(a.X / (float)b.X, a.Y / (float)b.Y);
        }

        public static Vector2 operator +(Vector2Int a, Vector2 b)
        {
            return new Vector2((float)a.X + b.X, (float)a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2Int a, Vector2 b)
        {
            return new Vector2((float)a.X - b.X, (float)a.Y - b.Y);
        }

        public static Vector2 operator *(Vector2Int a, Vector2 b)
        {
            return new Vector2((float)a.X * b.X, (float)a.Y * b.Y);
        }

        public static Vector2 operator /(Vector2Int a, Vector2 b)
        {
            return new Vector2((float)a.X / b.X, (float)a.Y / b.Y);
        }

        public static bool operator ==(Vector2Int a, Vector2 b)
        {
            return (float)a.X == b.X && (float)a.Y == b.Y;
        }

        public static bool operator ==(Vector2 a, Vector2Int b)
        {
            return a.X == (float)b.X && a.Y == (float)b.Y;
        }

        public static bool operator !=(Vector2Int a, Vector2 b)
        {
            return (float)a.X != b.X || (float)a.Y != b.Y;
        }

        public static bool operator !=(Vector2 a, Vector2Int b)
        {
            return a.X != (float)b.X || a.Y != (float)b.Y;
        }

        public static implicit operator Point(Vector2Int value)
        {
            return new Point(value.X, value.Y);
        }

        public static implicit operator Vector2Int(Point value)
        {
            return new Vector2Int(value.X, value.Y);
        }

        public static implicit operator Vector2(Vector2Int value)
        {
            return new Vector2((float)value.X, (float)value.Y);
        }

        public static explicit operator Vector2Int(Vector2 value)
        {
            return new Vector2Int((int)value.X, (int)value.Y);
        }

        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Vector2Int));

        public static readonly Vector2Int Zero = default(Vector2Int);

        public static readonly Vector2Int One = new Vector2Int(1, 1);

        public static readonly Vector2Int UnitX = new Vector2Int(1, 0);

        public static readonly Vector2Int UnitY = new Vector2Int(0, 1);

        public static readonly Vector2Int Up = new Vector2Int(0, 1);

        public static readonly Vector2Int Down = new Vector2Int(0, -1);

        public static readonly Vector2Int Left = new Vector2Int(-1, 0);

        public static readonly Vector2Int Right = new Vector2Int(1, 0);

        public int X;

        public int Y;
    }
}
