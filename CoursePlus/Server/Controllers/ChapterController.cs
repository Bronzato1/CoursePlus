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
    public class ChapterController : ControllerBase
    {
        private readonly IChapterRepository _chapterRepository;

        public ChapterController(IChapterRepository chapterRepository)
        {
            _chapterRepository = chapterRepository;
        }

        [HttpGet("getchapters")]
        public async Task<ActionResult<List<Chapter>>> GetChapters()
        {
            var list = await _chapterRepository.GetChapters();
            return list;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetChapter(int id)
        {
            return Ok(_chapterRepository.GetChapter(id));
        }

        [HttpPost]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult CreateChapter([FromBody] Chapter chapter)
        {
            if (chapter == null)
                return BadRequest();

            if (chapter.Title == string.Empty)
            {
                ModelState.AddModelError("Title", "The title shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdChapter = _chapterRepository.AddChapter(chapter);

            return Created("chapter", createdChapter);
        }

        [HttpPut]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult UpdateChapter([FromBody] Chapter chapter)
        {
            if (chapter == null)
                return BadRequest();

            if (chapter.Title == string.Empty)
            {
                ModelState.AddModelError("Title", "The title shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var chapterToUpdate = _chapterRepository.GetChapter(chapter.Id);

            if (chapterToUpdate == null)
                return NotFound();

            _chapterRepository.UpdateChapter(chapter);

            return NoContent(); //success
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult DeleteChapter(int id)
        {
            if (id == 0)
                return BadRequest();

            var chapterToDelete = _chapterRepository.GetChapter(id);
            if (chapterToDelete == null)
                return NotFound();

            _chapterRepository.DeleteChapter(id);

            return NoContent();//success
        }
    }
}
