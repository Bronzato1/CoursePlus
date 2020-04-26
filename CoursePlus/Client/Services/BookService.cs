using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            var data = await _httpClient.GetFromJsonAsync<Book[]>("api/Book");
            return data;
            //var response = await _httpClient.GetAsync("api/Book");
            //var result = JsonSerializer.Deserialize<IEnumerable<Book>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            //return result;
        }

    }
}
