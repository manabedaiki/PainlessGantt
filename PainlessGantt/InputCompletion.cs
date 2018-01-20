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
        /// <see cref="IGanttSource"/> のサブプロパティに可能な限り値を設定します。
        /// </summary>
        /// <param name="source">値を設定する対象の <see cref="IGanttSource"/>。</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> が <c>null</c> です。</exception>
        /// <exception cref="NotImplementedException">
        /// 現在、特定の <see cref="IGanttSource"/> 実装クラスのみをサポートしています。
        /// <see cref="GanttSource.Load"/> を使用して作成したオブジェクトを指定してください。
        /// </exception>
        [NotNull]
        public static IGanttSource Complete([NotNull] IGanttSource source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!(source is GanttSourceBuilder sourceBuilder))
                throw new NotImplementedException();
            CompleteBubble(sourceBuilder);
            CompleteTunnel(sourceBuilder);
            return sourceBuilder;
        }

        private static void CompleteBubble([NotNull] GanttSourceBuilder source)
        {
            void Loop(TicketBuilder ticket)
            {
                if (ticket.Children.Count == 0)
                    return;
                foreach (var child in ticket.Children)
                {
                    Loop(child);
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

            foreach (var project in source.Projects)
            {
                foreach (var ticket in project.Tickets)
                {
                    Loop(ticket);
                }
            }
        }

        private static void CompleteTunnel([NotNull] GanttSourceBuilder source)
        {
            void Loop(TicketBuilder ticket, IDateRange range)
            {
                if (ticket.EstimatedPeriod.Start == default)
                {
                    ticket.EstimatedPeriod.Start = range.Start;
                }

                if (ticket.EstimatedPeriod.End == default)
                {
                    ticket.EstimatedPeriod.End = range.End;
                }

                foreach (var child in ticket.Children)
                {
                    Loop(child, ticket.EstimatedPeriod);
                }
            }

            foreach (var project in source.Projects)
            {
                var range = new DateRangeBuilder
                {
                    Start = project.Tickets.Select(x => x.EstimatedPeriod).Minimum(),
                    End = project.Tickets.Select(x => x.EstimatedPeriod).Maximum(),
                };
                foreach (var ticket in project.Tickets)
                {
                    Loop(ticket, range);
                }
            }
        }
    }
}
