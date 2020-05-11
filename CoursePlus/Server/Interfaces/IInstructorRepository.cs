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
        Task<PaginatedList<Instructor>> GetList(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue);

        public Instructor GetInstructor(int id);

        public Instructor AddInstructor(Instructor instructor);

        public Instructor UpdateInstructor(Instructor instructor);

        public void DeleteInstructor(int id);
    }
}
