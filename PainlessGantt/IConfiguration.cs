using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace PainlessGantt
{
    /// <summary>
    /// ガント チャートの設定を管理します。
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// 祝日の一覧を取得します。
        /// </summary>
        [NotNull]
        IReadOnlyCollection<DateTime> Holidays { get; }

        /// <summary>
        /// 見積もり期間の線の色を取得します。
        /// </summary>
        /// <seealso cref="ITicket.EstimatedPeriod"/>
        [NotNull]
        IColor EstimatedLineColor { get; }

        /// <summary>
        /// 実際の期間の線の色を取得します。
        /// </summary>
        /// <seealso cref="ITicket.ActualPeriod"/>
        [NotNull]
        IColor ActualLineColor { get; }

        /// <summary>
        /// 遅延している期間の線の色を取得します。
        /// </summary>
        /// <seealso cref="ITicket.EstimatedPeriod"/>
        /// <seealso cref="ITicket.ActualPeriod"/>
        [NotNull]
        IColor DelayLineColor { get; }
    }
}
