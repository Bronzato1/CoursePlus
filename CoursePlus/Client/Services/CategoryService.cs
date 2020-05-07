using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Category>>
                (await _httpClient.GetStreamAsync($"api/category"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Category> GetCategoryById(int categoryId)
        {
            return await JsonSerializer.DeserializeAsync<Category>
                (await _httpClient.GetStreamAsync($"api/category/{categoryId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
