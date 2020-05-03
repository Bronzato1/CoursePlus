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
    public class BookOverviewBase : ComponentBase
    {
        [Inject]
        public IBookService BookService { get; set; }
        [Inject]
        public NavigationManager MyNavigationManager { get; set; }
        [Inject]
        public IModalDialogService ModalDialog { get; set; }

        public IEnumerable<CoursePlus.Shared.Models.Book> AllBooks { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AllBooks = await BookService.GetBooks();
        }

        protected void EditBook(Book book)
        {
            MyNavigationManager.NavigateTo("/admin/book-edit/" + book.Id);
        }

        protected void AddnewBook()
        {
            MyNavigationManager.NavigateTo("/admin/book-edit/0");
        }

        public async Task ConfirmDelete(Book book)
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the book ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await BookService.DeleteBook(book.Id);
                AllBooks = await BookService.GetBooks();
            }
        }

        public async Task RefreshList()
        {
            AllBooks = await BookService.GetBooks();
        }
    }
}
