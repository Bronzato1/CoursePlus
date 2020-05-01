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
using System.Drawing.Imaging;

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

                System.Drawing.Image img;
                System.Drawing.Image thumb;

                Image newImage;
                Thumbnail newThumbnail;

                using (MemoryStream ms = new MemoryStream())
                {
                    await fileForm.CopyToAsync(ms);
                    img = System.Drawing.Image.FromStream(ms);
                    newImage = new Image { Data = ms.ToArray() };
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    thumb = img.GetThumbnailImage(300, 390, () => false, IntPtr.Zero);
                    thumb.Save(ms, ImageFormat.Jpeg);
                    newThumbnail = new Thumbnail { Data = ms.ToArray() };
                }

                _dbContext.Images.Add(newImage);
                _dbContext.Thumbnails.Add(newThumbnail);

                await _dbContext.SaveChangesAsync();
                
                return Ok(new UploadResult { ImageId = newImage.Id, ThumbnailId = newThumbnail.Id });
            }
            return BadRequest();
        }
    }
}
