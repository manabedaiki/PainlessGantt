namespace PainlessGantt
{
    /// <summary>
    /// チケットの状態を識別します。
    /// </summary>
    public enum TicketStatus
    {
        /// <summary>
        /// 不明。
        /// </summary>
        [EnumDisplayName("不明")]
        Unknown,

        /// <summary>
        /// 着手。
        /// </summary>
        [EnumDisplayName("着手")]
        Doing,

        /// <summary>
        /// 遅延。
        /// </summary>
        [EnumDisplayName("遅延")]
        Delayed,

        /// <summary>
        /// 中断。
        /// </summary>
        [EnumDisplayName("中断")]
        Suspended,

        /// <summary>
        /// 完了。
        /// </summary>
        [EnumDisplayName("完了")]
        Completed,
    }
}
