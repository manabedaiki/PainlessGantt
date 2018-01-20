using YamlDotNet.Serialization;

namespace PainlessGantt.Construction
{
    /// <summary>
    /// <see cref="IColor"/> インターフェイスの読み書き可能な実装を提供します。
    /// </summary>
    public sealed class ColorBuilder : IColor
    {
        /// <summary>
        /// 赤成分を取得または設定します。
        /// </summary>
        [YamlMember(Alias = LocalizationProperties.ColorRKey)]
        public byte R { get; set; }

        /// <summary>
        /// 緑成分を取得または設定します。
        /// </summary>
        [YamlMember(Alias = LocalizationProperties.ColorGKey)]
        public byte G { get; set; }

        /// <summary>
        /// 青成分を取得または設定します。
        /// </summary>
        [YamlMember(Alias = LocalizationProperties.ColorBKey)]
        public byte B { get; set; }
    }
}
