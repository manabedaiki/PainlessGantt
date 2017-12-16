using System;
using JetBrains.Annotations;
using NPOI.SS.UserModel;

namespace PainlessGantt.Internal
{
    /// <summary>
    /// <see cref="NPOI"/> を拡張します。
    /// </summary>
    internal static class NPOIExtension
    {
        /// <summary>
        /// セルに日付型の値を設定します。
        /// 日付が規定値 (<c>default</c>) の場合は <c>null</c> を設定します。
        /// </summary>
        /// <param name="cell">値を設定する対象のセル。</param>
        /// <param name="value">設定する日付。</param>
        /// <exception cref="ArgumentNullException"><paramref name="cell"/> が <c>null</c> です。</exception>
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
