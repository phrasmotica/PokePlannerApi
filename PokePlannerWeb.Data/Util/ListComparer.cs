using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PokePlannerWeb.Data.Util
{
    /// <summary>
    /// Comparer for lists of comparable objects.
    /// </summary>
    public class ListComparer<T> : IEqualityComparer<List<T>>
    {
        /// <summary>
        /// The equals function.
        /// </summary>
        private readonly Func<T, T, bool> EqualsFunc;

        /// <summary>
        /// The hash function.
        /// </summary>
        private readonly Func<T, int> HashFunc;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="equalsFunc">Function to use for comparing objects.</param>
        /// <param name="hashFunc">Function to use for computing an object's hash code.</param>
        public ListComparer(
            Func<T, T, bool> equalsFunc,
            Func<T, int> hashFunc)
        {
            EqualsFunc = equalsFunc;
            HashFunc = hashFunc;
        }

        /// <summary>
        /// Returns whether the two sequences are componentwise equal.
        /// </summary>
        public bool Equals([AllowNull] List<T> x, [AllowNull] List<T> y)
        {
            return x.Count == y.Count && x.Select((o, i) => i).All(i => EqualsFunc(x[i], y[i]));
        }

        /// <summary>
        /// Returns a hash code for the given sequence.
        /// </summary>
        public int GetHashCode([DisallowNull] List<T> list)
        {
            if (!list.Any())
            {
                return typeof(T).GetHashCode();
            }

            return list.Select(x => HashFunc(x)).Aggregate((x, y) => x ^ y);
        }
    }
}
