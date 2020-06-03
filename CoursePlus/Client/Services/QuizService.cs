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
    public class QuizService : IQuizService
    {
        private readonly HttpClient _httpClient;

        public QuizService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedList<QuizTopic>> GetQuizzes(int pageNumber = 1, int pageSize = 8, IDictionary<string, string> sortOrder = null, IDictionary<string, string> filters = null)
        {
            string currentFilters = Newtonsoft.Json.JsonConvert.SerializeObject(filters);
            string currentSortOrder = Newtonsoft.Json.JsonConvert.SerializeObject(sortOrder);

            var response = await _httpClient.GetAsync($"api/quiz/getquizzes?pageNumber={pageNumber}&pageSize={pageSize}&sortOrder={currentSortOrder}&filters={currentFilters}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<PaginatedList<QuizTopic>>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });
            return result;
        }

        public async Task<List<QuizTopic>> GetPopularQuizzes()
        {
            return await JsonSerializer.DeserializeAsync<List<QuizTopic>>(await _httpClient.GetStreamAsync($"api/quiz/getpopularquizzes"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<QuizTopic> GetQuiz(int id)
        {
            var data = await JsonSerializer.DeserializeAsync<QuizTopic>(await _httpClient.GetStreamAsync($"api/quiz/{id}"), new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });
            return data;
        }

        public async Task<QuizTopic> AddQuiz(QuizTopic quiz)
        {
            var quizJson = new StringContent(JsonSerializer.Serialize(quiz), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/quiz", quizJson);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<QuizTopic>(responseStream, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true,
                });
                return result;
            }

            return null;
        }

        public async Task UpdateQuiz(QuizTopic quiz)
        {
            var quizJson = new StringContent(JsonSerializer.Serialize(quiz), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/quiz", quizJson);
        }

        public async Task DeleteQuiz(int id)
        {
            await _httpClient.DeleteAsync($"api/quiz/{id}");
        }

        public async Task<int> CreateQuizzesFromJsonOfOpenQuizzDB()
        {
            var response = await _httpClient.GetAsync($"api/quiz/createQuizzesFromJsonOfOpenQuizzDB");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();

            var result = await JsonSerializer.DeserializeAsync<int>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });

            return result;
        }

    }
}
