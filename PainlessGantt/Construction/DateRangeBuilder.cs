using System;
using YamlDotNet.Serialization;

namespace PainlessGantt.Construction
{
    /// <summary>
    /// <see cref="IDateRange"/> インターフェイスの読み書き可能な実装を提供します。
    /// </summary>
    public sealed class DateRangeBuilder : IDateRange
    {
        /// <summary>
        /// 開始日を取得または設定します。
        /// </summary>
        [YamlMember(Alias = LocalizationProperties.DateRangeStartKey)]
        public DateTime Start { get; set; }

        /// <summary>
        /// 終了日を取得または設定します。
        /// </summary>
        [YamlMember(Alias = LocalizationProperties.DateRangeEndKey)]
        public DateTime End { get; set; }
    }
}
