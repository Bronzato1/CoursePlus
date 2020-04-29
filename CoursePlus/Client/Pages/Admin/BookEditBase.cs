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

        public Book Book { get; set; } = new Book();

        //needed to bind to select to value
        protected string CategoryId = string.Empty;

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        public List<Category> Categories { get; set; } = new List<Category>();

        protected override void OnParametersSet()
        {
            //Console.WriteLine(Id);
            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            Saved = false;

            Categories = (await CategoryService.GetAllCategories()).ToList();

            if (Id == 0) // new book is being created
            {
                // add some defaults
                Book = new Book { Language = EnumLanguages.English, PublishingDate = new DateTime(2000, 1, 1) };
            }
            else
            {
                Book = await BookService.GetBook(Id);
            }

            CategoryId = Book.CategoryId.ToString();

        }

        protected async Task HandleValidSubmit()
        {
            Book.CategoryId = int.Parse(CategoryId);

            if (Id == 0)
            {
                var addedBook = await BookService.AddBook(Book);
                if (addedBook != null)
                {
                    StatusClass = "alert-success";
                    Message = "New book added successfully.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong adding the new book. Please try again.";
                    Saved = false;
                }
            }
            else
            {
                await BookService.UpdateBook(Book);
                StatusClass = "alert-success";
                Message = "Book updated successfully.";
                Saved = true;
            }
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are some validation errors. Please try again.";
        }

        protected async Task DeleteBook()
        {
            await BookService.DeleteBook(Book.Id);

            StatusClass = "alert-success";
            Message = "Deleted successfully";

            Saved = true;
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
                Book.CoverImageId = uploadResult.Id;
                Book.CoverImage.Data = ms.ToArray();
            }
        }

        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/admin/book-overview");
        }
    }
}
