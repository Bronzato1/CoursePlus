using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public class StudentService : IStudentService
    {
        private readonly HttpClient _httpClient;

        public StudentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedList<Student>> GetStudents(int pageNumber, string sortField, string sortOrder, string filterField, string filterValue)
        {
            var response = await _httpClient.GetAsync($"api/student/getstudents?pageNumber={pageNumber}&sortField={sortField}&sortOrder={sortOrder}&filterField={filterField}&filterValue={filterValue}");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<PaginatedList<Student>>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });
            return result;
        }

        public async Task<Student> GetStudent(int id)
        {
            return await JsonSerializer.DeserializeAsync<Student>(await _httpClient.GetStreamAsync($"api/student/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Student> AddStudent(Student student)
        {
            var studentJson = new StringContent(JsonSerializer.Serialize(student), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/student", studentJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Student>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdateStudent(Student student)
        {
            var studentJson = new StringContent(JsonSerializer.Serialize(student), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/student", studentJson);
        }

        public async Task DeleteStudent(int id)
        {
            await _httpClient.DeleteAsync($"api/student/{id}");
        }

        public async Task<FakeStudentModel[]> GetFakeStudents()
        {
            var response = await _httpClient.GetAsync($"api/student/getFakeStudents");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();

            var result = await JsonSerializer.DeserializeAsync<FakeStudentModel[]>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });

            return result;
        }

        public async Task<CreateFakeStudentsResult> CreateFakeStudents(List<FakeStudentModel> users)
        {
            try
            {
                var jsonList = new StringContent(JsonSerializer.Serialize(users), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/student/createFakeStudents", jsonList);
                response.EnsureSuccessStatusCode();
                var result = await JsonSerializer.DeserializeAsync<CreateFakeStudentsResult>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                Console.WriteLine("Inner Exception: {0}", ex.InnerException.Message);
                throw new ApplicationException();
            }
            
        }
    }
}
