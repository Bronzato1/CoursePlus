using CoursePlus.Server.Data;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using JsonNet.PrivateSettersContractResolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProfileRepository _profileRepository;

        public QuizRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IProfileRepository profileRepository)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _profileRepository = profileRepository;
        }

        public async Task<PaginatedList<QuizTopic>> GetQuizzes(int? pageNumber, int? pageSize, IDictionary<string, string> sortOrder, IDictionary<string, string> filters)
        {
            try
            {
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

                return await PaginatedList<QuizTopic>.CreateAsync(quizList.AsNoTracking(), pageNumber ?? 1, pageSize ?? 6);
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
                                   .Take(6)
                                   .ToListAsync();
        }

        public QuizTopic GetQuiz(int id)
        {
            var quiz = _dbContext.QuizTopics
                .Where(x => x.Id == id)
                .Include(x => x.Image)
                .Include(x => x.Category)
                .Include(x => x.Owner.User)
                .Include(x => x.Items).ThenInclude(x => x.Proposals)
                .Include(x => x.Chapters).ThenInclude(x => x.Episodes).ThenInclude(x => x.WatchHistory)
                .Include(x => x.Enrollments).ThenInclude(x => x.Profile)
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
                foundQuiz.Featured = quiz.Featured;
                foundQuiz.Popular = quiz.Popular;

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
                    Debug.WriteLine($"index: {index}");
                    var jsonFilename  = System.IO.Directory.GetFiles($"./Data/Secret/Quizz/{index:D3}", $"openquizzdb_{index}.json");
                    var thumbFilename = System.IO.Directory.GetFiles($"./Data/Secret/Quizz/{index:D3}", $"Thumbnail_{index}.jpg");
                    var imageFilename = System.IO.Directory.GetFiles($"./Data/Secret/Quizz/{index:D3}", $"Image_{index}.jpg");

                    if (jsonFilename.Length  != 1) continue;
                    if (thumbFilename.Length != 1) continue;
                    if (imageFilename.Length != 1) continue;

                    var json = await System.IO.File.ReadAllTextAsync(jsonFilename[0]);
                    var thumb = await System.IO.File.ReadAllBytesAsync(thumbFilename[0]);
                    var image = await System.IO.File.ReadAllBytesAsync(imageFilename[0]);

                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        ContractResolver = new PrivateSetterContractResolver()
                    };

                    if (json.Contains("\"fr\""))
                    {
                        QuizModelB quizB = JsonConvert.DeserializeObject<QuizModelB>(json, settings);
                        quiz = quizB;
                        quiz.Category = new CategoryRepository(_dbContext).GetCategoryByName(quizB.Catégorie);
                    }
                    else
                    {
                        QuizModelA quizA = JsonConvert.DeserializeObject<QuizModelA>(json, settings);
                        quiz = quizA;
                        quiz.Category = new CategoryRepository(_dbContext).GetCategoryByName(quizA.Catégorie);
                    }

                    var email = _httpContextAccessor.HttpContext.User.Identity.Name;
                    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    quiz.Owner = _profileRepository.GetProfileByUserId(userId);

                    quiz.Description = "<i>to be filled later</i>";

                    quiz.Thumbnail = new Thumbnail { Data = thumb };
                    _dbContext.Thumbnails.Add(quiz.Thumbnail);

                    quiz.Image = new Image { Data = image };
                    _dbContext.Images.Add(quiz.Image);

                    _dbContext.QuizTopics.Add(quiz);

                    if (_dbContext.ChangeTracker.HasChanges())
                        _dbContext.SaveChanges();

                    cptr++;
                }

                

                return cptr;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("CreateQuizzesFromJsonOfOpenQuizzDB");
            }
        }
    }
}
