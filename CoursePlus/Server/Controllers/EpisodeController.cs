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
    public class EpisodeController : ControllerBase
    {
        private readonly IEpisodeRepository _episodeRepository;

        public EpisodeController(IEpisodeRepository episodeRepository)
        {
            _episodeRepository = episodeRepository;
        }

        [HttpGet("getepisodes")]
        public async Task<ActionResult<List<Episode>>> GetEpisodes()
        {
            var list = await _episodeRepository.GetEpisodes();
            return list;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetEpisode(int id)
        {
            return Ok(_episodeRepository.GetEpisode(id));
        }

        [HttpPost]
        public IActionResult CreateEpisode([FromBody] Episode episode)
        {
            if (episode == null)
                return BadRequest();

            if (episode.Title == string.Empty)
            {
                ModelState.AddModelError("Title", "The title shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEpisode = _episodeRepository.AddEpisode(episode);

            return Created("episode", createdEpisode);
        }

        [HttpPut]
        public IActionResult UpdateEpisode([FromBody] Episode episode)
        {
            if (episode == null)
                return BadRequest();

            if (episode.Title == string.Empty)
            {
                ModelState.AddModelError("Title", "The title shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var episodeToUpdate = _episodeRepository.GetEpisode(episode.Id);

            if (episodeToUpdate == null)
                return NotFound();

            _episodeRepository.UpdateEpisode(episode);

            return NoContent(); //success
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteEpisode(int id)
        {
            if (id == 0)
                return BadRequest();

            var episodeToDelete = _episodeRepository.GetEpisode(id);
            if (episodeToDelete == null)
                return NotFound();

            _episodeRepository.DeleteEpisode(id);

            return NoContent();//success
        }
    }
}
