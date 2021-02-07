using System.Linq;
using PokeApiNet;

namespace PokePlannerWeb.Data.Extensions
{
    /// <summary>
    /// Extension methods for the PokeAPI Pokemon type.
    /// </summary>
    public static class PokemonExtensions
    {
        /// <summary>
        /// Returns this Pokemon's base stats in the version group with the given ID.
        /// </summary>
        public static int[] GetBaseStats(this Pokemon pokemon, int versionGroupId)
        {
            // FUTURE: anticipating a generation-based base stats changelog

            return pokemon.Stats.Select(bs => bs.BaseStat).ToArray();
        }
    }
}
