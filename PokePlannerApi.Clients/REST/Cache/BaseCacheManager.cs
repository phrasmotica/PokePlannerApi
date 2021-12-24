using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace PokeApiNet.Cache
{
    /// <summary>
    /// Base class for a cache manager implementation.
    /// </summary>
    /// <remarks>
    /// Main goal of the class is to cache the enumeration of types that are subclasses of <see cref="ResourceBase"/> in the assembly.
    /// 
    /// Adapted from the PokeApiNet implementation: https://github.com/mtrdp642/PokeApiNet/blob/master/PokeApiNet/Cache/BaseCacheManager.cs
    /// </remarks>
    internal abstract class BaseCacheManager : IDisposable
    {
        protected static readonly ImmutableHashSet<System.Type> ResourceTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsSubclassOf(typeof(ApiResource)) || type.IsSubclassOf(typeof(NamedApiResource)))
                .ToImmutableHashSet();

        protected static bool IsTypeSupported(System.Type type) => ResourceTypes.Contains(type);

        public abstract void Dispose();

        public abstract void ClearAll();
    }
}
