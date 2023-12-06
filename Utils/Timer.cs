using System;
using Terraria;

namespace DeadCells.Utils
{
    public struct Timer
    {
        private uint endTime;

        public bool Active => Main.GameUpdateCount < endTime;//Main.GameUpdateCount  是玩家进入世界后累计的时间。一秒为60。

        public int UnclampedValue => (int)(endTime - (long)Main.GameUpdateCount);//用于获取计时器剩余时间的值。用 endTime 减去 Main.GameUpdateCount 得到剩余时间的值，并将其转换为整数。

        public uint Value
        {
            get
            {
                return (uint)Math.Max(0, UnclampedValue);//读取值时，返回 计时器剩余时间的值 和 0 中更大的那个。
            }
            set
            {
                endTime = Main.GameUpdateCount + Math.Max(0u, value);//给Value赋值时（即使用了Value = xxx的时候），把 xxx 和 0 中较大的那个，加上玩家进入世界后当前的时间，一起赋值给endtime。赋值为0时Active为false。
            }
        }

        public void Set(uint minValue)
        {
            Value = Math.Max(minValue, Value);//把 minValue 和 计时器剩余时间的值 和 0  三者中最大的那个，加上玩家进入世界后当前的时间，一起赋值给endtime
        }

        public static implicit operator Timer(uint value)
        {
            Timer result = default(Timer);
            result.Value = value;
            return result;
        }

        public static implicit operator Timer(int value)
        {
            Timer result = default(Timer);
            result.Value = (uint)value;
            return result;
        }
    }

}
