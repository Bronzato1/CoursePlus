using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<IEnumerable<Book>> GetFeaturedBooksAsync();
        Task<IEnumerable<Book>> GetPopularBooksAsync();
        Task<Book> GetBook(int Id);
        Task<Book> AddBook(Book book);
        Task UpdateBook(Book book);
        Task DeleteBook(int id);
    }
}
