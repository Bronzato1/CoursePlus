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
        Task<PaginatedList<QuizTopic>> GetQuizzes(int pageNumber = 1, IDictionary<string, string> sortOrder = null, IDictionary<string, string> filters = null);

        Task<List<QuizTopic>> GetPopularQuizzes();

        Task<QuizTopic> GetQuiz(int id);

        Task<QuizTopic> AddQuiz(QuizTopic quiz);

        Task UpdateQuiz(QuizTopic quiz);

        Task DeleteQuiz(int id);

        Task<int> CreateQuizzesFromJsonOfOpenQuizzDB();
    }
}
