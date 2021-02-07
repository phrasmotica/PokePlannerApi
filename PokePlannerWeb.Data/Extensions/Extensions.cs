using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PokePlannerWeb.Data.Extensions
{
    /// <summary>
    /// Extension methods that are non-specific.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns this string in title case.
        /// </summary>
        public static string ToTitle(this string st)
        {
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(st);
        }

        /// <summary>
        /// Returns the string as an enum value.
        /// </summary>
        public static T ToEnum<T>(this string st)
        {
            if (string.IsNullOrEmpty(st))
            {
                return default;
            }

            return (T) Enum.Parse(typeof(T), st, true);
        }

        /// <summary>
        /// Creates a finite dictionary with initial key-value pairs.
        /// </summary>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<TKey> keys, TValue initialValue) where TKey : Enum
        {
            return keys.ToDictionary(k => k, _ => initialValue);
        }

        /// <summary>
        /// Returns a new array whose values are the products of corresponding values in the two given arrays.
        /// </summary>
        public static double[] Product(this double[] arr1, double[] arr2)
        {
            // truncate product array to minimum length of input arrays
            var minLength = Math.Min(arr1.Length, arr2.Length);
            var product = arr1[..minLength];

            for (var i = 0; i < product.Length; i++)
            {
                product[i] *= arr2[i];
            }

            return product;
        }

        /// <summary>
        /// Returns a new dictionary whose values are the products of corresponding values in the two given dictionaries.
        /// </summary>
        public static Dictionary<TKey, double> Product<TKey>(this IDictionary<TKey, double> dict1, IDictionary<TKey, double> dict2)
        {
            var product = dict1.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            foreach (var kvp in dict2)
            {
                var key = kvp.Key;
                if (product.ContainsKey(key))
                {
                    product[key] *= kvp.Value;
                }
                else
                {
                    product[key] = kvp.Value;
                }
            }

            return product;
        }
    }
}
