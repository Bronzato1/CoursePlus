using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public class EpisodeService : IEpisodeService
    {
        private readonly HttpClient _httpClient;

        public EpisodeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Episode>> GetEpisodes()
        {
            var response = await _httpClient.GetAsync($"api/episode/getepisodes");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<List<Episode>>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });
            return result;
        }

        public async Task<Episode> GetEpisode(int id)
        {
            return await JsonSerializer.DeserializeAsync<Episode>(await _httpClient.GetStreamAsync($"api/episode/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Episode> AddEpisode(Episode episode)
        {
            var episodeJson = new StringContent(JsonSerializer.Serialize(episode), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/episode", episodeJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Episode>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdateEpisode(Episode episode)
        {
            var episodeJson = new StringContent(JsonSerializer.Serialize(episode), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/episode", episodeJson);
        }

        public async Task DeleteEpisode(int id)
        {
            await _httpClient.DeleteAsync($"api/episode/{id}");
        }
    }
}
