using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoursePlus.Server.Data;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoursePlus.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext _dbContext;

        public UploadController(IWebHostEnvironment environment, ApplicationDbContext dbContext)
        {
            this.environment = environment;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            if (HttpContext.Request.Form.Files.Count == 1)
            {
                var fileForm = HttpContext.Request.Form.Files.First();
                var filePath = Path.Combine(environment.ContentRootPath, fileForm.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileForm.CopyToAsync(stream);
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);

                var newFile = new CoursePlus.Shared.Models.File
                {
                    Data = fileBytes
                };

                _dbContext.Files.Add(newFile);
                await _dbContext.SaveChangesAsync();
                System.IO.File.Delete(filePath);
                return Ok(new UploadResult { Id = newFile.Id });
            }
            return BadRequest();
        }
    }
}
