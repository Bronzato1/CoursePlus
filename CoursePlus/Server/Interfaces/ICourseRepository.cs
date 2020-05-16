using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public interface ICourseRepository
    {
        Task<PaginatedList<Course>> GetCourses(int? pageNumber, IDictionary<string, string> sortOrder, IDictionary<string, string> filters);

        public Course GetCourse(int id);

        public Course AddCourse(Course course);

        public Course UpdateCourse(Course course);

        public void DeleteCourse(int id);
    }
}
