using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public class FakeUserService : IFakeUserService
    {
        private readonly HttpClient _httpClient;

        public FakeUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FakeUserModel[]> GetFakeUsers()
        {
            var response = await _httpClient.GetAsync($"api/custom/getFakeUsers");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            
            var result = await JsonSerializer.DeserializeAsync<FakeUserModel[]>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });

            return result;
        }
    }
}
