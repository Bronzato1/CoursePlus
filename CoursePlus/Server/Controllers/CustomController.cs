using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursePlus.Server.Services;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoursePlus.Server.Controllers
{
    [ApiController]
    public class CustomController : ControllerBase
    {
        private readonly IWebCrawlerService _webCrawlerService;

        public CustomController(IWebCrawlerService webCrawlerService)
        {
            _webCrawlerService = webCrawlerService;
        }

        [HttpGet("api/custom/getFakeUsers")]
        public async Task<FakeUserModel[]> GetFakeUsers()
        {
            return await _webCrawlerService.GetFakeUsers();
        }
    }
}
