﻿using CoursePlus.Client.Services;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages
{
    public class CourseListBase : ComponentBase
    {
        [Inject]
        public ICourseService CourseService { get; set; }
        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public PaginatedList<Course> SomeCourses { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public FilterModel CurrentFilterModel = new FilterModel();

        public class FilterModel
        {
            public EnumDifficulty? DifficultyFilter { get; set; }
            public EnumDuration? DurationFilter { get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            await FilterCourses();
            Categories = await CategoryService.GetCategories();
        }

        protected async Task FilterCourses()
        {
            PaginatedList<Course> courses;

            var currentFilters = new Dictionary<string, string>();
            var currentSortOrder = new Dictionary<string, string>();

            if (CurrentFilterModel.DifficultyFilter.HasValue) 
            {
                currentFilters.Add("Difficulty", CurrentFilterModel.DifficultyFilter.Value.ToString());
            }

            if (CurrentFilterModel.DurationFilter.HasValue)
            {
                currentFilters.Add("Duration", CurrentFilterModel.DurationFilter.Value.ToString());
            }

            //if (!string.IsNullOrEmpty(currentSortField) && !string.IsNullOrEmpty(currentSortOrder))
            //    currentSortOrder.Add(currentSortField, currentSortOrder);

            courses = await CourseService.GetCourses(filters: currentFilters, sortOrder: currentSortOrder);
            
            SomeCourses = courses;
        }

        protected async void PageIndexChanged(PaginatedList<Course> context, int newPageNumber)
        {
            if (newPageNumber < 1 || newPageNumber > context.TotalPages)
            {
                return;
            }

            var cptr = context.Items.Count;

            //var data = await CourseService.GetCourses(pageNumber: newPageNumber, filterField: "Difficulty", filterValue: CurrentFilterModel.DifficultyFilter.ToString());

            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(CurrentFilterModel.DifficultyFilter.ToString()))
                filters.Add("Difficulty", CurrentFilterModel.DifficultyFilter.ToString());
            var data = await CourseService.GetCourses(pageNumber: newPageNumber, filters: filters);

            foreach (var elm in data.Items)
            {
                context.Items.Add(elm);
            }

            context.Items.RemoveRange(0, cptr);

            context.PageIndex = data.PageIndex;
            context.TotalPages = data.TotalPages;

            StateHasChanged();
        }

        protected void ViewCourse(Course OneCourse)
        {
            NavigationManager.NavigateTo("/course/" + OneCourse.Id);
        }
    }
}
