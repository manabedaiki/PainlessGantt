using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using YamlDotNet.Serialization;

namespace PainlessGantt.Construction
{
    /// <summary>
    /// <see cref="IConfiguration"/> インターフェイスの読み書き可能な実装を提供します。
    /// </summary>
    public sealed class ConfigurationBuilder : IConfiguration
    {
        /// <summary>
        /// 祝日の一覧を取得または設定します。
        /// </summary>
        [NotNull]
        [YamlMember(Alias = "祝日")]
        public List<DateTime> Holidays { get; set; } = new List<DateTime>();

        /// <inheritdoc />
        IReadOnlyCollection<DateTime> IConfiguration.Holidays => Holidays;

        /// <summary>
        /// 見積もり期間の線の色を取得または設定します。
        /// </summary>
        [NotNull]
        [YamlMember(Alias = "予定線の色")]
        public ColorBuilder EstimatedLineColor { get; set; } = new ColorBuilder { R = 0xAA, G = 0xAA, B = 0xAA };

        /// <inheritdoc />
        IColor IConfiguration.EstimatedLineColor => EstimatedLineColor;

        /// <summary>
        /// 実際の期間の線の色を取得または設定します。
        /// </summary>
        [NotNull]
        [YamlMember(Alias = "実績線の色")]
        public ColorBuilder ActualLineColor { get; set; } = new ColorBuilder { R = 0x75, G = 0xBF, B = 0xD6 };

        /// <inheritdoc />
        IColor IConfiguration.ActualLineColor => ActualLineColor;
    }
}
