using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;

namespace GwApiNET
{

    public static class Extensions
    {
        internal static string Code(this Language value)
        {
            FieldInfo info = value.GetType().GetField(value.ToString());
            var attributes = (LanguageAttribute[]) info.GetCustomAttributes(typeof (LanguageAttribute), false);
            return attributes.Length >= 1 ? attributes[0].Value : value.ToString();
        }

        /// <summary>
        /// Retrieve the <see cref="DescriptionAttribute"/> value
        /// </summary>
        /// <param name="value">The enum value to get the <see cref="DescriptionAttribute"/> of</param>
        /// <returns><see cref="DescriptionAttribute"/> value</returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo info = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[]) info.GetCustomAttributes(typeof (DescriptionAttribute), false);
            return attributes.Length >= 1 ? attributes[0].Description : value.ToString();
        }

        public static Gw2Point ToGw2Point(this int[] point)
        {
            if (point.Length != 2) throw new ArgumentOutOfRangeException("Expected an array of length 2");
            else return new Gw2Point {X = point[0], Y = point[1]};
        }

        public static Gw2Point ToGw2Point(this List<int> point)
        {
            if (point.Count != 2) throw new ArgumentOutOfRangeException("Expected an list of length 2");
            else return new Gw2Point {X = point[0], Y = point[1]};
        }

        public static int EnsureWithin(this int value, int min, int max)
        {
            return value < min
                       ? min
                       : value > max ? max : value;
        }

        public static bool IsBetweenInclusive(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        public static bool IsBetweenInclusive(this double value, double min, double max)
        {
            return value >= min && value <= max;
        }

/*
        public static EntryDictionary<TKey, TElement> ToEntryDictionary<TSource, TKey, TElement>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey> comparer)
        {
            ExceptionHelper.ThrowOnNull(source, "source");
            ExceptionHelper.ThrowOnNull(keySelector, "keySelector");
            ExceptionHelper.ThrowOnNull(elementSelector, "elementSelector");
            EntryDictionary<TKey, TElement> dictionary = new EntryDictionary<TKey, TElement>(comparer);
            foreach (TSource source1 in source)
                dictionary.Add(keySelector(source1), elementSelector(source1));
            return dictionary;
        }
*/

        public static EntryDictionary<TKey, TSource> ToEntryDictionary<TKey, TSource>(this IEnumerable<TSource> enumerable,
                                                                        Func<TSource, TKey> keyselector)
        {
            ExceptionHelper.ThrowOnNull(keyselector, "keyselector");
            EntryDictionary<TKey, TSource> dictionary = new EntryDictionary<TKey, TSource>();
            foreach (TSource element in enumerable)
            {
                dictionary.Add(keyselector(element), element);
            }
            return dictionary;
        }
    }
}
