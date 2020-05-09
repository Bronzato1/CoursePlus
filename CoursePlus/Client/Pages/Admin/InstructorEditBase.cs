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
using CoursePlus.Shared.Infrastructure;

namespace CoursePlus.Client.Pages.Admin
{
    public class InstructorEditBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IInstructorService InstructorService { get; set; }
        [Inject]
        public HttpClient Client { get; set; }
        [Inject]
        public IModalDialogService ModalDialog { get; set; }
        [Inject]
        public UserValidator UserValidator { get; set; }

        public EditForm FormContext { get; set; }

        public Instructor OneInstructor { get; set; } = new Instructor();

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            if (Id == 0) // new instructor is being created
            {
                OneInstructor = new Instructor { User = new CustomUser() };
            }
            else
            {
                OneInstructor = await InstructorService.GetInstructor(Id);
            }
        }

        protected async Task HandleValidSubmit()
        {
            if (Id == 0)
            {
                var addedInstructor = await InstructorService.AddInstructor(OneInstructor);
                if (addedInstructor != null)
                {
                    StatusClass = "uk-text-success";
                    Message = "New instructor added successfully";
                    StateHasChanged();
                    await Task.Delay(2000);
                    NavigationManager.NavigateTo("/admin/instructors");
                }
                else
                {
                    StatusClass = "uk-text-danger";
                    Message = "Something went wrong";
                }
            }
            else
            {
                await InstructorService.UpdateInstructor(OneInstructor);
                StatusClass = "uk-text-success";
                Message = "Instructor updated successfully";
                StateHasChanged();
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/admin/instructors");
            }
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "uk-text-warning";
            Message = "Validation errors";
        }

        protected async Task DeleteInstructor()
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the instructor ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await InstructorService.DeleteInstructor(OneInstructor.Id);
                StatusClass = "alert-danger";
                Message = "Deleted successfully";
                StateHasChanged();
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/admin/instructors");
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
                var result = await Client.PostAsync("api/upload/avatar", content);
                result.EnsureSuccessStatusCode();
                var uploadAvatarResult = JsonSerializer.Deserialize<UploadAvatarResult>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                OneInstructor.User.AvatarId = uploadAvatarResult.AvatarId;

                if (OneInstructor.User.Avatar == null) // First time image for this instructor
                    OneInstructor.User.Avatar = new CoursePlus.Shared.Models.Avatar();

                OneInstructor.User.Avatar.Data = ms.ToArray();
            }
        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/admin/instructors");
        }
    }
}
