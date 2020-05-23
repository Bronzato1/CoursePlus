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
    public class ProfileService : IProfileService
    {
        private readonly HttpClient _httpClient;

        public ProfileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedList<Profile>> GetProfiles(int pageNumber, string sortField, string sortOrder, string filterField, string filterValue)
        {
            var response = await _httpClient.GetAsync($"api/profile/getprofiles?pageNumber={pageNumber}&sortField={sortField}&sortOrder={sortOrder}&filterField={filterField}&filterValue={filterValue}");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<PaginatedList<Profile>>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });
            return result;
        }

        public async Task<List<Profile>> GetAllProfiles()
        {
            var response = await _httpClient.GetAsync($"api/profile/getallprofiles");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<List<Profile>>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });
            return result;
        }


        public async Task<Profile> GetProfile(int id)
        {
            return await JsonSerializer.DeserializeAsync<Profile>(await _httpClient.GetStreamAsync($"api/profile/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Profile> GetProfileByUserId(string userId)
        {
            var responseStream = await _httpClient.GetStreamAsync($"api/profile/getProfileByUserId?userId={userId}");

            if (responseStream.Length > 0)
            {
                return await JsonSerializer.DeserializeAsync<Profile>(responseStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            return null;   
        }

        public async Task<Profile> AddProfile(Profile profile)
        {
            var profileJson = new StringContent(JsonSerializer.Serialize(profile), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/profile", profileJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Profile>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdateProfile(Profile profile)
        {
            var profileJson = new StringContent(JsonSerializer.Serialize(profile), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/profile", profileJson);
        }

        public async Task DeleteProfile(int id)
        {
            await _httpClient.DeleteAsync($"api/profile/{id}");
        }

        public async Task<FakeProfileModel[]> GetFakeProfiles()
        {
            var response = await _httpClient.GetAsync($"api/profile/getFakeProfiles");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();

            var result = await JsonSerializer.DeserializeAsync<FakeProfileModel[]>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });

            return result;
        }

        public async Task<CreateFakeProfilesResult> CreateFakeProfiles(List<FakeProfileModel> users)
        {
            try
            {
                var jsonList = new StringContent(JsonSerializer.Serialize(users), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/profile/createFakeProfiles", jsonList);
                response.EnsureSuccessStatusCode();
                var result = await JsonSerializer.DeserializeAsync<CreateFakeProfilesResult>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
