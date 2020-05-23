using CoursePlus.Server.Data;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using CoursePlus.Shared.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
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

        public async Task<PaginatedList<Book>> GetBooks(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue)
        {
            try
            {
                int pageSize = 5;
                var bookList = _dbContext.Books
                                         .Include(x => x.Thumbnail)
                                         .Include(x => x.Category)
                                         .WhereDynamic(filterField, filterValue)
                                         .OrderByDynamic(sortField, sortOrder);

                return await PaginatedList<Book>.CreateAsync(bookList.AsNoTracking(), pageNumber ?? 1, pageSize);
            }
            catch
            {
                throw new ApplicationException();
            }
        }

        public Book GetBook(int id)
        {
            var book = _dbContext.Books
                .Where(x => x.Id == id)
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

        public async Task<bool> GenerateImagesAndThumbnailsFromUrl()
        {
            var books = _dbContext.Books.Include(x => x.Image);

            foreach(var book in books)
            {
                if (book.Image == null && !string.IsNullOrEmpty(book.ImageUrl))
                {
                    var data = CustomFunctions.ImageToByteArray(book.ImageUrl);
                    var newImage = new Image { Data = data };
                    book.Image = newImage;
                }

                if (book.Thumbnail == null && !string.IsNullOrEmpty(book.ThumbnailUrl))
                {
                    System.Drawing.Image img;
                    System.Drawing.Image thumb;

                    using (var ms = new MemoryStream(book.Image.Data))
                    {
                        img = System.Drawing.Image.FromStream(ms);
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        thumb = img.GetThumbnailImage(300, 390, () => false, IntPtr.Zero);
                        thumb.Save(ms, ImageFormat.Jpeg);
                        var newThumbnail = new Thumbnail { Data = ms.ToArray() };
                        book.Thumbnail = newThumbnail;
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
