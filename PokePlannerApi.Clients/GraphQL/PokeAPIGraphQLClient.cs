using System;
using System.Collections.Generic;
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

        public async Task<List<PokemonSpeciesInfo>> GetSpeciesInfoByPokedex(int languageId, int pokedexId, int versionGroupId)
        {
            var key = $"speciesInfoPokedex{pokedexId}VersionGroup{versionGroupId}Language{languageId}";

            var context = new Context(key);

            return await _resiliencePolicy.ExecuteAsync(async ctx =>
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                    query speciesInfoByPokedex($languageId: Int, $pokedexId: Int, $versionGroupId: Int) {
                        species_info: pokemon_v2_pokemonspecies(where: {pokemon_v2_pokemondexnumbers: {pokedex_id: {_eq: $pokedexId}}}, order_by: {id: asc}) {
                            id
                            name
                            order
                            generation_id
                            capture_rate
                            names: pokemon_v2_pokemonspeciesnames(where: {pokemon_v2_language: {id: {_eq: $languageId}}}) {
                                name
                                genus
                            }
                            pokedexes: pokemon_v2_pokemondexnumbers(where: {pokedex_id: {_eq: $pokedexId}}, order_by: {pokedex_id: asc}) {
                                pokedex_id
                                pokedex_number
                            }
                            flavor_texts: pokemon_v2_pokemonspeciesflavortexts(where: {pokemon_v2_language: {id: {_eq: $languageId}}, pokemon_v2_version: {version_group_id: {_eq: $versionGroupId}}}) {
                                flavor_text
                            }
                            varieties: pokemon_v2_pokemons(order_by: {order: asc}) {
                                id
                                name
                                is_default
                                abilities: pokemon_v2_pokemonabilities(order_by: {slot: asc}) {
                                    slot
                                    is_hidden
                                    ability: pokemon_v2_ability {
                                        id
                                        name
                                        generation_id
                                        names: pokemon_v2_abilitynames(where: {pokemon_v2_language: {id: {_eq: $languageId}}}) {
                                            name
                                        }
                                        flavor_texts: pokemon_v2_abilityflavortexts(where: {pokemon_v2_language: {id: {_eq: $languageId}}, version_group_id: {_eq: $versionGroupId}}) {
                                            flavor_text
                                        }
                                    }
                                }
                                forms: pokemon_v2_pokemonforms(where: {version_group_id: {_lte: $versionGroupId}}, order_by: {form_order: asc}) {
                                    id
                                    form_name
                                    is_mega
                                    names: pokemon_v2_pokemonformnames(where: {pokemon_v2_language: {id: {_eq: $languageId}}}) {
                                        name
                                    }
                                    types: pokemon_v2_pokemonformtypes(order_by: {slot: asc}) {
                                        type_id
                                    }
                                }
                                past_types: pokemon_v2_pokemontypepasts(order_by: {generation_id: asc, slot: asc}) {
                                    generation_id
                                    type_id
                                }
                                stats: pokemon_v2_pokemonstats(order_by: {stat_id: asc}) {
                                    stat_id
                                    base_value: base_stat
                                }
                                types: pokemon_v2_pokemontypes(order_by: {slot: asc}) {
                                    type_id
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
                        versionGroupId,
                    },
                };

                var response = await _client.SendQueryAsync<PokemonSpeciesInfoResponse>(request);
                var data = response.Data;

                return data.SpeciesInfo;
            }, context);
        }

        public async Task<List<StatInfo>> GetStatInfo(int languageId)
        {
            var key = $"statInfoLanguage{languageId}";

            var context = new Context(key);

            return await _resiliencePolicy.ExecuteAsync(async ctx =>
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                    query statInfo($languageId: Int) {
                        stat_info: pokemon_v2_stat {
                            id
                            name
                            is_battle_only
                            pokemon_v2_statnames(where: {pokemon_v2_language: {id: {_eq: $languageId}}}) {
                                name
                            }
                        }
                    }
                    ",
                    OperationName = "statInfo",
                    Variables = new
                    {
                        languageId,
                    }
                };

                var response = await _client.SendQueryAsync<StatInfoResponse>(request);
                var data = response.Data;

                return data.StatInfo;
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
                            pokemon_v2_pokemontypes_aggregate {
                                aggregate {
                                    count
                                }
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
                            pokemon_v2_pokedexversiongroups {
                                pokemon_v2_pokedex {
                                    id
                                    pokemon_v2_pokedexnames(where: {pokemon_v2_language: {id: {_eq: $languageId}}}) {
                                        name
                                    }
                                }
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
    }
}
