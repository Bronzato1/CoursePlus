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
    public class InstructorService : IInstructorService
    {
        private readonly HttpClient _httpClient;

        public InstructorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedList<Instructor>> GetInstructors(int pageNumber, string sortField, string sortOrder, string filterField, string filterValue)
        {
            var response = await _httpClient.GetAsync($"api/instructor/getinstructors?pageNumber={pageNumber}&sortField={sortField}&sortOrder={sortOrder}&filterField={filterField}&filterValue={filterValue}");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<PaginatedList<Instructor>>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });
            return result;
        }

        public async Task<List<Instructor>> GetAllInstructors()
        {
            var response = await _httpClient.GetAsync($"api/instructor/getallinstructors");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<List<Instructor>>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });
            return result;
        }

        public async Task<Instructor> GetInstructor(int id)
        {
            return await JsonSerializer.DeserializeAsync<Instructor>(await _httpClient.GetStreamAsync($"api/instructor/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Instructor> AddInstructor(Instructor instructor)
        {
            var instructorJson = new StringContent(JsonSerializer.Serialize(instructor), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/instructor", instructorJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Instructor>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdateInstructor(Instructor instructor)
        {
            var instructorJson = new StringContent(JsonSerializer.Serialize(instructor), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/instructor", instructorJson);
        }

        public async Task DeleteInstructor(int id)
        {
            await _httpClient.DeleteAsync($"api/instructor/{id}");
        }
    }
}
