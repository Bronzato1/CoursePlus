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
    public class QuizEditBase : ComponentBase
    {
        [Parameter] public int Id { get; set; }
        [JSInvokable] public async Task QuillContentChanged()
        {
            OneQuiz.Description = await JSRuntime.InvokeAsync<string>("QuillFunctions.getQuillHTML", divEditorElement);
        }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public IQuizService QuizService { get; set; }
        [Inject] public IChapterService ChapterService { get; set; }
        [Inject] public IEpisodeService EpisodeService { get; set; }
        [Inject] public ICategoryService CategoryService { get; set; }
        [Inject] public IProfileService ProfileService { get; set; }
        [Inject] public HttpClient Client { get; set; }
        [Inject] public IModalDialogService ModalDialog { get; set; }
        
        public EditForm FormContext { get; set; }
        public QuizTopic OneQuiz { get; set; } = new QuizTopic();
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

            if (Id == 0) // new quiz is being created
            {
                OneQuiz = new QuizTopic { };
            }
            else
            {
                OneQuiz = await QuizService.GetQuiz(Id);
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            { 
                // Attention, sous Google Chrome ca bug à cause de l'extreme lenteur
                await Task.Delay(2000);
                await JSRuntime.InvokeVoidAsync("QuillFunctions.createQuill", divEditorElement);
                await JSRuntime.InvokeVoidAsync("QuillFunctions.notifyQuillChanges", divEditorElement, DotNetObjectReference.Create(this));
                await JSRuntime.InvokeVoidAsync("QuillFunctions.loadQuillContentFromHTML", divEditorElement, OneQuiz.Description);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/admin/quizzes");
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
                NavigationManager.NavigateTo("/admin/quizzes");
            }
        }
        protected async Task<bool> TrySavingChanges()
        {
            if (Id == 0)
            {
                var addedQuiz = await QuizService.AddQuiz(OneQuiz);
                if (addedQuiz != null)
                {
                    Console.WriteLine("Id:{0}", addedQuiz.Id);
                    Id = addedQuiz.Id;
                    OneQuiz.Id = addedQuiz.Id;
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
                await QuizService.UpdateQuiz(OneQuiz);
                StatusClass = "uk-text-success";
                Message = "Playlist updated successfully";
                StateHasChanged();
                return true;
            }
        }
        protected async Task DeleteQuiz()
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the quiz ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await QuizService.DeleteQuiz(OneQuiz.Id);
                StatusClass = "alert-danger";
                Message = "Deleted successfully";
                StateHasChanged();
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/admin/quizzes");
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
                OneQuiz.ImageId = uploadImageResult.ImageId;
                OneQuiz.ThumbnailId = uploadImageResult.ThumbnailId;

                if (OneQuiz.Image == null) // First time image for this quiz
                    OneQuiz.Image = new CoursePlus.Shared.Models.Image();

                OneQuiz.Image.Data = ms.ToArray();
            }
        }
        protected async Task AddChapter()
        {
            if (OneQuiz.Id == 0)
            {
                var response = await ModalDialog.ShowMessageBoxAsync("Question", "Do you want to save this new quiz ?", MessageBoxButtons.YesNo);
                if (response == MessageBoxDialogResult.No) return;
                var result = await TrySavingChanges();
                if (result == false) return;
            }

            ModalDataInputForm frm = new ModalDataInputForm("Add chapter", "Please give a title");

            var titleFld = frm.AddStringField("title", "Title", "", "The title of the chapter");

            if (await frm.ShowAsync(ModalDialog))
            {
                var chapter = new Chapter { Title = titleFld.Value, QuizTopicId = OneQuiz.Id };
                var addedChapter = await ChapterService.AddChapter(chapter);

                if (OneQuiz.Chapters == null)
                    OneQuiz.Chapters = new List<Chapter>();

                OneQuiz.Chapters.Add(addedChapter);
                StateHasChanged();
            }
        }
        protected async Task DeleteChapter(Chapter OneChapter)
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the chapter ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await ChapterService.DeleteChapter(OneChapter.Id);
                OneQuiz.Chapters.Remove(OneChapter);
                StateHasChanged();
            }
        }
        protected async Task AddEpisode(Chapter OneChapter)
        {
            if (OneQuiz.Id == 0)
            {
                var response = await ModalDialog.ShowMessageBoxAsync("Question", "Do you want to save this new quiz ?", MessageBoxButtons.YesNo);
                if (response == MessageBoxDialogResult.No) return;
                var result = await TrySavingChanges();
                if (result == false) return;
            }

            ModalDialogParameters parameters = new ModalDialogParameters();

            parameters.Add("Title", "");
            parameters.Add("VideoId", "");
            parameters.Add("Duration", 0);
            parameters.Add("Trailer", "");

            var dialogResult = await ModalDialog.ShowDialogAsync<EpisodeEdit>("Add episode", new ModalDialogOptions(), parameters);

            if (dialogResult.Success)
            {
                var episode = new Episode 
                { 
                    Title = dialogResult.ReturnParameters.Get<string>("Title"), 
                    VideoId = dialogResult.ReturnParameters.Get<string>("VideoId"),
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
            parameters.Add("VideoId", OneEpisode.VideoId);
            parameters.Add("Duration", OneEpisode.Duration);
            parameters.Add("Trailer", OneEpisode.Trailer);

            var dialogResult = await ModalDialog.ShowDialogAsync<EpisodeEdit>("Edit episode", new ModalDialogOptions(), parameters);

            if (dialogResult.Success)
            {
                OneEpisode.Title = dialogResult.ReturnParameters.Get<string>("Title");
                OneEpisode.VideoId = dialogResult.ReturnParameters.Get<string>("VideoId");
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
