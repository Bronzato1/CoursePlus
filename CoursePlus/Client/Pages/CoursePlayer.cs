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
    public class CoursePlayerBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public ICourseService CourseService { get; set; }

        public Course OneCourse { get; set; }

        protected override async Task OnInitializedAsync()
        {
            OneCourse = await CourseService.GetCourse(Id);
        }

        protected async Task GoBack()
        {
            await JSRuntime.InvokeVoidAsync("History.goBack");
        }
    }
}
