namespace PainlessGantt
{
    /// <summary>
    /// RGB 色を表します。
    /// </summary>
    public interface IColor
    {
        /// <summary>
        /// 赤成分を取得します。
        /// </summary>
        byte R { get; }

        /// <summary>
        /// 緑成分を取得します。
        /// </summary>
        byte G { get; }

        /// <summary>
        /// 青成分を取得します。
        /// </summary>
        byte B { get; }
    }
}
