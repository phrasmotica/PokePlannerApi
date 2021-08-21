using System;
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

            // TODO: include species validity in info entry

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

        private async Task<T> GetResponse<T>(GraphQLRequest request)
        {
            var response = await _client.SendQueryAsync<T>(request);
            return response.Data;
        }
    }
}
