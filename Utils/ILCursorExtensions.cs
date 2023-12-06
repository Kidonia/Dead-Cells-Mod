using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using log4net;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using DeadCells.Core;

namespace DeadCells.Utils
{
    internal static class ILCursorExtensions
    {
        public static ILCursor HijackIncomingLabels(this ILCursor cursor)
        {
            //IL_000c: Unknown result type (might be due to invalid IL or missing references)
            ILLabel[] array = cursor.IncomingLabels.ToArray();
            cursor.Emit(OpCodes.Nop);
            ILLabel[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                array2[i].Target = cursor.Prev;
            }
            return cursor;
        }

        public static Func<Instruction, bool>?[] CreateDebugInstructionPredicates(this ILCursor _, Expression<Func<Instruction, bool>>?[] expressions)
        {
            Func<Instruction, bool>[] result = new Func<Instruction, bool>[expressions.Length];
            for (int j = 0; j < expressions.Length; j++)
            {
                Expression<Func<Instruction, bool>> expression = expressions[j];
                if (expression == null)
                {
                    continue;
                }
                string expressionText = expression.ToString();
                Func<Instruction, bool> predicate = expression.Compile();
                result[j] = delegate (Instruction i)
                {
                    bool flag = predicate(i);
                    if (!flag)
                    {
                        ILog logger = DebugSystem.Logger;
                        DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 3);
                        defaultInterpolatedStringHandler.AppendLiteral("Expression '");
                        defaultInterpolatedStringHandler.AppendFormatted(expressionText);
                        defaultInterpolatedStringHandler.AppendLiteral("' returned false on instruction '");
                        defaultInterpolatedStringHandler.AppendFormatted<int>(i.Offset, "x4");
                        defaultInterpolatedStringHandler.AppendLiteral("' (");
                        defaultInterpolatedStringHandler.AppendFormatted<OpCode>(i.OpCode);
                        defaultInterpolatedStringHandler.AppendLiteral(").");
                        logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear());
                    }
                    return flag;
                };
            }
            return result;
        }
    }

}
