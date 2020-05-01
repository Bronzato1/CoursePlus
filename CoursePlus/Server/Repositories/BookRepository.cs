using CoursePlus.Server.Data;
using CoursePlus.Shared.Models;
using Microsoft.EntityFrameworkCore;
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
            return _dbContext.Books
                .Include(x => x.Thumbnail)
                .Include(x => x.Category);
        }

        public Book GetBook(int Id)
        {
            var book = _dbContext.Books
                .Where(x => x.Id == Id)
                .Include(x => x.Image)
                .Include(x => x.Category)
                .FirstOrDefault();
            return book;
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
                foundBook.ImageId = book.ImageId;
                foundBook.ThumbnailId = book.ThumbnailId;
                foundBook.Language = book.Language;
                foundBook.PageCount = book.PageCount;
                foundBook.PublishingDate = book.PublishingDate;
                foundBook.PurchaseLink = book.PurchaseLink;
                foundBook.CategoryId = book.CategoryId;
                foundBook.Featured = book.Featured;
                foundBook.Popular = book.Popular;
                
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
