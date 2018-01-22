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
    }
}
