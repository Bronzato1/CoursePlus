using CoursePlus.Client.Services;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages
{
    public class BookDetailBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IBookService BookService { get; set; }
        [Inject]
        protected IJSRuntime JsRuntime { get; set; }

        public Book OneBook { get; set; }

        protected override async Task OnInitializedAsync()
        {
            OneBook = await BookService.GetBook(Id);
        }

        protected async Task OpenCoverInNewTab()
        {
            await JsRuntime.InvokeAsync<object>("openBase64ImageInNewTab", OneBook.Image.Data);
        }

        protected async Task NavigateToPurchaseLink()
        {
            await JsRuntime.InvokeAsync<object>("open", OneBook.PurchaseLink, "_blank");
        }
    }
}
