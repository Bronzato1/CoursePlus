using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Server.Repositories;
using CoursePlus.Shared.Models;
using CoursePlus.Shared.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoursePlus.Server.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;

        public QuizController(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        [HttpGet("getquizzes")]
        public async Task<ActionResult<PaginatedList<Quiz>>> GetQuizzes(int? pageNumber, string sortOrder, string filters)
        {
            var currentSortOrder = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, string>>(sortOrder);
            var currentFilters = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, string>>(filters);
            var list = await _quizRepository.GetQuizzes(pageNumber, currentSortOrder, currentFilters);
            return list;
        }

        [HttpGet("getpopularquizzes")]
        public async Task<ActionResult<List<Quiz>>> GetPopularQuizzes()
        {
            return await _quizRepository.GetPopularQuizzes();
        }

        [HttpGet("{id:int}")]
        public IActionResult GetQuiz(int id)
        {
            var data = _quizRepository.GetQuiz(id);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data, new Newtonsoft.Json.JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            return Ok(json);
        }

        [HttpPost]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult CreateQuiz([FromBody] Quiz quiz)
        {
            if (quiz == null)
                return BadRequest();

            if (quiz.Title == string.Empty || quiz.Description == string.Empty)
            {
                ModelState.AddModelError("Title/Description", "The title or description shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdQuiz = _quizRepository.AddQuiz(quiz);

            return Created("quiz", createdQuiz);
        }

        [HttpPut]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult UpdateQuiz([FromBody] Quiz quiz)
        {
            if (quiz == null)
                return BadRequest();

            if (quiz.Title == string.Empty || quiz.Description == string.Empty)
            {
                ModelState.AddModelError("Title/Description", "The title or description shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var quizToUpdate = _quizRepository.GetQuiz(quiz.Id);

            if (quizToUpdate == null)
                return NotFound();

            _quizRepository.UpdateQuiz(quiz);

            return NoContent(); //success
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult DeleteQuiz(int id)
        {
            if (id == 0)
                return BadRequest();

            var quizToDelete = _quizRepository.GetQuiz(id);
            if (quizToDelete == null)
                return NotFound();

            _quizRepository.DeleteQuiz(id);

            return NoContent();//success
        }

        [HttpGet("generateImagesAndThumbnailsFromPath")]
        public async Task<string> GenerateImagesAndThumbnailsFromPath()
        {
            var success = await _quizRepository.GenerateImagesAndThumbnailsFromPath();

            if (success)
                return "Operation finished successfully";
            else
                return "Operation failed";
        }
    }
}
