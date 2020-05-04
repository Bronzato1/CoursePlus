using CoursePlus.Client.Services;
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

        public IEnumerable<CoursePlus.Shared.Models.Book> AllBooks { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AllBooks = await BookService.GetAllBooksAsync();
        }

    }
}
