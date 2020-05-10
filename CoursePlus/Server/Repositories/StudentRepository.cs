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
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly UserManager<CustomUser> _userManager;

        public StudentRepository(ApplicationDbContext dbContext, UserManager<CustomUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<PaginatedList<Student>> GetList(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue)
        {
            try
            {
                int pageSize = 5;
                var studentList = _dbContext.Students
                                               .Include(x => x.User.Avatar)
                                               .WhereDynamic(filterField, filterValue)
                                               .OrderByDynamic(sortField, sortOrder);

                return await PaginatedList<Student>.CreateAsync(studentList.AsNoTracking(), pageNumber ?? 1, pageSize);
            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }
        }

        public Student GetStudent(int id)
        {
            var student = _dbContext.Students
                                       .Include(x => x.User)
                                       .Include(x => x.User.Avatar)
                                       .Where(x => x.Id == id)
                                       .FirstOrDefault();

            return student;
        }

        public Student AddStudent(Student student)
        {
            var newUser = new CustomUser { FirstName = student.User.FirstName, LastName = student.User.LastName, UserName = student.User.Email, Email = student.User.Email, AvatarId = student.User.AvatarId };
            var result = _userManager.CreateAsync(newUser, "Pa$$w0rd").Result;

            if (!result.Succeeded)
            {
                throw new ApplicationException();
            }

            _userManager.AddToRoleAsync(newUser, "User");

            student.User = newUser;

            var addedEntity = _dbContext.Students.Add(student);

            _dbContext.SaveChangesAsync();

            return addedEntity.Entity;
        }

        public Student UpdateStudent(Student student)
        {
            var foundStudent = _dbContext.Students.FirstOrDefault(e => e.Id == student.Id);
            var foundUser = _dbContext.Users.FirstOrDefault(e => e.Id == student.UserId);

            if (foundStudent != null)
            {
                foundStudent.UserId = student.UserId;

                if (foundUser != null)
                {
                    foundUser.FirstName = student.User.FirstName;
                    foundUser.LastName = student.User.LastName;
                    foundUser.AvatarId = student.User.AvatarId;
                }

                _dbContext.SaveChanges();

                return foundStudent;
            }

            return null;
        }

        public void DeleteStudent(int id)
        {
            var foundStudent = _dbContext.Students.FirstOrDefault(e => e.Id == id);
            if (foundStudent == null) return;

            var foundAvatar = foundStudent.User.Avatar;

            var result = _userManager.DeleteAsync(foundStudent.User).Result;

            if (!result.Succeeded)
            {
                throw new ApplicationException();
            }

            if (foundAvatar != null)
            {
                _dbContext.Avatars.Remove(foundAvatar);
            }

            _dbContext.Students.Remove(foundStudent);
            _dbContext.SaveChanges();
        }
    }
}
