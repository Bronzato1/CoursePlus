using CoursePlus.Server.Controllers;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public interface IChapterRepository
    {
        Task<List<Chapter>> GetChapters();

        public Chapter GetChapter(int id);

        public Chapter AddChapter(Chapter chapter);

        public Chapter UpdateChapter(Chapter chapter);

        public void DeleteChapter(int id);
    }
}
