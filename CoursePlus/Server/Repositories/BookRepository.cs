using CoursePlus.Server.Data;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public class BookRepository :IBookRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BookRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IEnumerable<Book>> GetBooks()
        {
            var books = _dbContext.Books.AsEnumerable();

            return Task.FromResult(books);
        }
    }
}
