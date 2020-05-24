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
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IAvatarRepository _avatarRepository;

        public ProfileController(IProfileRepository profileRepository, IAvatarRepository avatarRepository)
        {
            _profileRepository = profileRepository;
            _avatarRepository = avatarRepository;
        }

        [HttpGet]
        [Route("getprofiles")]
        public async Task<ActionResult<PaginatedList<Profile>>> Get(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue)
        {
            var list = await _profileRepository.GetProfiles(pageNumber, sortField, sortOrder, filterField, filterValue);
            return list;
        }

        [HttpGet("getallprofiles")]
        public async Task<ActionResult<List<Profile>>> GetAllProfiles()
        {
            var list = await _profileRepository.GetAllProfiles();
            return list;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetProfile(int id)
        {
            return Ok(_profileRepository.GetProfile(id));
        }

        [HttpGet]
        [Route("getProfileByUserId")]
        public IActionResult GetProfileByUserId(string userId)
        {
            var profile = _profileRepository.GetProfileByUserId(userId);
            return Ok(profile);
        }

        [HttpPost]
        [Authorize(Policy = Policies.IsAdmin)]
        public async Task<IActionResult> CreateProfile([FromBody] Profile profile)
        {
            if (profile == null)
                return BadRequest();

            if (profile.User.FirstName == string.Empty || profile.User.LastName == string.Empty)
            {
                ModelState.AddModelError("Firstname/Lastname", "The firstname or lastname shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdProfile = await _profileRepository.AddProfile(profile);

            return Created("profile", createdProfile);
        }

        [HttpPut]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult UpdateProfile([FromBody] Profile profile)
        {
            if (profile == null)
                return BadRequest();

            if (profile.User.FirstName == string.Empty || profile.User.LastName == string.Empty)
            {
                ModelState.AddModelError("Firstname/Lastname", "The firstname or lastname shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var profileToUpdate = _profileRepository.GetProfile(profile.Id);

            if (profileToUpdate == null)
                return NotFound();

            _profileRepository.UpdateProfile(profile);

            return NoContent(); //success
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult DeleteProfile(int id)
        {
            var profileToDelete = _profileRepository.GetProfile(id);
            if (profileToDelete == null)
                return NotFound();

            _profileRepository.DeleteProfile(id);

            return NoContent();//success
        }

        [HttpGet("getFakeProfiles")]
        public async Task<FakeProfileModel[]> GetFakeProfiles()
        {
            return await _profileRepository.GetFakeProfiles();
        }

        [HttpPost("createFakeProfiles")]
        public async Task<IActionResult> CreateFakeProfiles([FromBody] List<FakeProfileModel> users)
        {
            try
            {
                var cptrSucceed = 0;
                var cptrFailed = 0;

                foreach (var user in users)
                {
                    var email = user.FirstName + "." + user.LastName + "@ululu.com";
                    var found = _profileRepository.GetProfileByUserName(email) != null;
                    if (found) continue;

                    var avatarId = await _avatarRepository.CreateAvatarFromUrl(user.PhotoUrl);

                    var profile = new Profile
                    {
                        User = new CustomUser
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = email,
                            AvatarId = avatarId
                        }
                    };
                    var createdProfile = await _profileRepository.AddProfile(profile);

                    if (createdProfile != null)
                        cptrSucceed++;
                    else
                        cptrFailed++;
                }

                return Ok(new CreateFakeProfilesResult { CptrSucceed = cptrSucceed, CptrFailed = cptrFailed });
            }
            catch
            {
                throw new ApplicationException();
            }
            
        }
    }
}
