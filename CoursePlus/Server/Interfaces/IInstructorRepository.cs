using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public interface IInstructorRepository
    {
        Task<PaginatedList<Instructor>> GetInstructors(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue);

        Task<List<Instructor>> GetAllInstructors();

        Instructor GetInstructor(int id);

        Task<Instructor> AddInstructor(Instructor instructor);

        Instructor UpdateInstructor(Instructor instructor);

        void DeleteInstructor(int id);
    }
}
