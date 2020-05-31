using CoursePlus.Server.Data;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using JsonNet.PrivateSettersContractResolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<PaginatedList<QuizTopic>> GetQuizzes(int? pageNumber, IDictionary<string, string> sortOrder, IDictionary<string, string> filters)
        {
            try
            {
                int pageSize = 6;
                var quizList = _dbContext.QuizTopics
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

                return await PaginatedList<QuizTopic>.CreateAsync(quizList.AsNoTracking(), pageNumber ?? 1, pageSize);
            }
            catch
            {
                throw new ApplicationException();
            }
        }

        public async Task<List<QuizTopic>> GetPopularQuizzes()
        {
            return await _dbContext.QuizTopics
                                   .Include(x => x.Category)
                                   .Include(x => x.Thumbnail)
                                   .Take(6).ToListAsync();
        }

        public QuizTopic GetQuiz(int id)
        {
            var quiz = _dbContext.QuizTopics
                .Where(x => x.Id == id)
                .Include(x => x.Image)
                .Include(x => x.Category)
                .FirstOrDefault();

            return quiz;
        }

        public QuizTopic AddQuiz(QuizTopic quiz)
        {
            var addedEntity = _dbContext.QuizTopics.Add(quiz);
            _dbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public QuizTopic UpdateQuiz(QuizTopic quiz)
        {
            var foundQuiz = _dbContext.QuizTopics.FirstOrDefault(e => e.Id == quiz.Id);

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
            var foundQuiz = _dbContext.QuizTopics.FirstOrDefault(e => e.Id == id);
            if (foundQuiz == null) return;

            _dbContext.QuizTopics.Remove(foundQuiz);
            _dbContext.SaveChanges();
        }

        public async Task<int> CreateQuizzesFromJsonOfOpenQuizzDB()
        {
            try
            {
                QuizTopic quiz;
                int cptr = 0;

                for (var index=1; index<=230; index++)
                {
                    var jsonData = await System.IO.File.ReadAllTextAsync($"./Data/Secret/Quizz/{index:D3}/openquizzdb_{index}.json");

                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        ContractResolver = new PrivateSetterContractResolver()
                    };

                    if (jsonData.Contains("\"fr\""))
                    {
                        QuizModelB quizB = JsonConvert.DeserializeObject<QuizModelB>(jsonData, settings);
                        quiz = quizB;
                    }
                    else 
                    {
                        QuizModelA quizA = JsonConvert.DeserializeObject<QuizModelA>(jsonData, settings);
                        quiz = quizA;
                    }

                    _dbContext.QuizTopics.Add(quiz);
                    cptr++;
                }

                if (_dbContext.ChangeTracker.HasChanges())
                    _dbContext.SaveChanges();

                return cptr;
            }
            catch
            {
                throw new ApplicationException("CreateQuizzesFromJsonOfOpenQuizzDB");
            }
        }
    }
}
