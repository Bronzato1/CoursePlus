using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public interface IQuizRepository
    {
        Task<PaginatedList<Quiz>> GetQuizzes(int? pageNumber, IDictionary<string, string> sortOrder, IDictionary<string, string> filters);

        public Task<List<Quiz>> GetPopularQuizzes();
         
        public Quiz GetQuiz(int id);

        public Quiz AddQuiz(Quiz quiz);

        public Quiz UpdateQuiz(Quiz quiz);

        public void DeleteQuiz(int id);

        Task<bool> GenerateImagesAndThumbnailsFromPath();
    }
}
