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

        public async Task<List<GenerationInfo>> GetGenerationInfo(int languageId)
        {
            var key = $"generationInfoLanguage{languageId}";

            var context = new Context(key);

            return await _resiliencePolicy.ExecuteAsync(async ctx =>
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                    query generationInfo($languageId: Int) {
                        generation_info: pokemon_v2_generation {
                            id
                            name
                            pokemon_v2_generationnames(where: {language_id: {_eq: $languageId}}) {
                                name
                            }
                        }
                    }
                    ",
                    OperationName = "generationInfo",
                    Variables = new
                    {
                        languageId,
                    }
                };

                var response = await _client.SendQueryAsync<GenerationInfoResponse>(request);
                var data = response.Data;

                return data.GenerationInfo;
            }, context);
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
                        species_info: pokemon_v2_pokemonspecies(where: {pokemon_v2_generation: {id: {_eq: $generationId}}}, order_by: {id: asc}) {
                            id
                            name
                            order
                            generation_id
                            names: pokemon_v2_pokemonspeciesnames(where: {pokemon_v2_language: {id: {_eq: $languageId}}}) {
                                name
                            }
                            pokedexes: pokemon_v2_pokemondexnumbers {
                                pokedex_id
                                pokedex_number
                            }
                            varieties: pokemon_v2_pokemons {
                                is_default
                                types: pokemon_v2_pokemontypes {
                                    type_id
                                }
                                stats: pokemon_v2_pokemonstats {
                                    stat_id
                                    base_value: base_stat
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

                foreach (var species in data.SpeciesInfo)
                {
                    species.Validity = GetValidity(species, versionGroupInfo);
                }

                return data.SpeciesInfo;
            }, context);
        }

        public async Task<List<PokemonSpeciesInfo>> GetSpeciesInfoByPokedex(int languageId, int pokedexId)
        {
            var key = $"speciesInfoPokedex{pokedexId}Language{languageId}";

            var context = new Context(key);

            return await _resiliencePolicy.ExecuteAsync(async ctx =>
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                    query speciesInfoByPokedex($languageId: Int, $pokedexId: Int) {
                        species_info: pokemon_v2_pokemonspecies(where: {pokemon_v2_pokemondexnumbers: {pokedex_id: {_eq: $pokedexId}}}, order_by: {id: asc}) {
                            id
                            name
                            order
                            generation_id
                            names: pokemon_v2_pokemonspeciesnames(where: {pokemon_v2_language: {id: {_eq: $languageId}}}) {
                                name
                            }
                            pokedexes: pokemon_v2_pokemondexnumbers {
                                pokedex_id
                                pokedex_number
                            }
                            varieties: pokemon_v2_pokemons {
                                is_default
                                types: pokemon_v2_pokemontypes {
                                    type_id
                                }
                                stats: pokemon_v2_pokemonstats {
                                    stat_id
                                    base_value: base_stat
                                }
                            }
                        }
                    }
                    ",
                    OperationName = "speciesInfoByPokedex",
                    Variables = new
                    {
                        languageId,
                        pokedexId,
                    },
                };

                var response = await _client.SendQueryAsync<PokemonSpeciesInfoResponse>(request);
                var data = response.Data;

                var versionGroupInfo = await GetVersionGroupInfo(languageId);

                foreach (var species in data.SpeciesInfo)
                {
                    species.Validity = new List<int>();
                }

                return data.SpeciesInfo;
            }, context);
        }

        public async Task<List<TypeInfo>> GetTypeInfo(int languageId)
        {
            var key = $"typeInfoLanguage{languageId}";

            var context = new Context(key);

            return await _resiliencePolicy.ExecuteAsync(async ctx =>
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                    query typeInfo($languageId: Int) {
                        type_info: pokemon_v2_type {
                            id
                            name
                            generation_id
                            pokemon_v2_typenames(where: {language_id: {_eq: $languageId}}) {
                                name
                            }
                        }
                    }
                    ",
                    OperationName = "typeInfo",
                    Variables = new
                    {
                        languageId,
                    }
                };

                var response = await _client.SendQueryAsync<TypeInfoResponse>(request);
                var data = response.Data;

                return data.TypeInfo;
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
                            generation_id
                            versions: pokemon_v2_versions {
                                id
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

        private static List<int> GetValidity(PokemonSpeciesInfo species, List<VersionGroupInfo> versionGroupInfo)
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
