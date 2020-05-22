using Blazor.ModalDialog;
using BlazorInputFile;
using CoursePlus.Client.Services;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages.Admin
{
    public class PlaylistEditBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        [Inject]
        public IPlaylistService PlaylistService { get; set; }
        [Inject]
        public IChapterService ChapterService { get; set; }
        [Inject]
        public IEpisodeService EpisodeService { get; set; }
        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public IInstructorService InstructorService { get; set; }
        [Inject]
        public HttpClient Client { get; set; }
        [Inject]
        public IModalDialogService ModalDialog { get; set; }
        [JSInvokable]
        public async Task QuillContentChanged()
        {
            OnePlaylist.Description = await JSRuntime.InvokeAsync<string>("QuillFunctions.getQuillHTML", divEditorElement);
        }

        public EditForm FormContext { get; set; }
        public Playlist OnePlaylist { get; set; } = new Playlist();
        public ElementReference divEditorElement;
        public bool EditorEnabled = true;

        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        protected override async Task OnInitializedAsync()
        {
            Categories = (await CategoryService.GetCategories()).ToList();
            Instructors = (await InstructorService.GetAllInstructors()).ToList();

            if (Id == 0) // new playlist is being created
            {
                OnePlaylist = new Playlist { };
            }
            else
            {
                OnePlaylist = await PlaylistService.GetPlaylist(Id);
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Delay(1000);
                await JSRuntime.InvokeVoidAsync("QuillFunctions.createQuill", divEditorElement);
                await JSRuntime.InvokeVoidAsync("QuillFunctions.notifyQuillChanges", divEditorElement, DotNetObjectReference.Create(this));
                await JSRuntime.InvokeVoidAsync("QuillFunctions.loadQuillContentFromHTML", divEditorElement, OnePlaylist.Description);
            }
        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/admin/playlists");
        }
        protected void HandleInvalidSubmit()
        {
            StatusClass = "uk-text-warning";
            Message = "Validation errors";
        }

        protected async Task HandleValidSubmit()
        {
            if (Id == 0)
            {
                var addedPlaylist = await PlaylistService.AddPlaylist(OnePlaylist);
                if (addedPlaylist != null)
                {
                    StatusClass = "uk-text-success";
                    Message = "New playlist added successfully";
                    StateHasChanged();
                    await Task.Delay(2000);
                    NavigationManager.NavigateTo("/admin/playlists");
                }
                else
                {
                    StatusClass = "uk-text-danger";
                    Message = "Something went wrong";
                }
            }
            else
            {
                await PlaylistService.UpdatePlaylist(OnePlaylist);
                StatusClass = "uk-text-success";
                Message = "Playlist updated successfully";
                StateHasChanged();
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/admin/playlists");
            }
        }
        protected async Task DeletePlaylist()
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the playlist ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await PlaylistService.DeletePlaylist(OnePlaylist.Id);
                StatusClass = "alert-danger";
                Message = "Deleted successfully";
                StateHasChanged();
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/admin/playlists");
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
                var result = await Client.PostAsync("api/upload/image/390/300", content);
                result.EnsureSuccessStatusCode();
                var uploadImageResult = JsonSerializer.Deserialize<UploadImageResult>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                OnePlaylist.ImageId = uploadImageResult.ImageId;
                OnePlaylist.ThumbnailId = uploadImageResult.ThumbnailId;

                if (OnePlaylist.Image == null) // First time image for this playlist
                    OnePlaylist.Image = new CoursePlus.Shared.Models.Image();

                OnePlaylist.Image.Data = ms.ToArray();
            }
        }
        protected async Task AddChapter()
        {
            ModalDataInputForm frm = new ModalDataInputForm("Add chapter", "Please give a title");

            var titleFld = frm.AddStringField("title", "Title", "", "The title of the chapter");

            if (await frm.ShowAsync(ModalDialog))
            {
                var chapter = new Chapter { Title = titleFld.Value, PlaylistId = OnePlaylist.Id };
                var addedChapter = await ChapterService.AddChapter(chapter);
                OnePlaylist.Chapters.Add(addedChapter);
                StateHasChanged();
            }
        }
        protected async Task DeleteChapter(Chapter OneChapter)
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the chapter ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await ChapterService.DeleteChapter(OneChapter.Id);
                OnePlaylist.Chapters.Remove(OneChapter);
                StateHasChanged();
            }
        }
        protected async Task AddEpisode(Chapter OneChapter)
        {
            ModalDialogParameters parameters = new ModalDialogParameters();

            parameters.Add("Title", "");
            parameters.Add("VideoUrl", "");
            parameters.Add("Duration", "");
            parameters.Add("Trailer", "");

            var dialogResult = await ModalDialog.ShowDialogAsync<EpisodeEdit>("Add episode", new ModalDialogOptions(), parameters);

            if (dialogResult.Success)
            {
                var episode = new Episode 
                { 
                    Title = dialogResult.ReturnParameters.Get<string>("Title"), 
                    VideoUrl = dialogResult.ReturnParameters.Get<string>("VideoUrl"),
                    Duration = dialogResult.ReturnParameters.Get<int>("Duration"),
                    Trailer = dialogResult.ReturnParameters.Get<string>("Trailer"),
                    ChapterId = OneChapter.Id 
                };

                if (OneChapter.Episodes == null)
                    OneChapter.Episodes = new List<Episode>();

                OneChapter.Episodes.Add(episode);
                await EpisodeService.AddEpisode(episode);
                StateHasChanged();
            }
        }
        protected async Task EditEpisode(Chapter OneChapter, Episode OneEpisode)
        {
            ModalDialogParameters parameters = new ModalDialogParameters();

            parameters.Add("Title", OneEpisode.Title);
            parameters.Add("VideoUrl", OneEpisode.VideoUrl);
            parameters.Add("Duration", OneEpisode.Duration);
            parameters.Add("Trailer", OneEpisode.Trailer);

            var dialogResult = await ModalDialog.ShowDialogAsync<EpisodeEdit>("Edit episode", new ModalDialogOptions(), parameters);

            if (dialogResult.Success)
            {
                OneEpisode.Title = dialogResult.ReturnParameters.Get<string>("Title");
                OneEpisode.VideoUrl = dialogResult.ReturnParameters.Get<string>("VideoUrl");
                OneEpisode.Duration = dialogResult.ReturnParameters.Get<int>("Duration");
                OneEpisode.Trailer = dialogResult.ReturnParameters.Get<string>("Trailer");
                await EpisodeService.UpdateEpisode(OneEpisode);
            }
        }
        protected async Task DeleteEpisode(Chapter OneChapter, Episode OneEpisode)
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the episode ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await EpisodeService.DeleteEpisode(OneEpisode.Id);
                OneChapter.Episodes.Remove(OneEpisode);
                StateHasChanged();
            }
        }
    }
}
