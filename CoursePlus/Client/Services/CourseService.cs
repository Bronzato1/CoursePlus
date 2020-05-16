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
    public class CourseService : ICourseService
    {
        private readonly HttpClient _httpClient;

        public CourseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedList<Course>> GetCourses(int pageNumber = 1, IDictionary<string, string> sortOrder = null, IDictionary<string, string> filters = null)
        {
            string currentFilters = Newtonsoft.Json.JsonConvert.SerializeObject(filters);
            string currentSortOrder = Newtonsoft.Json.JsonConvert.SerializeObject(sortOrder);

            var response = await _httpClient.GetAsync($"api/course/getcourses?pageNumber={pageNumber}&sortOrder={currentSortOrder}&filters={currentFilters}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<PaginatedList<Course>>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });
            return result;
        }

        public async Task<Course> GetCourse(int id)
        {
            var data = await JsonSerializer.DeserializeAsync<Course>(await _httpClient.GetStreamAsync($"api/course/{id}"), new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });
            return data;
        }

        public async Task<Course> AddCourse(Course course)
        {
            var courseJson = new StringContent(JsonSerializer.Serialize(course), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/course", courseJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Course>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdateCourse(Course course)
        {
            var courseJson = new StringContent(JsonSerializer.Serialize(course), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/course", courseJson);
        }

        public async Task DeleteCourse(int id)
        {
            await _httpClient.DeleteAsync($"api/course/{id}");
        }
    }
}
