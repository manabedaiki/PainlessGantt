using System.Collections.Generic;
using JetBrains.Annotations;
using YamlDotNet.Serialization;

namespace PainlessGantt.Construction
{
    /// <summary>
    /// <see cref="IProject"/> インターフェイスの読み書き可能な実装を提供します。
    /// </summary>
    public sealed class ProjectBuilder : IProject
    {
        /// <summary>
        /// このプロジェクトの名前を取得または設定します。
        /// </summary>
        [YamlMember(Alias = "プロジェクト")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// このプロジェクトに含まれているチケットの一覧を取得または設定します。
        /// </summary>
        [ItemNotNull]
        [NotNull]
        [YamlMember(Alias = "作業一覧")]
        public List<TicketBuilder> Tickets { get; set; } = new List<TicketBuilder>();

        /// <inheritdoc />
        IReadOnlyCollection<ITicket> IProject.Tickets => Tickets;
    }
}
