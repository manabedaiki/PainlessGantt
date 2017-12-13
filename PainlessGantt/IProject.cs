using System.Collections.Generic;
using JetBrains.Annotations;

namespace PainlessGantt
{
    /// <summary>
    /// プロジェクトを表します。
    /// </summary>
    public interface IProject
    {
        /// <summary>
        /// このプロジェクトの名前を取得します。
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// このプロジェクトに含まれているチケットの一覧を取得します。
        /// </summary>
        [ItemNotNull]
        [NotNull]
        IReadOnlyCollection<ITicket> Tickets { get; }
    }
}
