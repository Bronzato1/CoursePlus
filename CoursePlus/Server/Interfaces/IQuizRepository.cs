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
        Task<PaginatedList<QuizTopic>> GetQuizzes(int? pageNumber, IDictionary<string, string> sortOrder, IDictionary<string, string> filters);

        Task<List<QuizTopic>> GetPopularQuizzes();
         
        QuizTopic GetQuiz(int id);

        QuizTopic AddQuiz(QuizTopic quiz);

        QuizTopic UpdateQuiz(QuizTopic quiz);

        void DeleteQuiz(int id);

        Task<int> CreateQuizzesFromJsonOfOpenQuizzDB();
    }
}
