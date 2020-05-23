using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public interface IBookRepository
    {
        Task<PaginatedList<Book>> GetBooks(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue);

        Book GetBook(int id);

        Book AddBook(Book book);

        Book UpdateBook(Book book);

        void DeleteBook(int id);

        Task<bool> GenerateImagesAndThumbnailsFromUrl();
    }
}
