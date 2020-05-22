using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly HttpClient _httpClient;

        public PlaylistService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedList<Playlist>> GetPlaylists(int pageNumber = 1, IDictionary<string, string> sortOrder = null, IDictionary<string, string> filters = null)
        {
            string currentFilters = Newtonsoft.Json.JsonConvert.SerializeObject(filters);
            string currentSortOrder = Newtonsoft.Json.JsonConvert.SerializeObject(sortOrder);

            var response = await _httpClient.GetAsync($"api/playlist/getplaylists?pageNumber={pageNumber}&sortOrder={currentSortOrder}&filters={currentFilters}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<PaginatedList<Playlist>>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });
            return result;
        }

        public async Task<Playlist> GetPlaylist(int id)
        {
            var data = await JsonSerializer.DeserializeAsync<Playlist>(await _httpClient.GetStreamAsync($"api/playlist/{id}"), new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });
            return data;
        }

        public async Task<Playlist> AddPlaylist(Playlist playlist)
        {
            var playlistJson = new StringContent(JsonSerializer.Serialize(playlist), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/playlist", playlistJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Playlist>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdatePlaylist(Playlist playlist)
        {
            var playlistJson = new StringContent(JsonSerializer.Serialize(playlist), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/playlist", playlistJson);
        }

        public async Task DeletePlaylist(int id)
        {
            await _httpClient.DeleteAsync($"api/playlist/{id}");
        }
    }
}
