using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public interface IBookService
    {
        Task<PaginatedList<Book>> GetBooks(int pageNumber = 1, string sortField = "", string sortOrder = "", string filterField = "", string filterValue = "");

        Task<Book> GetBook(int id);

        Task<Book> AddBook(Book book);

        Task UpdateBook(Book book);

        Task DeleteBook(int id);
    }
}
