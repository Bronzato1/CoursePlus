using CoursePlus.Client.Services;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages
{
    public class CourseDetailBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public ICourseService CourseService { get; set; }

        public Course OneCourse { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            OneCourse = await CourseService.GetCourse(Id);
        }
    }
}
