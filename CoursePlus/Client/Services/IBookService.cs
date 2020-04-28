using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetBooks();
        Task<Book> GetBook(int Id);
        Task<Book> AddBook(Book book);
        Task UpdateBook(Book book);
        Task DeleteBook(int id);
    }
}
