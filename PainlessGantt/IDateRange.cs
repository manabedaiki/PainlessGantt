using System;

namespace PainlessGantt
{
    /// <summary>
    /// 日付の範囲を表します。
    /// </summary>
    public interface IDateRange
    {
        /// <summary>
        /// 開始日を取得します。
        /// </summary>
        DateTime Start { get; }

        /// <summary>
        /// 終了日を取得します。
        /// </summary>
        DateTime End { get; }
    }
}
