using Blazor.ModalDialog;
using CoursePlus.Client.Services;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages.Admin
{
    public class CourseListBase : ComponentBase
    {
        [Inject]
        public ICourseService CourseService { get; set; }
        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IModalDialogService ModalDialog { get; set; }

        public PaginatedList<Course> PaginatedList = new PaginatedList<Course>();

        public IEnumerable<Course> SomeCourses { get { return PaginatedList.Items; } }

        public IEnumerable<Category> SomeCategories { get; set; }

        int currentPageNumber = 1;

        string currentSortField = "Title";

        string currentSortOrder = "Asc";

        string currentFilterField = string.Empty;

        string currentFilterValue = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            SomeCategories = await CategoryService.GetCategories();
            await RefreshListAsync();
        }

        public async void PageIndexChanged(int newPageNumber)
        {
            if (newPageNumber < 1 || newPageNumber > PaginatedList.TotalPages)
            {
                return;
            }

            currentPageNumber = newPageNumber;
            await RefreshListAsync();
            StateHasChanged();
        }

        protected void EditCourse(Course course)
        {
            NavigationManager.NavigateTo("/admin/course/" + course.Id);
        }

        protected void AddCourse()
        {
            NavigationManager.NavigateTo("/admin/course/0");
        }

        public async Task DeleteCourse(Course course)
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the course ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await CourseService.DeleteCourse(course.Id);
                await RefreshListAsync();
            }
        }

        public async Task RefreshListAsync()
        {
            var filters = new Dictionary<string, string>();
            var sortOrder = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(currentFilterField) && !string.IsNullOrEmpty(currentFilterValue))
                filters.Add(currentFilterField, currentFilterValue);

            if (!string.IsNullOrEmpty(currentSortField) && !string.IsNullOrEmpty(currentSortOrder))
                sortOrder.Add(currentSortField, currentSortOrder);

            PaginatedList = await CourseService.GetCourses(currentPageNumber, sortOrder, filters);
        }

        public async Task Sort(string sortField)
        {
            if (sortField.Equals(currentSortField))
            {
                currentSortOrder = currentSortOrder.Equals("Asc") ? "Desc" : "Asc";
            }
            else
            {
                currentSortField = sortField;
                currentSortOrder = "Asc";
            }
            await RefreshListAsync();
        }

        public string SortIndicator(string sortField)
        {
            if (sortField.Equals(currentSortField))
            {
                return currentSortOrder.Equals("Asc") ? "icon-material-outline-arrow-drop-down" : "icon-material-outline-arrow-drop-up";
            }
            return string.Empty;
        }

        public async Task Filter(string field, string value)
        {
            currentPageNumber = 1;
            currentFilterField = field;
            currentFilterValue = value;

            await RefreshListAsync();
        }

        public string FilterIndicator(string filterField, string filterValue)
        {
            if (filterField.Equals(currentFilterField) && filterValue.Equals(currentFilterValue))
            {
                return "uk-active";
            }
            return string.Empty;
        }
    }
}
