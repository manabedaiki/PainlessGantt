using System;
using JetBrains.Annotations;
using NPOI.SS.UserModel;

namespace PainlessGantt.Internal
{
    internal static class NPOIExtension
    {
        public static void SetCellValueOrDefault([NotNull] this ICell cell, DateTime value)
        {
            if (cell == null)
                throw new ArgumentNullException(nameof(cell));
            if (value == default)
            {
                cell.SetCellValue((string)null);
            }
            else
            {
                cell.SetCellValue(value);
            }
        }
    }
}
