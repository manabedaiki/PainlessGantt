using System.Collections.Generic;
using JetBrains.Annotations;
using YamlDotNet.Serialization;

namespace PainlessGantt.Construction
{
    /// <summary>
    /// <see cref="ITicket"/> インターフェイスの読み書き可能な実装を提供します。
    /// </summary>
    public sealed class TicketBuilder : ITicket
    {
        /// <summary>
        /// このチケットの名前を取得または設定します。
        /// </summary>
        [YamlMember(Alias = LocalizationProperties.TicketNameKey)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// このチケットの消化に必要な見積もり期間を取得または設定します。
        /// </summary>
        [YamlMember(Alias = LocalizationProperties.TicketEstimatedPeriodKey)]
        public DateRangeBuilder EstimatedPeriod { get; set; } = new DateRangeBuilder();

        /// <inheritdoc />
        IDateRange ITicket.EstimatedPeriod => EstimatedPeriod;

        /// <summary>
        /// このチケットの消化にかかった実際の期間を取得または設定します。
        /// </summary>
        [YamlMember(Alias = LocalizationProperties.TicketActualPeriodKey)]
        public DateRangeBuilder ActualPeriod { get; set; } = new DateRangeBuilder();

        /// <inheritdoc />
        IDateRange ITicket.ActualPeriod => ActualPeriod;

        /// <summary>
        /// このチケットの現状の状態を取得または設定します。
        /// </summary>
        [YamlMember(Alias = LocalizationProperties.TicketStatusKey)]
        public TicketStatus Status { get; set; } = TicketStatus.Unknown;

        /// <summary>
        /// このチケットの子チケットを取得または設定します。
        /// </summary>
        [ItemNotNull]
        [NotNull]
        [YamlMember(Alias = LocalizationProperties.TicketChildrenKey)]
        public List<TicketBuilder> Children { get; set; } = new List<TicketBuilder>();

        /// <inheritdoc />
        IReadOnlyCollection<ITicket> ITicket.Children => Children;
    }
}
