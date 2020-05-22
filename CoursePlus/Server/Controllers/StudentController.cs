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
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAvatarRepository _avatarRepository;

        public StudentController(IStudentRepository studentRepository, IAvatarRepository avatarRepository)
        {
            _studentRepository = studentRepository;
            _avatarRepository = avatarRepository;
        }

        [HttpGet]
        [Route("getstudents")]
        public async Task<ActionResult<PaginatedList<Student>>> Get(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue)
        {
            var list = await _studentRepository.GetStudents(pageNumber, sortField, sortOrder, filterField, filterValue);
            return list;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetStudent(int id)
        {
            return Ok(_studentRepository.GetStudent(id));
        }

        [HttpGet]
        [Route("getStudentByUserId")]
        public IActionResult GetStudentByUserId(string userId)
        {
            var student = _studentRepository.GetStudentByUserId(userId);
            return Ok(student);
        }

        [HttpPost]
        [Authorize(Policy = Policies.IsAdmin)]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            if (student == null)
                return BadRequest();

            if (student.User.FirstName == string.Empty || student.User.LastName == string.Empty)
            {
                ModelState.AddModelError("Firstname/Lastname", "The firstname or lastname shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdStudent = await _studentRepository.AddStudent(student);

            return Created("student", createdStudent);
        }

        [HttpPut]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult UpdateStudent([FromBody] Student student)
        {
            if (student == null)
                return BadRequest();

            if (student.User.FirstName == string.Empty || student.User.LastName == string.Empty)
            {
                ModelState.AddModelError("Firstname/Lastname", "The firstname or lastname shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var studentToUpdate = _studentRepository.GetStudent(student.Id);

            if (studentToUpdate == null)
                return NotFound();

            _studentRepository.UpdateStudent(student);

            return NoContent(); //success
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult DeleteStudent(int id)
        {
            var studentToDelete = _studentRepository.GetStudent(id);
            if (studentToDelete == null)
                return NotFound();

            _studentRepository.DeleteStudent(id);

            return NoContent();//success
        }

        [HttpGet("getFakeStudents")]
        public async Task<FakeStudentModel[]> GetFakeStudents()
        {
            return await _studentRepository.GetFakeStudents();
        }

        [HttpPost("createFakeStudents")]
        public async Task<IActionResult> CreateFakeStudents([FromBody] List<FakeStudentModel> users)
        {
            var cptrSucceed = 0;
            var cptrFailed = 0;

            foreach (var user in users)
            {
                var avatarId = await _avatarRepository.CreateAvatarFromUrl(user.PhotoUrl);
                
                var student = new Student
                {
                    User = new CustomUser
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.FirstName + "." + user.LastName + "@ululu.com",
                        AvatarId = avatarId
                    }
                };
                var createdStudent = await _studentRepository.AddStudent(student);

                if (createdStudent != null)
                    cptrSucceed++;
                else
                    cptrFailed++;
            }

            return Ok(new CreateFakeStudentsResult { CptrSucceed = cptrSucceed, CptrFailed = cptrFailed });
        }
    }
}
