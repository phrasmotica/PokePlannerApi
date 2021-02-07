using System.Collections.Generic;
using System.Linq;
using PokeApiNet;
using PokePlannerWeb.Data.DataStore;

namespace PokePlannerWeb.Data.Extensions
{
    /// <summary>
    /// Extensions methods for other PokeAPI types.
    /// </summary>
    public static class PokeAPIExtensions
    {
        /// <summary>
        /// Returns whether this evolution chain has only one stage.
        /// </summary>
        public static bool IsEmpty(this EvolutionChain evolutionChain)
        {
            return !evolutionChain.Chain.EvolvesTo.Any();
        }

        /// <summary>
        /// Returns a minimal copy of this API resource.
        /// </summary>
        public static T Minimise<T>(this T resource) where T : ResourceBase, new()
        {
            return new T
            {
                Id = resource.Id
            };
        }

        /// <summary>
        /// Returns a minimal copy of this named API resource.
        /// </summary>
        public static T MinimiseNamed<T>(this T resource) where T : NamedApiResource, new()
        {
            return new T
            {
                Id = resource.Id,
                Name = resource.Name
            };
        }

        /// <summary>
        /// Returns the name from the given list of names in the given locale.
        /// </summary>
        public static string GetName(this List<Names> names, string locale = "en")
        {
            return names?.FirstOrDefault(n => n.Language.Name == locale)?.Name;
        }

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
