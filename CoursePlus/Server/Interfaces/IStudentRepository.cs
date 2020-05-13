using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public interface IStudentRepository
    {
        Task<PaginatedList<Student>> GetStudents(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue);

        public Student GetStudent(int id);

        public Task<Student> AddStudent(Student student);

        public Student UpdateStudent(Student student);

        public void DeleteStudent(int id);

        Task<FakeStudentModel[]> GetFakeStudents();
    }
}
