using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public interface IBookRepository
    {
        public IEnumerable<Book> GetBooks();
        public IEnumerable<Book> GetFeaturedBooks();
        public IEnumerable<Book> GetPopularBooks();
        public IEnumerable<Book> GetBooksByCategory(int id);
        public Book GetBook(int id);
        public Book AddBook(Book book);
        public Book UpdateBook(Book book);
        public void DeleteBook(int id);
    }
}
