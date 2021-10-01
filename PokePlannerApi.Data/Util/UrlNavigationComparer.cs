using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using PokeApiNet;

namespace PokePlannerApi.Data.Util
{
    /// <summary>
    /// Class for comparing navigation objects to API resources.
    /// </summary>
    public class NamedApiResourceComparer<T> : IEqualityComparer<NamedApiResource<T>>
        where T : NamedApiResource
    {
        /// <summary>
        /// Returns whether the two navigation objects are equal.
        /// </summary>
        public bool Equals([AllowNull] NamedApiResource<T> x, [AllowNull] NamedApiResource<T> y)
        {
            // GetHashCode(x) and GetHashCode(y) must be equal for this method to execute
            return string.Equals(x.Url, y.Url, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a hash code for the navigation object.
        /// </summary>
        public int GetHashCode([DisallowNull] NamedApiResource<T> obj)
        {
            return obj.Url.GetHashCode();
        }
    }
}
