using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace PainlessGantt
{
    /// <summary>
    /// <see cref="IDateRange"/> インターフェイスを拡張します。
    /// </summary>
    public static class DateRangeExtension
    {
        /// <summary>
        /// 指定した日付の範囲に含まれている日を列挙します。
        /// </summary>
        /// <param name="range">日付の範囲。</param>
        /// <returns><paramref name="range"/> に含まれているすべての日を順番に含むシーケンス。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="range"/> が <c>null</c> です。</exception>
        [LinqTunnel]
        [NotNull]
        public static IEnumerable<DateTime> AsEnumerable([NotNull] this IDateRange range)
        {
            if (range == null)
                throw new ArgumentNullException(nameof(range));
            var daysInRange = (range.End - range.Start).Days + 1;
            return Enumerable.Range(0, daysInRange).Select(x => range.Start.AddDays(x));
        }

        /// <summary>
        /// 指定した <see cref="IDateRange"/> のシーケンスに含まれている日付の最小値を取得します。
        /// </summary>
        /// <param name="source"><see cref="IDateRange"/> のシーケンス。</param>
        /// <returns><paramref name="source"/> に含まれている日付の最小値。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> が <c>null</c> です。</exception>
        public static DateTime Minimum([ItemNotNull] [NotNull] this IEnumerable<IDateRange> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            return source.Where(x => x.Start != default).Select(x => x.Start).DefaultIfEmpty().Min();
        }

        /// <summary>
        /// 指定した <see cref="IDateRange"/> のシーケンスに含まれている日付の最大値を取得します。
        /// </summary>
        /// <param name="source"><see cref="IDateRange"/> のシーケンス。</param>
        /// <returns><paramref name="source"/> に含まれている日付の最大値。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> が <c>null</c> です。</exception>
        public static DateTime Maximum([ItemNotNull] [NotNull] this IEnumerable<IDateRange> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            return source.Where(x => x.End != default).Select(x => x.End).DefaultIfEmpty().Max();
        }
    }
}
