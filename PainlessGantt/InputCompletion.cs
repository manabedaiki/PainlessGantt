using System;
using System.Linq;
using JetBrains.Annotations;
using PainlessGantt.Construction;

namespace PainlessGantt
{
    /// <summary>
    /// <see cref="IGanttSource"/> のサブプロパティに値を設定します。
    /// </summary>
    public static class InputCompletion
    {
        /// <summary>
        /// ボトム アップ戦略で、<see cref="IGanttSource"/> のサブプロパティに可能な限り値を設定します。
        /// </summary>
        /// <param name="source">値を設定する対象の <see cref="GanttSourceBuilder"/>。</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> が <c>null</c> です。</exception>
        public static void CompleteBubble([NotNull] GanttSourceBuilder source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            foreach (var project in source.Projects)
            {
                foreach (var ticket in project.Tickets)
                {
                    CompleteBubble(ticket);
                }
            }
        }

        private static void CompleteBubble([NotNull] TicketBuilder ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));
            if (ticket.Children.Count == 0)
                return;
            foreach (var child in ticket.Children)
            {
                CompleteBubble(child);
            }
            if (ticket.EstimatedPeriod.Start == default)
            {
                ticket.EstimatedPeriod.Start = ticket.Children.Select(x => x.EstimatedPeriod).Minimum();
            }
            if (ticket.EstimatedPeriod.End == default)
            {
                ticket.EstimatedPeriod.End = ticket.Children.Select(x => x.EstimatedPeriod).Maximum();
            }
            if (ticket.ActualPeriod.Start == default)
            {
                ticket.ActualPeriod.Start = ticket.Children.Select(x => x.ActualPeriod).Minimum();
            }
            if (ticket.ActualPeriod.End == default && ticket.Children.All(x => x.ActualPeriod.End != default))
            {
                ticket.ActualPeriod.End = ticket.Children.Select(x => x.ActualPeriod).Maximum();
            }
        }

        /// <summary>
        /// トップ ダウン戦略で、<see cref="IGanttSource"/> のサブプロパティに可能な限り値を設定します。
        /// </summary>
        /// <param name="source">値を設定する対象の <see cref="GanttSourceBuilder"/>。</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> が <c>null</c> です。</exception>
        public static void CompleteTunnel([NotNull] GanttSourceBuilder source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            foreach (var project in source.Projects)
            {
                var range = new DateRangeBuilder
                {
                    Start = project.Tickets.Select(x => x.EstimatedPeriod).Minimum(),
                    End = project.Tickets.Select(x => x.EstimatedPeriod).Maximum(),
                };
                foreach (var ticket in project.Tickets)
                {
                    CompleteTunnel(ticket, range);
                }
            }
        }

        private static void CompleteTunnel([NotNull] TicketBuilder ticket, [NotNull] IDateRange range)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));
            if (range == null)
                throw new ArgumentNullException(nameof(range));
            if (ticket.EstimatedPeriod.Start == default)
            {
                ticket.EstimatedPeriod.Start = range.Start;
            }
            if (ticket.EstimatedPeriod.End == default)
            {
                ticket.EstimatedPeriod.End = range.Start;
            }
            foreach (var child in ticket.Children)
            {
                CompleteTunnel(child, ticket.EstimatedPeriod);
            }
        }
    }
}
