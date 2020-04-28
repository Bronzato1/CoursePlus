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

        public IEnumerable<Book> GetBooks()
        {
            return _dbContext.Books;
        }

        public Book GetBook(int Id)
        {
            return _dbContext.Books.FirstOrDefault(x => x.Id == Id);
        }

        public Book AddBook(Book book)
        {
            var addedEntity = _dbContext.Books.Add(book);
            _dbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public Book UpdateBook(Book book)
        {
            var foundBook = _dbContext.Books.FirstOrDefault(e => e.Id == book.Id);

            if (foundBook != null)
            {
                foundBook.Title = book.Title;
                foundBook.Description = book.Description;
                foundBook.Author = book.Author;
                foundBook.CoverImage = book.CoverImage;
                foundBook.Language = book.Language;
                foundBook.PageCount = book.PageCount;
                foundBook.PublishingDate = book.PublishingDate;
                foundBook.PurchaseLink = book.PurchaseLink;
                
                _dbContext.SaveChanges();

                return foundBook;
            }

            return null;
        }

        public void DeleteBook(int id)
        {
            var foundBook = _dbContext.Books.FirstOrDefault(e => e.Id == id);
            if (foundBook == null) return;

            _dbContext.Books.Remove(foundBook);
            _dbContext.SaveChanges();
        }
    }
}
