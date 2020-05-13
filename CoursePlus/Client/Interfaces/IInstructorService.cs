using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public interface IInstructorService
    {
        Task<PaginatedList<Instructor>> GetInstructors(int pageNumber = 1, string sortField = "", string sortOrder = "", string filterField = "", string filterValue = "");

        Task<List<Instructor>> GetAllInstructors();

        Task<Instructor> GetInstructor(int id);

        Task<Instructor> AddInstructor(Instructor instructor);

        Task UpdateInstructor(Instructor instructor);

        Task DeleteInstructor(int id);
    }
}
