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
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;

        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        [HttpGet("getcourses")]
        public async Task<ActionResult<PaginatedList<Course>>> GetCourses(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue)
        {
            var list = await _courseRepository.GetCourses(pageNumber, sortField, sortOrder, filterField, filterValue);
            return list;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCourse(int id)
        {
            var data = _courseRepository.GetCourse(id);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data, new Newtonsoft.Json.JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            return Ok(json);
        }

        [HttpPost]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult CreateCourse([FromBody] Course course)
        {
            if (course == null)
                return BadRequest();

            if (course.Title == string.Empty || course.Description == string.Empty)
            {
                ModelState.AddModelError("Title/Description", "The title or description shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCourse = _courseRepository.AddCourse(course);

            return Created("course", createdCourse);
        }

        [HttpPut]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult UpdateCourse([FromBody] Course course)
        {
            if (course == null)
                return BadRequest();

            if (course.Title == string.Empty || course.Description == string.Empty)
            {
                ModelState.AddModelError("Title/Description", "The title or description shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var courseToUpdate = _courseRepository.GetCourse(course.Id);

            if (courseToUpdate == null)
                return NotFound();

            _courseRepository.UpdateCourse(course);

            return NoContent(); //success
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = Policies.IsAdmin)]
        public IActionResult DeleteCourse(int id)
        {
            if (id == 0)
                return BadRequest();

            var courseToDelete = _courseRepository.GetCourse(id);
            if (courseToDelete == null)
                return NotFound();

            _courseRepository.DeleteCourse(id);

            return NoContent();//success
        }
    }
}
