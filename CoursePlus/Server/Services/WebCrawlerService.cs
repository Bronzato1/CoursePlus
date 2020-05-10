using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoursePlus.Server.Services
{
    public class WebCrawlerService : IWebCrawlerService
    {
        private readonly HttpClient _httpClient;

        public WebCrawlerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FakeUserModel[]> GetFakeUsers()
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "get_users?av=f&sp=0&ps=10");

                //rm.SetBrowserRequestMode(BrowserRequestMode.NoCors);
                //rm.SetBrowserRequestCache(BrowserRequestCache.NoCache);

                var response = await _httpClient.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();

                var parsedUsers = JsonSerializer.Deserialize<FakeUserModel[]>(result.ToString(), new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true,
                });

                var index = 0;

                foreach(var elm in parsedUsers)
                {
                    elm.Index = index;
                    index++;
                }

                return parsedUsers;
            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }
        }
    }
}
