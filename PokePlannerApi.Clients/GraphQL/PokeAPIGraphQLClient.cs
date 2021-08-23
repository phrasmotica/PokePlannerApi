using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using PokePlannerApi.Models;

namespace PokePlannerApi.Clients.GraphQL
{
    /// <summary>
    /// Class for accessing PokeAPI's GraphQL endpoint.
    /// </summary>
    public class PokeAPIGraphQLClient
    {
        private readonly GraphQLHttpClient _client;

        public PokeAPIGraphQLClient(Uri graphQlEndpoint)
        {
            _client = new GraphQLHttpClient(graphQlEndpoint, new NewtonsoftJsonSerializer());
        }

        public async Task<PokemonSpeciesInfoEntry> GetSpeciesInfo(int languageId, int generationId)
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

            var data = await GetResponse<PokemonSpeciesInfoResponse>(request);

            var versionGroupInfo = await GetVersionGroupInfo();

            foreach (var species in data.SpeciesInfo.Select(s => s.Species))
            {
                species.Validity = GetValidity(species, versionGroupInfo.VersionGroupInfo);
            }

            var id = $"speciesInfoGeneration{generationId}Language{languageId}";

            // TODO: cache this entry
            return new PokemonSpeciesInfoEntry
            {
                Id = id,
                Name = id,
                CreationTime = DateTime.UtcNow,
                GenerationId = generationId,
                LanguageId = languageId,
                Species = data.SpeciesInfo,
            };
        }

        public async Task<VersionGroupInfoEntry> GetVersionGroupInfo()
        {
            var request = new GraphQLRequest
            {
                Query = @"
                query versionGroupInfo {
                    version_group_info: pokemon_v2_versiongroup {
                        id
                        pokedexes: pokemon_v2_pokedexversiongroups {
                            pokedex_id
                        }
                    }
                }
                ",
                OperationName = "versionGroupInfo",
            };

            var data = await GetResponse<VersionGroupInfoResponse>(request);

            var id = "versionGroupInfo";

            // TODO: cache this entry
            return new VersionGroupInfoEntry
            {
                Id = id,
                Name = id,
                CreationTime = DateTime.UtcNow,
                VersionGroupInfo = data.VersionGroupInfo,
            };
        }

        private static List<int> GetValidity(SpeciesInfo species, List<VersionGroupInfo> versionGroupInfo)
        {
            if (!species.Pokedexes.Any())
            {
                return versionGroupInfo.Select(vg => vg.Id).ToList();
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
            }).Select(vg => vg.Id).ToList();
        }

        private async Task<T> GetResponse<T>(GraphQLRequest request)
        {
            var response = await _client.SendQueryAsync<T>(request);
            return response.Data;
        }
    }
}
