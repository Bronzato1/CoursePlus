using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public interface IQuizService
    {
        Task<PaginatedList<Quiz>> GetQuizzes(int pageNumber = 1, IDictionary<string, string> sortOrder = null, IDictionary<string, string> filters = null);

        Task<Quiz> GetQuiz(int id);

        Task<Quiz> AddQuiz(Quiz quiz);

        Task UpdateQuiz(Quiz quiz);

        Task DeleteQuiz(int id);
    }
}
