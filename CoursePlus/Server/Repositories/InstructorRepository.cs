using CoursePlus.Server.Data;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly UserManager<CustomUser> _userManager;

        public InstructorRepository(ApplicationDbContext dbContext, UserManager<CustomUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<PaginatedList<Instructor>> GetInstructors(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue)
        {
            try
            {
                int pageSize = 5;
                var instructorList = _dbContext.Instructors
                                               .Include(x => x.User.Avatar)
                                               .WhereDynamic(filterField, filterValue)
                                               .OrderByDynamic(sortField, sortOrder);

                return await PaginatedList<Instructor>.CreateAsync(instructorList.AsNoTracking(), pageNumber ?? 1, pageSize);
            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }
        }

        public async Task<List<Instructor>> GetAllInstructors()
        {
            try
            {
                var instructorList = _dbContext.Instructors
                                               .Include(x => x.User)
                                               .OrderBy(x => x.User.FirstName);

                return await instructorList.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }
        }

        public Instructor GetInstructor(int id)
        {
            var instructor = _dbContext.Instructors
                                       .Include(x => x.User)
                                       .Include(x => x.User.Avatar)
                                       .Where(x => x.Id == id)
                                       .FirstOrDefault();

            return instructor;
        }

        public Instructor AddInstructor(Instructor instructor)
        {
            var newUser = new CustomUser { FirstName = instructor.User.FirstName, LastName = instructor.User.LastName, UserName = instructor.User.Email, Email = instructor.User.Email, AvatarId = instructor.User.AvatarId };
            var result = _userManager.CreateAsync(newUser, "Pa$$w0rd").Result;

            if (!result.Succeeded)
            {
                throw new ApplicationException();
            }

            _userManager.AddToRoleAsync(newUser, "User");

            instructor.User = newUser;

            var addedEntity = _dbContext.Instructors.Add(instructor);

            _dbContext.SaveChangesAsync();

            return addedEntity.Entity;
        }

        public Instructor UpdateInstructor(Instructor instructor)
        {
            var foundInstructor = _dbContext.Instructors.FirstOrDefault(e => e.Id == instructor.Id);
            var foundUser = _dbContext.Users.FirstOrDefault(e => e.Id == instructor.UserId);

            if (foundInstructor != null)
            {
                foundInstructor.UserId = instructor.UserId;
                foundInstructor.Joined = instructor.Joined;

                if (foundUser != null)
                {
                    foundUser.FirstName = instructor.User.FirstName;
                    foundUser.LastName = instructor.User.LastName;
                    foundUser.AvatarId = instructor.User.AvatarId;
                }

                _dbContext.SaveChanges();

                return foundInstructor;
            }

            return null;
        }

        public void DeleteInstructor(int id)
        {
            var foundInstructor = _dbContext.Instructors.FirstOrDefault(e => e.Id == id);
            if (foundInstructor == null) return;

            var foundAvatar = foundInstructor.User.Avatar;

            var result = _userManager.DeleteAsync(foundInstructor.User).Result;

            if (!result.Succeeded)
            {
                throw new ApplicationException();
            }

            if (foundAvatar != null)
            {
                _dbContext.Avatars.Remove(foundAvatar);
            }

            _dbContext.Instructors.Remove(foundInstructor);
            _dbContext.SaveChanges();
        }
    }
}
