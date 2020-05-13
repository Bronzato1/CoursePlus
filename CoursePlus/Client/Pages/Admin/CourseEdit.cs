using Blazor.ModalDialog;
using BlazorInputFile;
using CoursePlus.Client.Services;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages.Admin
{
    public class CourseEditBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public ICourseService CourseService { get; set; }
        [Inject]
        public IChapterService ChapterService { get; set; }
        [Inject]
        public IEpisodeService EpisodeService { get; set; }
        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public HttpClient Client { get; set; }
        [Inject]
        public IModalDialogService ModalDialog { get; set; }

        public EditForm FormContext { get; set; }

        public Course OneCourse { get; set; } = new Course();

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;

        public List<Category> Categories { get; set; } = new List<Category>();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            Categories = (await CategoryService.GetCategories()).ToList();

            if (Id == 0) // new course is being created
            {
                OneCourse = new Course { };
            }
            else
            {
                OneCourse = await CourseService.GetCourse(Id);
            }
        }

        protected async Task HandleValidSubmit()
        {
            if (Id == 0)
            {
                var addedCourse = await CourseService.AddCourse(OneCourse);
                if (addedCourse != null)
                {
                    StatusClass = "uk-text-success";
                    Message = "New course added successfully";
                    StateHasChanged();
                    await Task.Delay(2000);
                    NavigationManager.NavigateTo("/admin/courses");
                }
                else
                {
                    StatusClass = "uk-text-danger";
                    Message = "Something went wrong";
                }
            }
            else
            {
                await CourseService.UpdateCourse(OneCourse);
                StatusClass = "uk-text-success";
                Message = "Course updated successfully";
                StateHasChanged();
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/admin/courses");
            }
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "uk-text-warning";
            Message = "Validation errors";
        }

        protected async Task DeleteCourse()
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the course ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await CourseService.DeleteCourse(OneCourse.Id);
                StatusClass = "alert-danger";
                Message = "Deleted successfully";
                StateHasChanged();
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/admin/courses");
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
                var result = await Client.PostAsync("api/upload/image", content);
                result.EnsureSuccessStatusCode();
                var uploadImageResult = JsonSerializer.Deserialize<UploadImageResult>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                OneCourse.ImageId = uploadImageResult.ImageId;
                OneCourse.ThumbnailId = uploadImageResult.ThumbnailId;

                if (OneCourse.Image == null) // First time image for this course
                    OneCourse.Image = new CoursePlus.Shared.Models.Image();

                OneCourse.Image.Data = ms.ToArray();
            }
        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/admin/courses");
        }

        protected async Task AddChapter()
        {
            ModalDataInputForm frm = new ModalDataInputForm("Add chapter", "Please give a title");

            var titleFld = frm.AddStringField("title", "Title", "", "The title of the chapter");

            if (await frm.ShowAsync(ModalDialog))
            {
                var chapter = new Chapter { Title = titleFld.Value, CourseId = OneCourse.Id };

                OneCourse.Chapters.Add(chapter);
                await ChapterService.AddChapter(chapter);
                StateHasChanged();
            }
        }

        protected async Task DeleteChapter(Chapter OneChapter)
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the chapter ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await ChapterService.DeleteChapter(OneChapter.Id);
                OneCourse.Chapters.Remove(OneChapter);
                StateHasChanged();
            }
        }

        protected async Task AddEpisode(Chapter OneChapter)
        {
            ModalDataInputForm frm = new ModalDataInputForm("Add episode", "Please fill in the information below");

            var titleFld = frm.AddStringField("title", "Title", "", "The title of the episode");
            var urlFld = frm.AddStringField("url", "Url", "", "The youtube url of the episode");

            if (await frm.ShowAsync(ModalDialog))
            {
                var episode = new Episode { Title = titleFld.Value, VideoUrl = urlFld.Value, ChapterId = OneChapter.Id };

                OneChapter.Episodes.Add(episode);
                await EpisodeService.AddEpisode(episode);
                StateHasChanged();
            }
        }

        protected async Task EditEpisode(Chapter OneChapter, Episode OneEpisode)
        {
            ModalDataInputForm frm = new ModalDataInputForm("Edit episode", "Please fill in the information below");

            var titleFld = frm.AddStringField("title", "Title", OneEpisode.Title, "The title of the episode");
            var urlFld = frm.AddStringField("url", "Url", OneEpisode.VideoUrl, "The youtube url of the episode");

            if (await frm.ShowAsync(ModalDialog))
            {
                OneEpisode.Title = titleFld.Value;
                OneEpisode.VideoUrl = urlFld.Value;
                await EpisodeService.UpdateEpisode(OneEpisode);
                StateHasChanged();
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
