using System;
using System.Collections.Generic;
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
        [YamlMember(Alias = "祝日")]
        public List<DateTime> Holidays { get; set; } = new List<DateTime>();

        /// <inheritdoc />
        IReadOnlyCollection<DateTime> IConfiguration.Holidays => Holidays;
    }
}
