using CoursePlus.Client.Services;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages.Admin
{
    public class StudentGeneratorBase : ComponentBase
    {
        [Inject]
        public IFakeUserService FakeUserService { get; set; }

        public List<FakeUserModel> SomeUsers { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var users = await FakeUserService.GetFakeUsers();
            SomeUsers = users.ToList();
        }

        protected void HandleValidSubmit()
        {

        }

        protected void HandleInvalidSubmit()
        {

        }

        public void Delete(FakeUserModel OneUser)
        {
            //var OneUser = SomeUsers.Select(x => x.Index == OneUser.Index);

            SomeUsers.Remove(OneUser);
        }
    }
}
