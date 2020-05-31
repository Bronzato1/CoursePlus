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
    public class ProfileGeneratorBase : ComponentBase
    {
        [Inject] public IProfileService ProfileService { get; set; }
        [Inject] public IModalDialogService ModalDialog { get; set; }

        public List<FakeProfileModel> SomeUsers { get; set; } = new List<FakeProfileModel>();
        protected override async Task OnInitializedAsync()
        {
            await LoadMoreUsers();
        }
        protected async Task LoadMoreUsers()
        {
            var users = await ProfileService.GetFakeProfiles();
            SomeUsers.AddRange(users);
        }
        protected async Task CreateProfiles()
        {
            var result = await ProfileService.CreateFakeProfiles(SomeUsers);

            if (result != null)
            {
                SomeUsers = new List<FakeProfileModel>();
                await ModalDialog.ShowMessageBoxAsync("Profile profiles created successfully", $"Profiles created: {result.CptrSucceed} - Failed: {result.CptrFailed}", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
            }
        }
        public void Delete(FakeProfileModel OneUser)
        {
            SomeUsers.Remove(OneUser);
        }
    }
}