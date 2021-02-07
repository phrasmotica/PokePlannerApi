using System.Collections.Generic;
using PokeApiNet;

namespace PokePlannerWeb.Tests
{
    /// <summary>
    /// Helper functions for generating PokeAPI objects for tests.
    /// </summary>
    public class PokeApiHelpers
    {
        /// <summary>
        /// Returns a navigation property for a named API resource.
        /// </summary>
        public static NamedApiResource<T> NamedResourceNavigation<T>(string name = "name", string url = "url")
            where T : NamedApiResource
        {
            return new NamedApiResource<T>
            {
                Name = name,
                Url = url
            };
        }

        /// <summary>
        /// Returns a list of navigation properties for a named API resource.
        /// </summary>
        public static IEnumerable<NamedApiResource<T>> NamedResourceNavigations<T>(int count)
            where T : NamedApiResource
        {
            for (int i = 0; i < count; i++)
            {
                yield return NamedResourceNavigation<T>($"name{i + 1}", $"url{i + 1}");
            }
        }

        /// <summary>
        /// Returns a list of encounter condition value navigation properties.
        /// </summary>
        public static IEnumerable<NamedApiResource<EncounterConditionValue>> ConditionValues(int count)
        {
            return NamedResourceNavigations<EncounterConditionValue>(count);
        }

        /// <summary>
        /// Returns an encounter for a single method and condition value set.
        /// </summary>
        public static Encounter Encounter(
            int chance = 1,
            int minLevel = 1,
            int maxLevel = 6,
            List<NamedApiResource<EncounterConditionValue>> conditionValues = null,
            NamedApiResource<EncounterMethod> method = null)
        {
            return new Encounter
            {
                Chance = chance,
                MinLevel = minLevel,
                MaxLevel = maxLevel,
                ConditionValues = conditionValues ?? new List<NamedApiResource<EncounterConditionValue>>(),
                Method = method ?? NamedResourceNavigation<EncounterMethod>()
            };
        }

        /// <summary>
        /// Returns a list of encounters for a single method and condition value set.
        /// </summary>
        public static IEnumerable<Encounter> Encounters(
            int count,
            List<NamedApiResource<EncounterConditionValue>> conditionValues = null,
            NamedApiResource<EncounterMethod> method = null)
        {
            for (int i = 0; i < count; i++)
            {
                yield return Encounter(i + 1, i + 1, i + 6, conditionValues, method);
            }
        }
    }
}
