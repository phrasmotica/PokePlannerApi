using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Threading;
using System;

namespace PokeApiNet.Cache
{
    /// <remarks>
    /// Adapted from the PokeApiNet implementation: https://github.com/mtrdp642/PokeApiNet/blob/master/PokeApiNet/Cache/BaseExpirableCache.cs
    /// </remarks>
    internal abstract class BaseExpirableCache : IDisposable
    {
        private CancellationTokenSource ClearToken { get; set; } = new CancellationTokenSource();

        private void ExpireAll()
        {
            // TODO add lock?
            if (ClearToken != null && !ClearToken.IsCancellationRequested && ClearToken.Token.CanBeCanceled)
            {
                ClearToken.Cancel();
                ClearToken.Dispose();
            }

            ClearToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Clears all cache data
        /// </summary>
        public void Clear() 
        {
            ExpireAll();
        }

        /// <summary>
        /// Gets the <see cref="MemoryCacheEntryOptions"/> instance.
        /// </summary>
        /// <remarks>
        /// New options instance has to be constantly instantiated instead of shared
        /// as a consequence of <see cref="ClearToken"/> being mutable
        /// </remarks>
        protected MemoryCacheEntryOptions CacheEntryOptions => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(ClearToken.Token));
    
        /// <summary>
        /// Dispose object
        /// </summary>
        public abstract void Dispose();
    }
}
