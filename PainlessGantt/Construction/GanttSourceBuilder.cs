using System.Collections.Generic;
using JetBrains.Annotations;
using YamlDotNet.Serialization;

namespace PainlessGantt.Construction
{
    /// <summary>
    /// <see cref="IGanttSource"/> インターフェイスの読み書き可能な実装を提供します。
    /// </summary>
    public sealed class GanttSourceBuilder : IGanttSource
    {
        /// <summary>
        /// このガント チャートの設定を取得または設定します。
        /// </summary>
        [NotNull]
        [YamlMember(Alias = "設定")]
        public ConfigurationBuilder Configuration { get; set; } = new ConfigurationBuilder();

        /// <inheritdoc />
        IConfiguration IGanttSource.Configuration => Configuration;

        /// <summary>
        /// このガント チャートに含まれているプロジェクトの一覧を取得または設定します。
        /// </summary>
        [ItemNotNull]
        [NotNull]
        [YamlMember(Alias = "プロジェクト一覧")]
        public List<ProjectBuilder> Projects { get; set; } = new List<ProjectBuilder>();

        /// <inheritdoc />
        IReadOnlyCollection<IProject> IGanttSource.Projects => Projects;
    }
}
