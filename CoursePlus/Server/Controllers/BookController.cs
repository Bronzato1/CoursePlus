using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursePlus.Server.Repositories;
using CoursePlus.Shared.Models;
using CoursePlus.Shared.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoursePlus.Server.Controllers
{
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("api/books")]
        public IActionResult GetBooks()
        {
            return Ok(_bookRepository.GetBooks());
        }

        [HttpGet("api/books/featured")]
        public IActionResult GetFeaturedBooks()
        {
            return Ok(_bookRepository.GetFeaturedBooks());
        }

        [HttpGet("api/books/popular")]
        public IActionResult GetPopularBooks()
        {
            return Ok(_bookRepository.GetPopularBooks());
        }

        [HttpGet("api/books/category/{id:int}")]
        public IActionResult GetBooksByCategory(int id)
        {
            return Ok(_bookRepository.GetBooksByCategory(id));
        }

        [HttpGet("api/book/{id:int}")]
        public IActionResult GetBook(int id)
        {
            return Ok(_bookRepository.GetBook(id));
        }

        [HttpPost("api/book")]
        public IActionResult CreateBook([FromBody] Book book)
        {
            if (book == null)
                return BadRequest();

            if (book.Title == string.Empty || book.Description == string.Empty)
            {
                ModelState.AddModelError("Title/Description", "The title or description shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdBook = _bookRepository.AddBook(book);

            return Created("book", createdBook);
        }

        [HttpPut("api/book")]
        public IActionResult UpdateBook([FromBody] Book book)
        {
            if (book == null)
                return BadRequest();

            if (book.Title == string.Empty || book.Description == string.Empty)
            {
                ModelState.AddModelError("Title/Description", "The title or description shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookToUpdate = _bookRepository.GetBook(book.Id);

            if (bookToUpdate == null)
                return NotFound();

            _bookRepository.UpdateBook(book);

            return NoContent(); //success
        }

        [HttpDelete("api/book/{id:int}")]
        public IActionResult DeleteBook(int id)
        {
            if (id == 0)
                return BadRequest();

            var bookToDelete = _bookRepository.GetBook(id);
            if (bookToDelete == null)
                return NotFound();

            _bookRepository.DeleteBook(id);

            return NoContent();//success
        }
    }
}
