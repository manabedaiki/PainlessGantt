using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace PainlessGantt
{
    /// <summary>
    /// ガント チャートの設定を管理します。
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// 祝日の一覧を取得します。
        /// </summary>
        [NotNull]
        IReadOnlyCollection<DateTime> Holidays { get; }
    }
}
