using CoursePlus.Client.Services;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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

        public PaginatedList<Book> FeaturedBooks { get; set; }

        public PaginatedList<Book> PopularBooks { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        protected override async Task OnInitializedAsync()
        {
            FeaturedBooks = await BookService.GetBooks(filterField: "Featured", filterValue: "true");
            PopularBooks = await BookService.GetBooks(filterField: "Popular", filterValue: "true");
            Categories = await CategoryService.GetCategories();
        }

        protected async Task<PaginatedList<Book>> GetBooksForCategory(int id)
        {
            var books = await BookService.GetBooks(filterField: "CategoryId", filterValue: id.ToString());
            return books;
        }

        protected async void PageIndexChanged(PaginatedList<Book> context, int newPageNumber, int categoryId)
        {
            if (newPageNumber < 1 || newPageNumber > context.TotalPages)
            {
                return;
            }

            var cptr = context.Items.Count;

            var data = await BookService.GetBooks(pageNumber: newPageNumber, filterField: "CategoryId", filterValue: categoryId.ToString());

            foreach (var elm in data.Items)
            {
                context.Items.Add(elm);
            }

            context.Items.RemoveRange(0, cptr);

            context.PageIndex = data.PageIndex;
            context.TotalPages = data.TotalPages;

            StateHasChanged();
        }

        protected void ViewBook(Book OneBook)
        {
            NavigationManager.NavigateTo("/book/" + OneBook.Id);
        }
    }
}
