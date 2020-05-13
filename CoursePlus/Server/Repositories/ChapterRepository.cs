using CoursePlus.Server.Data;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ChapterRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Chapter>> GetList()
        {
            try
            {
                var chapterList = _dbContext.Chapters;
                return await chapterList.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }
        }

        public Chapter GetChapter(int id)
        {
            var chapter = _dbContext.Chapters
                .Where(x => x.Id == id)
                .FirstOrDefault();
            return chapter;
        }

        public Chapter AddChapter(Chapter chapter)
        {
            var addedEntity = _dbContext.Chapters.Add(chapter);
            _dbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public Chapter UpdateChapter(Chapter chapter)
        {
            var foundChapter = _dbContext.Chapters.FirstOrDefault(e => e.Id == chapter.Id);

            if (foundChapter != null)
            {
                foundChapter.Title = chapter.Title;

                _dbContext.SaveChanges();

                return foundChapter;
            }

            return null;
        }

        public void DeleteChapter(int id)
        {
            var foundChapter = _dbContext.Chapters.FirstOrDefault(e => e.Id == id);
            if (foundChapter == null) return;

            _dbContext.Chapters.Remove(foundChapter);
            _dbContext.SaveChanges();
        }
    }
}
