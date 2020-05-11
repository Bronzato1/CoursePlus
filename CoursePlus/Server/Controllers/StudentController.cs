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
        [Route("api/students/get")]
        public async Task<ActionResult<PaginatedList<Student>>> Get(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue)
        {
            var list = await _studentRepository.GetList(pageNumber, sortField, sortOrder, filterField, filterValue);
            return list;
        }

        [HttpGet("api/student/{id:int}")]
        public IActionResult GetStudent(int id)
        {
            return Ok(_studentRepository.GetStudent(id));
        }

        [HttpPost("api/student")]
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

        [HttpPut("api/student")]
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

        [HttpDelete("api/student/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            if (id == null)
                return BadRequest();

            var studentToDelete = _studentRepository.GetStudent(id);
            if (studentToDelete == null)
                return NotFound();

            _studentRepository.DeleteStudent(id);

            return NoContent();//success
        }

        [HttpGet("api/students/getFakeStudents")]
        public async Task<FakeStudentModel[]> GetFakeStudents()
        {
            return await _studentRepository.GetFakeStudents();
        }

        [HttpPost("api/students/createFakeStudents")]
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
