using Blazor.ModalDialog;
using CoursePlus.Client.Services;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages.Admin
{
    public class BookListBase : ComponentBase
    {
        [Inject]
        public IBookService BookService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IModalDialogService ModalDialog { get; set; }

        public IEnumerable<CoursePlus.Shared.Models.Book> SomeBooks { get; set; }

        public EnumFilters CurrentFilter { get; set; } = EnumFilters.All;

        public enum EnumFilters
        {
            All,
            Featured,
            Popular
        }

        protected override async Task OnInitializedAsync()
        {
            await RefreshListAsync();
        }

        protected async Task GetAllBooksAsync()
        {
            CurrentFilter = EnumFilters.All;
            await RefreshListAsync();
        }

        protected async Task GetFeaturedBooksAsync()
        {
            CurrentFilter = EnumFilters.Featured;
            await RefreshListAsync();
        }

        protected async Task GetPopularBooksAsync()
        {
            CurrentFilter = EnumFilters.Popular;
            await RefreshListAsync();
        }

        protected void EditBook(Book book)
        {
            NavigationManager.NavigateTo("/admin/book/" + book.Id);
        }

        protected void AddBook()
        {
            NavigationManager.NavigateTo("/admin/book/0");
        }

        public async Task DeleteBook(Book book)
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the book ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await BookService.DeleteBook(book.Id);
                SomeBooks = await BookService.GetAllBooksAsync();
            }
        }

        public async Task RefreshListAsync()
        {
            switch (CurrentFilter)
            {
                case EnumFilters.All:
                    SomeBooks = await BookService.GetAllBooksAsync();
                    break;
                case EnumFilters.Featured:
                    SomeBooks = await BookService.GetFeaturedBooksAsync();
                    break;
                case EnumFilters.Popular:
                    SomeBooks = await BookService.GetPopularBooksAsync();
                    break;
            }
        }
    }
}
