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

        public async Task<List<EncountersInfo>> GetLocationAreaEncountersByVersion(int languageId, int locationAreaId, int versionId)
        {
            var key = $"encounters{locationAreaId}Version{versionId}Language{languageId}";

            var context = new Context(key);

            return await _resiliencePolicy.ExecuteAsync(async ctx =>
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                    query locationAreaEncountersByVersion($locationAreaId: Int, $versionId: Int, $languageId: Int) {
                        encounters_info: pokemon_v2_encounter(
                            where: {
                                location_area_id: { _eq: $locationAreaId }
                                _and: { version_id: { _eq: $versionId } }
                            }
                        ) {
                            location_area_id
                            pokemon_id
                            min_level
                            max_level
                            version_id
                            conditions: pokemon_v2_encounterconditionvaluemaps {
                                pokemon_v2_encounterconditionvalue {
                                    id
                                    name
                                }
                            }
                            encounter_slot: pokemon_v2_encounterslot {
                                method: pokemon_v2_encountermethod {
                                    id
                                    name
                                    names: pokemon_v2_encountermethodnames(
                                        where: { language_id: { _eq: $languageId } }
                                    ) {
                                        name
                                    }
                                }
                                rarity
                                slot
                            }
                        }
                    }
                    ",
                    OperationName = "locationAreaEncountersByVersion",
                    Variables = new
                    {
                        languageId,
                        locationAreaId,
                        versionId
                    }
                };

                var response = await _client.SendQueryAsync<LocationAreaEncountersInfoResponse>(request);
                var data = response.Data;

                return data.EncountersInfo;
            }, context);
        }

        public async Task<List<LocationInfo>> GetLocationsByRegion(int languageId, int regionId)
        {
            var key = $"locationsRegion{regionId}Language{languageId}";

            var context = new Context(key);

            return await _resiliencePolicy.ExecuteAsync(async ctx =>
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                    query locationsByRegion($regionId: Int, $languageId: Int) {
                        location_info: pokemon_v2_location(
                            where: { region_id: { _eq: $regionId } }
                        ) {
                            id
                            name
                            names: pokemon_v2_locationnames(
                                where: { language_id: { _eq: $languageId } }
                            ) {
                                name
                            }
                            location_areas: pokemon_v2_locationareas {
                                id
                                name
                                names: pokemon_v2_locationareanames(
                                    where: { language_id: { _eq: $languageId } }
                                ) {
                                    name
                                }
                            }
                        }
                    }
                    ",
                    OperationName = "locationsByRegion",
                    Variables = new
                    {
                        languageId,
                        regionId
                    }
                };

                var response = await _client.SendQueryAsync<LocationInfoResponse>(request);
                var data = response.Data;

                return data.LocationInfo;
            }, context);
        }

        public async Task<List<PokemonMoveInfo>> GetPokemonMovesInfoByVersionGroup(int languageId, int pokemonId, int versionGroupId)
        {
            var key = $"movesInfoPokemon{pokemonId}VersionGroup{versionGroupId}Language{languageId}";

            var context = new Context(key);

            return await _resiliencePolicy.ExecuteAsync(async ctx =>
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                    query pokemonMovesByVersionGroup($languageId: Int, $pokemonId: Int, $versionGroupId: Int) {
                        moves_info: pokemon_v2_pokemonmove(where: {pokemon_id: {_eq: $pokemonId}, version_group_id: {_eq: $versionGroupId}}, order_by: {id: asc}) {
                            id
                            level
                            learn_method: pokemon_v2_movelearnmethod {
                                name
                                names: pokemon_v2_movelearnmethodnames(where: {language_id: {_eq: $languageId}}, order_by: {id: asc}) {
                                    name
                                }
                            }
                            move: pokemon_v2_move {
                                id
                                name
                                power
                                accuracy
                                pp
                                priority
                                damage_class: pokemon_v2_movedamageclass {
                                    id
                                    name
                                }
                                flavor_texts: pokemon_v2_moveflavortexts(where: {pokemon_v2_language: {id: {_eq: $languageId}}, version_group_id: {_eq: $versionGroupId}}) {
                                    flavor_text
                                }
                                meta: pokemon_v2_movemeta {
                                    category: pokemon_v2_movemetacategory {
                                        id
                                        name
                                    }
                                }
                                names: pokemon_v2_movenames(where: {language_id: {_eq: $languageId}}, order_by: {id: asc}) {
                                    name
                                }
                                target: pokemon_v2_movetarget {
                                    id
                                    name
                                }
                                type: pokemon_v2_type {
                                    id
                                }
                            }
                        }
                    }
                    ",
                    OperationName = "pokemonMovesByVersionGroup",
                    Variables = new
                    {
                        languageId,
                        pokemonId,
                        versionGroupId
                    }
                };

                var response = await _client.SendQueryAsync<PokemonMovesInfoResponse>(request);
                var data = response.Data;

                return data.MovesInfo;
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
                            regions: pokemon_v2_versiongroupregions {
                                pokemon_v2_region {
                                    id
                                    name
                                    names: pokemon_v2_regionnames(where: {pokemon_v2_language: {id: {_eq: $languageId}}}) {
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
