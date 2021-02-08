using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Abstractions
{
    /// <summary>
    /// Data store source for Cosmos DB.
    /// </summary>
    public class NullDataStoreSource<TEntry> : IDataStoreSource<TEntry> where TEntry : EntryBase
    {
        /// <summary>
        /// Returns all entries.
        /// </summary>
        public Task<IEnumerable<TEntry>> GetAll()
        {
            return Task.FromResult(Enumerable.Empty<TEntry>());
        }

        /// <summary>
        /// Returns the first entry that matches the given predicate.
        /// </summary>
        public Task<TEntry> GetOne(Expression<Func<TEntry, bool>> predicate)
        {
            return Task.FromResult<TEntry>(default);
        }

        /// <summary>
        /// Creates the given entry and returns it.
        /// </summary>
        public Task<TEntry> Create(TEntry entry)
        {
            return Task.FromResult<TEntry>(entry);
        }

        /// <summary>
        /// Deletes the first entry that matches the given predicate.
        /// </summary>
        public Task DeleteOne(Expression<Func<TEntry, bool>> predicate)
        {
            return Task.CompletedTask;
        }
    }
}
