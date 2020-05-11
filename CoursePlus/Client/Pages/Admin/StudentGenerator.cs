using Blazor.ModalDialog;
using CoursePlus.Client.Services;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages.Admin
{
    public class StudentGeneratorBase : ComponentBase
    {
        [Inject]
        public IStudentService StudentService { get; set; }
        [Inject]
        public IModalDialogService ModalDialog { get; set; }
        [Inject] 
        HttpClient HttpClient { get; set; }

        public WebClient WebClient = new WebClient();

        public List<FakeStudentModel> SomeUsers { get; set; } = new List<FakeStudentModel>();

        protected override async Task OnInitializedAsync()
        {
            await LoadMoreUsers();
        }

        protected async Task LoadMoreUsers()
        {
            var users = await StudentService.GetFakeStudents();
            SomeUsers.AddRange(users);
        }

        protected async Task CreateStudentProfiles()
        {
            var result = await StudentService.CreateFakeStudents(SomeUsers);

            if (result != null)
            {
                SomeUsers = new List<FakeStudentModel>();
                await ModalDialog.ShowMessageBoxAsync("Student profiles created successfully", $"Profiles created: {result.CptrSucceed} - Failed: {result.CptrFailed}", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
            }
        }

        public void Delete(FakeStudentModel OneUser)
        {
            SomeUsers.Remove(OneUser);
        }
    }
}