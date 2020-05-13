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
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorRepository _instructorRepository;

        public InstructorController(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        [HttpGet("api/instructors/getinstructors")]
        public async Task<ActionResult<PaginatedList<Instructor>>> GetInstructors(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue)
        {
            var list = await _instructorRepository.GetInstructors(pageNumber, sortField, sortOrder, filterField, filterValue);
            return list;
        }

        [HttpGet("api/instructors/getallinstructors")]
        public async Task<ActionResult<List<Instructor>>> GetAllInstructors()
        {
            var list = await _instructorRepository.GetAllInstructors();
            return list;
        }

        [HttpGet("api/instructor/{id:int}")]
        public IActionResult GetInstructor(int id)
        {
            return Ok(_instructorRepository.GetInstructor(id));
        }

        [HttpPost("api/instructor")]
        public IActionResult CreateInstrutor([FromBody] Instructor instructor)
        {
            if (instructor == null)
                return BadRequest();

            if (instructor.User.FirstName == string.Empty || instructor.User.LastName == string.Empty)
            {
                ModelState.AddModelError("Firstname/Lastname", "The firstname or lastname shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdInstructor = _instructorRepository.AddInstructor(instructor);

            return Created("instructor", createdInstructor);
        }

        [HttpPut("api/instructor")]
        public IActionResult UpdateInstructor([FromBody] Instructor instructor)
        {
            if (instructor == null)
                return BadRequest();

            if (instructor.User.FirstName == string.Empty || instructor.User.LastName == string.Empty)
            {
                ModelState.AddModelError("Firstname/Lastname", "The firstname or lastname shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var instructorToUpdate = _instructorRepository.GetInstructor(instructor.Id);

            if (instructorToUpdate == null)
                return NotFound();

            _instructorRepository.UpdateInstructor(instructor);

            return NoContent(); //success
        }

        [HttpDelete("api/instructor/{id}")]
        public IActionResult DeleteInstructor(int id)
        {
            if (id == null)
                return BadRequest();

            var instructorToDelete = _instructorRepository.GetInstructor(id);
            if (instructorToDelete == null)
                return NotFound();

            _instructorRepository.DeleteInstructor(id);

            return NoContent();//success
        }
    }
}
