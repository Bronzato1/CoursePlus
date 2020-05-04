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
        public NavigationManager NavigationManager { get; set; }

        public IEnumerable<CoursePlus.Shared.Models.Book> FeaturedBooks { get; set; }

        public IEnumerable<CoursePlus.Shared.Models.Book> PopularBooks { get; set; }

        protected override async Task OnInitializedAsync()
        {
            FeaturedBooks = await BookService.GetFeaturedBooksAsync();
            PopularBooks = await BookService.GetPopularBooksAsync();
        }

        protected void ViewBook(Book OneBook)
        {
            NavigationManager.NavigateTo("/book-detail/" + OneBook.Id);
        }
    }
}
