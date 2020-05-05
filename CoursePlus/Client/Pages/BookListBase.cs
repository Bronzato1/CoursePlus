using CoursePlus.Client.Services;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages
{
    public class BookListBase : ComponentBase
    {
        [Inject]
        public IBookService BookService { get; set; }
        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public IEnumerable<Book> FeaturedBooks { get; set; }

        public IEnumerable<Book> PopularBooks { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        protected override async Task OnInitializedAsync()
        {
            FeaturedBooks = await BookService.GetFeaturedBooksAsync();
            PopularBooks = await BookService.GetPopularBooksAsync();
            Categories = await CategoryService.GetAllCategories();
        }
        protected async Task<IEnumerable<Book>> GetBooksForCategory(int id)
        {
            var books = await BookService.GetBooksByCategory(id);
            return books;
        }

        protected void ViewBook(Book OneBook)
        {
            NavigationManager.NavigateTo("/book-detail/" + OneBook.Id);
        }
    }
}
