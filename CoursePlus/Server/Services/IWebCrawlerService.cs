using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Services
{
    public interface IWebCrawlerService
    {
        Task<FakeUserModel[]> GetFakeUsers();
    }
}
