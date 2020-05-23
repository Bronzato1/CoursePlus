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
        [Parameter] public int Id { get; set; }
        [JSInvokable] public async Task QuillContentChanged()
        {
            OnePlaylist.Description = await JSRuntime.InvokeAsync<string>("QuillFunctions.getQuillHTML", divEditorElement);
        }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public IPlaylistService PlaylistService { get; set; }
        [Inject] public IChapterService ChapterService { get; set; }
        [Inject] public IEpisodeService EpisodeService { get; set; }
        [Inject] public ICategoryService CategoryService { get; set; }
        [Inject] public IProfileService ProfileService { get; set; }
        [Inject] public HttpClient Client { get; set; }
        [Inject] public IModalDialogService ModalDialog { get; set; }
        
        public EditForm FormContext { get; set; }
        public Playlist OnePlaylist { get; set; } = new Playlist();
        public ElementReference divEditorElement;
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Profile> Profiles { get; set; } = new List<Profile>();

        public bool EditorEnabled = true;
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        protected override async Task OnInitializedAsync()
        {
            Categories = (await CategoryService.GetCategories()).ToList();
            Profiles = (await ProfileService.GetAllProfiles()).ToList();

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
            var success = await TrySavingChanges();
            if (success == true)
            {
                await Task.Delay(2000);
                NavigationManager.NavigateTo("/admin/playlists");
            }
        }
        protected async Task<bool> TrySavingChanges()
        {
            if (Id == 0)
            {
                var addedPlaylist = await PlaylistService.AddPlaylist(OnePlaylist);
                if (addedPlaylist != null)
                {
                    Console.WriteLine("Id:{0}", addedPlaylist.Id);
                    Id = addedPlaylist.Id;
                    OnePlaylist.Id = addedPlaylist.Id;
                    StatusClass = "uk-text-success";
                    Message = "New playlist added successfully";
                    StateHasChanged();
                    return true;
                }
                else
                {
                    StatusClass = "uk-text-danger";
                    Message = "Something went wrong";
                    return false;
                }
            }
            else
            {
                await PlaylistService.UpdatePlaylist(OnePlaylist);
                StatusClass = "uk-text-success";
                Message = "Playlist updated successfully";
                StateHasChanged();
                return true;
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
            if (OnePlaylist.Id == 0)
            {
                var response = await ModalDialog.ShowMessageBoxAsync("Question", "Do you want to save this new playlist ?", MessageBoxButtons.YesNo);
                if (response == MessageBoxDialogResult.No) return;
                var result = await TrySavingChanges();
                if (result == false) return;
            }

            ModalDataInputForm frm = new ModalDataInputForm("Add chapter", "Please give a title");

            var titleFld = frm.AddStringField("title", "Title", "", "The title of the chapter");

            if (await frm.ShowAsync(ModalDialog))
            {
                var chapter = new Chapter { Title = titleFld.Value, PlaylistId = OnePlaylist.Id };
                var addedChapter = await ChapterService.AddChapter(chapter);

                if (OnePlaylist.Chapters == null)
                    OnePlaylist.Chapters = new List<Chapter>();

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
            if (OnePlaylist.Id == 0)
            {
                var response = await ModalDialog.ShowMessageBoxAsync("Question", "Do you want to save this new playlist ?", MessageBoxButtons.YesNo);
                if (response == MessageBoxDialogResult.No) return;
                var result = await TrySavingChanges();
                if (result == false) return;
            }

            ModalDialogParameters parameters = new ModalDialogParameters();

            parameters.Add("Title", "");
            parameters.Add("VideoUrl", "");
            parameters.Add("Duration", 0);
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
