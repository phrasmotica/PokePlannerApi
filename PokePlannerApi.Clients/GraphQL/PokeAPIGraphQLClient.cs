using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using PokePlannerApi.Models.GraphQL;
using Polly;

namespace PokePlannerApi.Clients.GraphQL
{
    /// <summary>
    /// Class for accessing PokeAPI's GraphQL endpoint.
    /// </summary>
    public class PokeAPIGraphQLClient
    {
        private readonly GraphQLHttpClient _client;
        private readonly IAsyncPolicy _resiliencePolicy;

        public PokeAPIGraphQLClient(Uri graphQlEndpoint, IAsyncPolicy resiliencePolicy)
        {
            _client = new GraphQLHttpClient(graphQlEndpoint, new NewtonsoftJsonSerializer());
            _resiliencePolicy = resiliencePolicy;
        }

        public async Task<List<PokemonSpeciesInfo>> GetSpeciesInfo(int languageId, int generationId)
        {
            var key = $"speciesInfoGeneration{generationId}Language{languageId}";

            var context = new Context(key);

            return await _resiliencePolicy.ExecuteAsync(async ctx =>
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                    query speciesInfo($languageId: Int, $generationId: Int) {
                        species_info: pokemon_v2_pokemonspeciesname(where: {pokemon_v2_language: {id: {_eq: $languageId}}, pokemon_v2_pokemonspecy: {pokemon_v2_generation: {id: {_eq: $generationId}}}}, order_by: {id: asc}) {
                            pokemon_species_id
                            name
                            species: pokemon_v2_pokemonspecy {
                                order
                                generation_id
                                pokemon_v2_pokemondexnumbers {
                                    pokedex_id
                                }
                                varieties: pokemon_v2_pokemons {
                                    is_default
                                    pokemon_v2_pokemontypes {
                                        type_id
                                    }
                                    pokemon_v2_pokemonstats {
                                        stat_id
                                        base_value: base_stat
                                    }
                                }
                            }
                        }
                    }
                    ",
                    OperationName = "speciesInfo",
                    Variables = new
                    {
                        languageId,
                        generationId,
                    },
                };

                var response = await _client.SendQueryAsync<PokemonSpeciesInfoResponse>(request);
                var data = response.Data;

                var versionGroupInfo = await GetVersionGroupInfo(languageId);

                foreach (var species in data.SpeciesInfo.Select(s => s.Species))
                {
                    species.Validity = GetValidity(species, versionGroupInfo);
                }

                return data.SpeciesInfo;
            }, context);
        }

        public async Task<List<VersionGroupInfo>> GetVersionGroupInfo(int languageId)
        {
            var key = $"versionGroupInfoLanguage{languageId}";

            var context = new Context(key);

            return await _resiliencePolicy.ExecuteAsync(async ctx =>
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                    query versionGroupInfo($languageId: Int) {
                        version_group_info: pokemon_v2_versiongroup {
                            id
                            versions: pokemon_v2_versions {
                                pokemon_v2_versionnames(where: {pokemon_v2_language: {id: {_eq: $languageId}}}) {
                                    name
                                }
                            }
                            pokedexes: pokemon_v2_pokedexversiongroups {
                                pokedex_id
                            }
                        }
                    }
                    ",
                    OperationName = "versionGroupInfo",
                    Variables = new
                    {
                        languageId,
                    }
                };

                var response = await _client.SendQueryAsync<VersionGroupInfoResponse>(request);
                var data = response.Data;

                return data.VersionGroupInfo;
            }, context);
        }

        private static List<int> GetValidity(SpeciesInfo species, List<VersionGroupInfo> versionGroupInfo)
        {
            if (!species.Pokedexes.Any())
            {
                return versionGroupInfo.Select(vg => vg.VersionGroupId).ToList();
            }

            return versionGroupInfo.Where(vg =>
            {
                if (!vg.Pokedexes.Any())
                {
                    return true;
                }

                var versionGroupPokedexes = vg.Pokedexes.Select(p => p.PokedexId);
                var speciesPokedexes = species.Pokedexes.Select(p => p.PokedexId);
                return versionGroupPokedexes.Intersect(speciesPokedexes).Any();
            }).Select(vg => vg.VersionGroupId).ToList();
        }
    }
}
