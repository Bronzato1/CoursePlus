﻿using Blazor.ModalDialog;
using CoursePlus.Client.Services;
using CoursePlus.Shared.Infrastructure;
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
        [Inject] public IBookService BookService { get; set; }
        [Inject] public ICategoryService CategoryService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IModalDialogService ModalDialog { get; set; }

        public PaginatedList<Book> PaginatedList = new PaginatedList<Book>();
        public IEnumerable<Book> SomeBooks { get { return PaginatedList.Items; } }
        public IEnumerable<Category> SomeCategories { get; set; }

        int currentPageNumber = 1;
        string currentSortField = "Title";
        string currentSortOrder = "Asc";
        string currentFilterField = string.Empty;
        string currentFilterValue = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            SomeCategories = await CategoryService.GetCategories();
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
        public string SortIndicator(string sortField)
        {
            if (sortField.Equals(currentSortField))
            {
                return currentSortOrder.Equals("Asc") ? "icon-material-outline-arrow-drop-down" : "icon-material-outline-arrow-drop-up";
            }
            return string.Empty;
        }
        public string FilterIndicator(string filterField, string filterValue)
        {
            if (filterField.Equals(currentFilterField) && filterValue.Equals(currentFilterValue))
            {
                return "uk-active";
            }
            return string.Empty;
        }

        public async void PageIndexChanged(int newPageNumber)
        {
            if (newPageNumber < 1 || newPageNumber > PaginatedList.TotalPages)
            {
                return;
            }

            currentPageNumber = newPageNumber;
            await RefreshListAsync();
            StateHasChanged();
        }
        public async Task DeleteBook(Book book)
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the book ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await BookService.DeleteBook(book.Id);
                await RefreshListAsync();
            }
        }
        public async Task RefreshListAsync()
        {
            PaginatedList = await BookService.GetBooks(currentPageNumber, currentSortField, currentSortOrder, currentFilterField, currentFilterValue);
        }
        public async Task Sort(string sortField)
        {
            if (sortField.Equals(currentSortField))
            {
                currentSortOrder = currentSortOrder.Equals("Asc") ? "Desc" : "Asc";
            }
            else
            {
                currentSortField = sortField;
                currentSortOrder = "Asc";
            }
            await RefreshListAsync();
        }
        public async Task Filter(string field, string value)
        {
            currentPageNumber = 1;
            currentFilterField = field;
            currentFilterValue = value;

            await RefreshListAsync();
        }
    }
}
