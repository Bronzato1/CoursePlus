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
    public class ProfileEditBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IProfileService ProfileService { get; set; }
        [Inject]
        public HttpClient HttpClient { get; set; }
        [Inject]
        public IModalDialogService ModalDialog { get; set; }
        [Inject]
        public UserValidator UserValidator { get; set; }

        public EditForm FormContext { get; set; }

        public Profile OneProfile { get; set; } = new Profile();

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
                OneProfile = new Profile { User = new CustomUser(), Joined = DateTime.Now };
            }
            else
            {
                OneProfile = await ProfileService.GetProfile(Id);
            }
        }

        protected async Task HandleValidSubmit()
        {
            if (Id == 0)
            {
                var addedProfile = await ProfileService.AddProfile(OneProfile);
                if (addedProfile != null)
                {
                    StatusClass = "uk-text-success";
                    Message = "New profile added successfully";
                    StateHasChanged();
                    await Task.Delay(2000);
                    NavigationManager.NavigateTo("/admin/profiles");
                }
                else
                {
                    StatusClass = "uk-text-danger";
                    Message = "Something went wrong";
                }
            }
            else
            {
                await ProfileService.UpdateProfile(OneProfile);
                StatusClass = "uk-text-success";
                Message = "Profile updated successfully";
                StateHasChanged();
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/admin/profiles");
            }
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "uk-text-warning";
            Message = "Validation errors";
        }

        protected async Task DeleteProfile()
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the profile ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await ProfileService.DeleteProfile(OneProfile.Id);
                StatusClass = "alert-danger";
                Message = "Deleted successfully";
                StateHasChanged();
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/admin/profiles");
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
                var result = await HttpClient.PostAsync("api/upload/avatar", content);
                result.EnsureSuccessStatusCode();
                var uploadAvatarResult = JsonSerializer.Deserialize<UploadAvatarResult>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                OneProfile.User.AvatarId = uploadAvatarResult.AvatarId;

                if (OneProfile.User.Avatar == null) // First time image for this instructor
                    OneProfile.User.Avatar = new CoursePlus.Shared.Models.Avatar();

                OneProfile.User.Avatar.Data = ms.ToArray();
            }
        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/admin/profiles");
        }
    }
}
