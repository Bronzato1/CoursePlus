using CoursePlus.Client.Services;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages
{
    public class CoursePlayerBase : ComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> authState { get; set; }
        [Parameter]
        public int Id { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public ICourseService CourseService { get; set; }
        [Inject]
        public IStudentService StudentService { get; set; }

        public int StudentId { get; set; }

        public Course OneCourse { get; set; }

        public Student OneStudent { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ClaimsPrincipal principal = (await authState).User;

            if (principal.Identity.IsAuthenticated)
            {
                Claim claim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                String userId = claim.Value;
                OneStudent = await StudentService.GetStudentByUserId(userId);
            }
            
            OneCourse = await CourseService.GetCourse(Id);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await JSRuntime.InvokeVoidAsync("Player.initialize");
        }

        protected async Task LoadYouTubeVideo(Episode OneEpisode)
        {
            await JSRuntime.InvokeVoidAsync("Player.loadYouTubeVideo", OneEpisode.VideoUrl);
        }

        protected async Task GoBack()
        {
            await JSRuntime.InvokeVoidAsync("History.goBack");
        }

        protected bool StudentAlreadyWatchedThisEpisode(Episode OneEpisode)
        {
            if (OneStudent == null)
            {
                // Current user is not student
                return false;
            }

            var watched = OneEpisode.WatchHistory.FirstOrDefault(x => x.StudentId == OneStudent.Id) != null;
            return watched;
        }
    }
}
