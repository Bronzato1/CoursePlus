using CoursePlus.Server.Data;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public QuizRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<Quiz>> GetQuizzes(int? pageNumber, IDictionary<string, string> sortOrder, IDictionary<string, string> filters)
        {
            try
            {
                int pageSize = 6;
                var quizList = _dbContext.Quizzes
                                         .Include(x => x.Thumbnail)
                                         .Include(x => x.Category).AsQueryable();

                if (filters != null)
                { 
                    foreach (var filter in filters)
                    {
                        var filterField = filter.Key;
                        var filterValue = filter.Value;

                        quizList = quizList.WhereDynamic(filterField, filterValue);
                    }
                }
                if (sortOrder != null)
                { 
                    foreach (var sort in sortOrder)
                    {
                        var sortField = sort.Key;
                        var sortValue = sort.Value;

                        quizList = quizList.OrderByDynamic(sortField, sortValue);
                    }
                }

                return await PaginatedList<Quiz>.CreateAsync(quizList.AsNoTracking(), pageNumber ?? 1, pageSize);
            }
            catch
            {
                throw new ApplicationException();
            }
        }

        public async Task<List<Quiz>> GetPopularQuizzes()
        {
            return await _dbContext.Quizzes
                                   .Include(x => x.Category)
                                   .Include(x => x.Image)
                                   .Take(10).ToListAsync();
        }

        public Quiz GetQuiz(int id)
        {
            var quiz = _dbContext.Quizzes
                .Where(x => x.Id == id)
                .Include(x => x.Image)
                .Include(x => x.Category)
                .FirstOrDefault();

            return quiz;
        }

        public Quiz AddQuiz(Quiz quiz)
        {
            var addedEntity = _dbContext.Quizzes.Add(quiz);
            _dbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public Quiz UpdateQuiz(Quiz quiz)
        {
            var foundQuiz = _dbContext.Quizzes.FirstOrDefault(e => e.Id == quiz.Id);

            if (foundQuiz != null)
            {
                foundQuiz.Title = quiz.Title;
                foundQuiz.Description = quiz.Description;
                foundQuiz.ImageId = quiz.ImageId;
                foundQuiz.ThumbnailId = quiz.ThumbnailId;
                foundQuiz.CategoryId = quiz.CategoryId;

                _dbContext.SaveChanges();

                return foundQuiz;
            }

            return null;
        }

        public void DeleteQuiz(int id)
        {
            var foundQuiz = _dbContext.Quizzes.FirstOrDefault(e => e.Id == id);
            if (foundQuiz == null) return;

            _dbContext.Quizzes.Remove(foundQuiz);
            _dbContext.SaveChanges();
        }

        public async Task<bool> GenerateImagesAndThumbnailsFromPath()
        {
            var quizzes = _dbContext.Quizzes.Include(x => x.Image);

            foreach (var quiz in quizzes)
            {
                if (quiz.Image == null && !string.IsNullOrEmpty(quiz.ImagePath))
                {
                    var data = await File.ReadAllBytesAsync(quiz.ImagePath);
                    var newImage = new Image { Data = data };
                    quiz.Image = newImage;
                }

                if (quiz.Thumbnail == null && !string.IsNullOrEmpty(quiz.ThumbnailPath))
                {
                    var data = await File.ReadAllBytesAsync(quiz.ThumbnailPath);
                    var newThumbnail = new Thumbnail { Data = data };
                    quiz.Thumbnail = newThumbnail;
                }
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
