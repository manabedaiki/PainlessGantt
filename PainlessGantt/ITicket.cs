using System.Collections.Generic;
using JetBrains.Annotations;

namespace PainlessGantt
{
    /// <summary>
    /// 作業の最小単位を表します。
    /// </summary>
    public interface ITicket
    {
        /// <summary>
        /// このチケットの名前を取得します。
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// このチケットの消化に必要な見積もり期間を取得します。
        /// </summary>
        [NotNull]
        IDateRange EstimatedPeriod { get; }

        /// <summary>
        /// このチケットの消化にかかった実際の期間を取得します。
        /// </summary>
        [NotNull]
        IDateRange ActualPeriod { get; }

        /// <summary>
        /// このチケットの現状の状態を取得します。
        /// </summary>
        TicketStatus Status { get; }

        /// <summary>
        /// このチケットの子チケットを取得します。
        /// </summary>
        [ItemNotNull]
        [NotNull]
        IReadOnlyCollection<ITicket> Children { get; }
    }
}
