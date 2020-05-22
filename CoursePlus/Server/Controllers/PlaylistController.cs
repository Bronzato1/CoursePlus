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
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistController(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        [HttpGet("getplaylists")]
        public async Task<ActionResult<PaginatedList<Playlist>>> GetPlaylists(int? pageNumber, string sortOrder, string filters)
        {
            var currentSortOrder = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, string>>(sortOrder);
            var currentFilters = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, string>>(filters);
            var list = await _playlistRepository.GetPlaylists(pageNumber, currentSortOrder, currentFilters);
            return list;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetPlaylist(int id)
        {
            var data = _playlistRepository.GetPlaylist(id);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data, new Newtonsoft.Json.JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            return Ok(json);
        }

        [HttpPost]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult CreatePlaylist([FromBody] Playlist playlist)
        {
            if (playlist == null)
                return BadRequest();

            if (playlist.Title == string.Empty || playlist.Description == string.Empty)
            {
                ModelState.AddModelError("Title/Description", "The title or description shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPlaylist = _playlistRepository.AddPlaylist(playlist);

            return Created("playlist", createdPlaylist);
        }

        [HttpPut]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult UpdatePlaylist([FromBody] Playlist playlist)
        {
            if (playlist == null)
                return BadRequest();

            if (playlist.Title == string.Empty || playlist.Description == string.Empty)
            {
                ModelState.AddModelError("Title/Description", "The title or description shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var playlistToUpdate = _playlistRepository.GetPlaylist(playlist.Id);

            if (playlistToUpdate == null)
                return NotFound();

            _playlistRepository.UpdatePlaylist(playlist);

            return NoContent(); //success
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult DeletePlaylist(int id)
        {
            if (id == 0)
                return BadRequest();

            var playlistToDelete = _playlistRepository.GetPlaylist(id);
            if (playlistToDelete == null)
                return NotFound();

            _playlistRepository.DeletePlaylist(id);

            return NoContent();//success
        }
    }
}
