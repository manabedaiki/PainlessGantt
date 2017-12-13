using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace PainlessGantt
{
    /// <summary>
    /// 列挙体のフィールドをユーザーに表示する際の表示名を定義します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumDisplayNameAttribute : Attribute
    {
        [NotNull]
        private static readonly ConcurrentDictionary<Type, Dictionary<string, string>> _cache = new ConcurrentDictionary<Type, Dictionary<string, string>>();

        /// <summary>
        /// 指定した列挙値に関連付けられている <see cref="DisplayName"/> を取得します。
        /// </summary>
        /// <typeparam name="TEnum"><see cref="EnumDisplayNameAttribute"/> 属性が宣言された列挙体の型。</typeparam>
        /// <param name="value"><typeparamref name="TEnum"/> 型の列挙値。</param>
        /// <returns><paramref name="value"/> に関連付けられている <see cref="DisplayName"/>。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> が列挙体ではありません。
        /// または、<paramref name="value"/> に関連付けられている <see cref="EnumDisplayNameAttribute"/> 属性が見つかりませんでした。
        /// </exception>
        [NotNull]
        public static string GetDisplayName<TEnum>(TEnum value)
            where TEnum : struct
        {
            var index = _cache.GetOrAdd(typeof(TEnum), type => type
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(x => (name: x.Name, attribute: x.GetCustomAttribute<EnumDisplayNameAttribute>()))
                .ToDictionary(x => x.name, x => x.attribute.DisplayName));
            var fieldName = Enum.GetName(typeof(TEnum), value);
            if (fieldName == null)
                throw new ArgumentException("列挙値ではありません。");
            if (!index.TryGetValue(fieldName, out var displayName))
                throw new FormatException($"列挙値に関連付けられている {nameof(EnumDisplayNameAttribute)} 属性が見つかりませんでした。");
            return displayName;
        }

        /// <summary>
        /// <see cref="EnumDisplayNameAttribute"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="displayName">列挙体の表示名。</param>
        /// <exception cref="ArgumentNullException"><paramref name="displayName"/> が <c>null</c> です。</exception>
        internal EnumDisplayNameAttribute([NotNull] string displayName)
        {
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        }

        /// <summary>
        /// 列挙体の表示名を取得します。
        /// </summary>
        [NotNull]
        public string DisplayName { get; }
    }
}
