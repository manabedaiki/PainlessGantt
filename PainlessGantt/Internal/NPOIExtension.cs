using System;
using JetBrains.Annotations;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

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

        /// <summary>
        /// 図形オブジェクトの線の色を設定します。
        /// </summary>
        /// <param name="shape">図形オブジェクト。</param>
        /// <param name="color">新しく設定する色。</param>
        /// <exception cref="ArgumentNullException"><paramref name="shape"/> または <paramref name="color"/> が <c>null</c> です。</exception>
        public static void SetLineStyleColor([NotNull] this XSSFShape shape, [NotNull] IColor color)
        {
            if (shape == null)
                throw new ArgumentNullException(nameof(shape));
            if (color == null)
                throw new ArgumentNullException(nameof(color));
            shape.SetLineStyleColor(color.R, color.G, color.B);
        }
    }
}
