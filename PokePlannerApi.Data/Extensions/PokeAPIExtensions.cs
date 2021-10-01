using System.Collections.Generic;
using System.Linq;
using PokeApiNet;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.Extensions
{
    /// <summary>
    /// Extensions methods for other PokeAPI types.
    /// </summary>
    public static class PokeAPIExtensions
    {
        /// <summary>
        /// Returns this collection of names as a collection of localised strings.
        /// </summary>
        public static IEnumerable<LocalString> Localise(this IEnumerable<Names> names)
        {
            return names.Select(n => new LocalString { Language = n.Language.Name, Value = n.Name });
        }

        /// <summary>
        /// Returns this collection of descriptions as a collection of localised strings.
        /// </summary>
        public static IEnumerable<LocalString> Localise(this IEnumerable<Descriptions> names)
        {
            return names.Select(n => new LocalString { Language = n.Language.Name, Value = n.Description });
        }

        /// <summary>
        /// Returns this collection of genera as a collection of localised strings.
        /// </summary>
        public static IEnumerable<LocalString> Localise(this IEnumerable<Genuses> genera)
        {
            return genera.Select(n => new LocalString { Language = n.Language.Name, Value = n.Genus });
        }

        /// <summary>
        /// Returns resources for all the versions spanned by this list of encounters.
        /// </summary>
        public static IEnumerable<NamedApiResource<Version>> GetDistinctVersions(this IEnumerable<LocationAreaEncounter> encounters)
        {
            return encounters.SelectMany(e => e.VersionDetails)
                             .GroupBy(ved => ved.Version)
                             .Select(g => g.First())
                             .Select(ved => ved.Version);
        }
    }
}
