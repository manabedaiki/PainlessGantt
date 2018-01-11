using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace PainlessGantt
{
    /// <summary>
    /// <see cref="ITicket"/> インターフェイスを拡張します。
    /// </summary>
    public static class TicketExtension
    {
        /// <summary>
        /// 指定したチケットとその子チケットを再帰的に列挙します。
        /// </summary>
        /// <param name="ticket">ルート チケット。</param>
        /// <returns><paramref name="ticket"/> とその子チケットを含む <see cref="IEnumerable{T}"/>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ticket"/> が <c>null</c> です。</exception>
        [LinqTunnel]
        [NotNull]
        public static IEnumerable<(ITicket ticket, int nest)> AsEnumerable([NotNull] this ITicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));
            var stack = new Stack<(ITicket ticket, int nest)>();
            stack.Push((ticket, 0));
            while (stack.Count != 0)
            {
                var item = stack.Pop();
                yield return item;
                foreach (var child in item.ticket.Children.Reverse())
                {
                    stack.Push((child, item.nest + 1));
                }
            }
        }

        /// <summary>
        /// <see cref="ITicket.Status"/> が未設定の場合に、<see cref="ITicket"/> の他のプロパティ値をもとに推定された値を取得します。
        /// </summary>
        /// <param name="ticket">チケット。</param>
        /// <returns><paramref name="ticket"/> のプロパティから推定された <see cref="TicketStatus"/>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ticket"/> が <c>null</c> です。</exception>
        public static TicketStatus InferredStatus([NotNull] this ITicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));
            // ステータスが明示的に設定されている場合は、その値を採用する。
            if (ticket.Status != TicketStatus.Unknown)
            {
                return ticket.Status;
            }
            if (ticket.EstimatedPeriod.Start == default || ticket.EstimatedPeriod.End == default)
            {
                return TicketStatus.Unknown;
            }
            if (DateTime.Today < ticket.EstimatedPeriod.Start)
            {
                return TicketStatus.Unknown;
            }
            if (ticket.ActualPeriod.Start != default && ticket.ActualPeriod.End != default)
            {
                return TicketStatus.Completed;
            }
            if (ticket.ActualPeriod.Start != default && DateTime.Today < ticket.EstimatedPeriod.End)
            {
                return TicketStatus.Doing;
            }
            return TicketStatus.Delayed;
        }
    }
}
