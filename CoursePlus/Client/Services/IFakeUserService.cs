using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public interface IFakeUserService
    {
        Task<FakeUserModel[]> GetFakeUsers();
    }
}
