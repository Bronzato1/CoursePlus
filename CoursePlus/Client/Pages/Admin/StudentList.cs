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
    public class StudentListBase : ComponentBase
    {
        [Inject]
        public IStudentService StudentService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IModalDialogService ModalDialog { get; set; }

        public PaginatedList<Student> PaginatedList = new PaginatedList<Student>();

        public IEnumerable<Student> SomeStudents { get { return PaginatedList.Items; } }

        int currentPageNumber = 1;

        string currentSortField = "User.FirstName";

        string currentSortOrder = "Asc";

        string currentFilterField = string.Empty;

        string currentFilterValue = string.Empty;

        protected override async Task OnInitializedAsync()
        {
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

        protected void EditStudent(Student student)
        {
            NavigationManager.NavigateTo("/admin/student/" + student.Id);
        }

        protected void AddStudent()
        {
            NavigationManager.NavigateTo("/admin/student/0");
        }

        public async Task DeleteStudent(Student student)
        {
            MessageBoxDialogResult result = await ModalDialog.ShowMessageBoxAsync("Confirm Delete", "Are you sure you want to delete the student ?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (result == MessageBoxDialogResult.Yes)
            {
                await StudentService.DeleteStudent(student.Id);
                await RefreshListAsync();
            }
        }

        public async Task RefreshListAsync()
        {
            PaginatedList = await StudentService.GetStudents(currentPageNumber, currentSortField, currentSortOrder, currentFilterField, currentFilterValue);
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
