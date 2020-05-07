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
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedList<Book>> GetBooks(int pageNumber, string sortField, string sortOrder, string filterField, string filterValue)
        {
            var response = await _httpClient.GetAsync($"api/books/get?pageNumber={pageNumber}&sortField={sortField}&sortOrder={sortOrder}&filterField={filterField}&filterValue={filterValue}");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<PaginatedList<Book>>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });
            return result;
        }

        public async Task<Book> GetBook(int id)
        {
            return await JsonSerializer.DeserializeAsync<Book>(await _httpClient.GetStreamAsync($"api/book/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Book> AddBook(Book book)
        {
            var bookJson = new StringContent(JsonSerializer.Serialize(book), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/book", bookJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Book>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdateBook(Book book)
        {
            var bookJson = new StringContent(JsonSerializer.Serialize(book), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/book", bookJson);
        }

        public async Task DeleteBook(int id)
        {
            await _httpClient.DeleteAsync($"api/book/{id}");
        }
    }
}
