using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursePlus.Shared.Models;
using CoursePlus.Client.Services;
using BlazorInputFile;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Diagnostics;
using Microsoft.AspNetCore.Components.Forms;
using Blazor.ModalDialog;

namespace CoursePlus.Client.Pages.Admin
{
    public class BookEditBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IBookService BookService { get; set; }
        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public HttpClient Client { get; set; }
        [Inject]
        public IModalDialogService ModalDialog { get; set; }

        public EditForm FormContext { get; set; }

        public Book Book { get; set; } = new Book();

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;

        public List<Category> Categories { get; set; } = new List<Category>();

        protected override void OnParametersSet()
        {
            //Console.WriteLine(Id);
            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            Categories = (await CategoryService.GetAllCategories()).ToList();

            if (Id == 0) // new book is being created
            {
                Book = new Book { PublishingDate = new DateTime(2000, 1, 1) };
            }
            else
            {
                Book = await BookService.GetBook(Id);
            }
        }

        protected async Task HandleValidSubmit()
        {
            if (Id == 0)
            {
                var addedBook = await BookService.AddBook(Book);
                if (addedBook != null)
                {
                    StatusClass = "uk-text-success";
                    Message = "New book added successfully";
                    StateHasChanged();
                    await Task.Delay(2000);
                    NavigationManager.NavigateTo("/admin/books");
                }
                else
                {
                    StatusClass = "uk-text-danger";
                    Message = "Something went wrong";
                }
            }
            else
            {
                await BookService.UpdateBook(Book);
                StatusClass = "uk-text-success";
                Message = "Book updated successfully";
                StateHasChanged();
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/admin/books");
            }
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "uk-text-warning";
            Message = "Validation errors";
        }

        protected async Task DeleteBook()
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the book ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await BookService.DeleteBook(Book.Id);
                StatusClass = "alert-danger";
                Message = "Deleted successfully";
                StateHasChanged();
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/admin/books");
            }            
        }

        protected async Task HandleSelection(IFileListEntry[] files)
        {
            var file = files.FirstOrDefault();
            if (file != null)
            {
                // Just load into .NET memory to show it can be done
                // Alternatively it could be saved to disk, or parsed in memory, or similar
                var ms = new MemoryStream();
                await file.Data.CopyToAsync(ms);

                var content = new MultipartFormDataContent { { new ByteArrayContent(ms.GetBuffer()), "\"upload\"", file.Name } };
                var result = await Client.PostAsync("api/upload", content);
                result.EnsureSuccessStatusCode();
                var uploadResult = JsonSerializer.Deserialize<UploadResult>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Book.ImageId = uploadResult.ImageId;
                Book.ThumbnailId = uploadResult.ThumbnailId;

                if (Book.Image == null) // First time image for this book
                    Book.Image = new CoursePlus.Shared.Models.Image();

                Book.Image.Data = ms.ToArray();
            }
        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/admin/books");
        }
    }
}
