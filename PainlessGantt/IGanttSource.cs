using System.Collections.Generic;
using JetBrains.Annotations;

namespace PainlessGantt
{
    /// <summary>
    /// ガント チャートの作成に必要なデータを管理します。
    /// </summary>
    public interface IGanttSource
    {
        /// <summary>
        /// このガント チャートの設定を取得します。
        /// </summary>
        [NotNull]
        IConfiguration Configuration { get; }

        /// <summary>
        /// このガント チャートに含まれているプロジェクトの一覧を取得します。
        /// </summary>
        [ItemNotNull]
        [NotNull]
        IReadOnlyCollection<IProject> Projects { get; }
    }
}
