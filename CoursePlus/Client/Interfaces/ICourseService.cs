using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public interface ICourseService
    {
        Task<PaginatedList<Course>> GetCourses(int pageNumber = 1, string sortField = "", string sortOrder = "", string filterField = "", string filterValue = "");

        Task<Course> GetCourse(int id);

        Task<Course> AddCourse(Course course);

        Task UpdateCourse(Course course);

        Task DeleteCourse(int id);
    }
}
