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

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Book>>(await _httpClient.GetStreamAsync($"api/books"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<Book>> GetFeaturedBooksAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Book>>(await _httpClient.GetStreamAsync($"api/books/featured"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<Book>> GetPopularBooksAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Book>>(await _httpClient.GetStreamAsync($"api/books/popular"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Book> GetBookAsync(int Id)
        {
            return await JsonSerializer.DeserializeAsync<Book>(await _httpClient.GetStreamAsync($"api/book/{Id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
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
