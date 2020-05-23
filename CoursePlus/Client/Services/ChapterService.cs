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
    public class ChapterService : IChapterService
    {
        private readonly HttpClient _httpClient;

        public ChapterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Chapter>> GetChapters()
        {
            var response = await _httpClient.GetAsync($"api/chapter/getchapters");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<List<Chapter>>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });
            return result;
        }

        public async Task<Chapter> GetChapter(int id)
        {
            return await JsonSerializer.DeserializeAsync<Chapter>(await _httpClient.GetStreamAsync($"api/chapter/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Chapter> AddChapter(Chapter chapter)
        {
            var chapterJson = new StringContent(JsonSerializer.Serialize(chapter), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/chapter", chapterJson);

            if (response.IsSuccessStatusCode)
            {
                //return await JsonSerializer.DeserializeAsync<Chapter>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                using var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<Chapter>(responseStream, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true,
                });
                return result;
            }

            return null;
        }

        public async Task UpdateChapter(Chapter chapter)
        {
            var chapterJson = new StringContent(JsonSerializer.Serialize(chapter), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/chapter", chapterJson);
        }

        public async Task DeleteChapter(int id)
        {
            await _httpClient.DeleteAsync($"api/chapter/{id}");
        }
    }
}
