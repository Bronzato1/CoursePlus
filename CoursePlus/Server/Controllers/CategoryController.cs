using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursePlus.Server.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoursePlus.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        // GET: api/<controller>
        [HttpGet]
        public IActionResult GetCategories()
        {
            return Ok(_categoryRepository.GetAllCategories());
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            return Ok(_categoryRepository.GetCategoryById(id));
        }
    }
}
