using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public interface IStudentService
    {
        Task<PaginatedList<Student>> GetStudents(int pageNumber = 1, string sortField = "", string sortOrder = "", string filterField = "", string filterValue = "");

        Task<Student> GetStudent(int id);

        Task<Student> AddStudent(Student student);

        Task UpdateStudent(Student student);

        Task DeleteStudent(int id);
    }
}
