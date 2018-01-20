using JetBrains.Annotations;

namespace PainlessGantt.Construction
{
    internal static class LocalizationProperties
    {
        /// <summary>
        /// <see cref="GanttSourceBuilder.Configuration"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string GanttSourceConfigurationKey = "設定";

        /// <summary>
        /// <see cref="GanttSourceBuilder.Projects"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string GanttSourceProjectsKey = "プロジェクト一覧";

        /// <summary>
        /// <see cref="ConfigurationBuilder.Holidays"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string ConfigurationHolidaysKey = "祝日";

        /// <summary>
        /// <see cref="ConfigurationBuilder.EstimatedLineColor"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string ConfigurationEstimatedLineColorKey = "予定線の色";

        /// <summary>
        /// <see cref="ConfigurationBuilder.ActualLineColor"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string ConfigurationActualLineColorKey = "実績線の色";

        /// <summary>
        /// <see cref="ProjectBuilder.Name"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string ProjectNameKey = "プロジェクト";

        /// <summary>
        /// <see cref="ProjectBuilder.Tickets"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string ProjectTicketsKey = "作業一覧";

        /// <summary>
        /// <see cref="TicketBuilder.Name"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string TicketNameKey = "作業";

        /// <summary>
        /// <see cref="TicketBuilder.EstimatedPeriod"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string TicketEstimatedPeriodKey = "予定";

        /// <summary>
        /// <see cref="TicketBuilder.ActualPeriod"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string TicketActualPeriodKey = "実績";

        /// <summary>
        /// <see cref="TicketBuilder.Status"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string TicketStatusKey = "状態";

        /// <summary>
        /// <see cref="TicketBuilder.Children"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string TicketChildrenKey = "詳細";

        /// <summary>
        /// <see cref="DateRangeBuilder.Start"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string DateRangeStartKey = "開始";

        /// <summary>
        /// <see cref="DateRangeBuilder.End"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string DateRangeEndKey = "終了";

        /// <summary>
        /// <see cref="ColorBuilder.R"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string ColorRKey = "赤";

        /// <summary>
        /// <see cref="ColorBuilder.G"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string ColorGKey = "緑";

        /// <summary>
        /// <see cref="ColorBuilder.B"/> をシリアル化する際に使用するキー。
        /// </summary>
        [NotNull]
        public const string ColorBKey = "青";
    }
}
