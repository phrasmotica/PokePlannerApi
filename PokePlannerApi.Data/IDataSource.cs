using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data
{
    /// <summary>
    /// Interface for data sources that store transformed representations of PokeAPI resources.
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Creates the given entry of the given type and returns it.
        /// </summary>
        Task<TEntry> Create<TEntry>(TEntry entry) where TEntry : EntryBase;

        /// <summary>
        /// Returns all entries of the given type.
        /// </summary>
        Task<IEnumerable<TEntry>> GetAll<TEntry>() where TEntry : EntryBase;

        /// <summary>
        /// Returns the first entry of the given type that matches the given predicate.
        /// </summary>
        Task<TEntry> Get<TEntry>(Expression<Func<TEntry, bool>> predicate) where TEntry : EntryBase;

        /// <summary>
        /// Replaces the entry with the key of the given entry, with the given entry.
        /// </summary>
        Task Update<TEntry>(int key, TEntry entry) where TEntry : EntryBase;

        /// <summary>
        /// Deletes the first entry of the given type that matches the given predicate.
        /// </summary>
        Task Delete<TEntry>(Expression<Func<TEntry, bool>> predicate) where TEntry : EntryBase;
    }
}
