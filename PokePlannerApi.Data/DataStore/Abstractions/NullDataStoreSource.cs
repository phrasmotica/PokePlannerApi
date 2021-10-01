using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Abstractions
{
    /// <summary>
    /// Data store source that does nothing.
    /// </summary>
    public class NullDataStoreSource<TEntry> : IDataStoreSource<TEntry> where TEntry : EntryBase
    {
        /// <inheritdoc />
        public Task<IEnumerable<TEntry>> GetAll()
        {
            return Task.FromResult(Enumerable.Empty<TEntry>());
        }

        /// <inheritdoc />
        public Task<TEntry> GetOne(Expression<Func<TEntry, bool>> predicate)
        {
            return Task.FromResult<TEntry>(default);
        }

        /// <inheritdoc />
        public Task<TEntry> Create(TEntry entry)
        {
            return Task.FromResult<TEntry>(entry);
        }

        /// <inheritdoc />
        public Task DeleteOne(Expression<Func<TEntry, bool>> predicate)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<(bool, TEntry)> HasOne(Expression<Func<TEntry, bool>> predicate)
        {
            return Task.FromResult((false, (TEntry) default));
        }
    }
}
