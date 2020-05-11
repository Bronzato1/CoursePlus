using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using CoursePlus.Server.Data;

namespace CoursePlus.Server.Repositories
{
    public class AvatarRepository : IAvatarRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AvatarRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateAvatarFromUrl(string url)
        {
            var WebClient = new WebClient();
            var imageBytes = WebClient.DownloadData(url);
            var stream = new MemoryStream(imageBytes);
            var avatar = new Avatar { Data = stream.ToArray() };
            _dbContext.Avatars.Add(avatar);
            await _dbContext.SaveChangesAsync();
            return avatar.Id;
        }
    }
}
