using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public interface IChapterService
    {
        Task<List<Chapter>> GetChapters();

        Task<Chapter> GetChapter(int id);

        Task<Chapter> AddChapter(Chapter chapter);

        Task UpdateChapter(Chapter chapter);

        Task DeleteChapter(int id);
    }
}
