using CoursePlus.Server.Data;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public class CourseRepository :ICourseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CourseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<Course>> GetList(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue)
        {
            try
            {
                int pageSize = 5;
                var courseList = _dbContext.Courses
                                         .Include(x => x.Thumbnail)
                                         .Include(x => x.Category)
                                         .WhereDynamic(filterField, filterValue)
                                         .OrderByDynamic(sortField, sortOrder);

                return await PaginatedList<Course>.CreateAsync(courseList.AsNoTracking(), pageNumber ?? 1, pageSize);
            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }
        }

        public Course GetCourse(int id)
        {
            var course = _dbContext.Courses
                .Where(x => x.Id == id)
                .Include(x => x.Image)
                .Include(x => x.Category)
                .FirstOrDefault();
            return course;
        }

        public Course AddCourse(Course course)
        {
            var addedEntity = _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public Course UpdateCourse(Course course)
        {
            var foundCourse = _dbContext.Courses.FirstOrDefault(e => e.Id == course.Id);

            if (foundCourse != null)
            {
                foundCourse.Title = course.Title;
                foundCourse.Description = course.Description;
                foundCourse.ImageId = course.ImageId;
                foundCourse.ThumbnailId = course.ThumbnailId;
                foundCourse.Language = course.Language;

                //foundCourse.InstructorId = course.InstructorId; <<<<<<<<<<---------

                _dbContext.SaveChanges();

                return foundCourse;
            }

            return null;
        }

        public void DeleteCourse(int id)
        {
            var foundCourse = _dbContext.Courses.FirstOrDefault(e => e.Id == id);
            if (foundCourse == null) return;

            _dbContext.Courses.Remove(foundCourse);
            _dbContext.SaveChanges();
        }
    }
}
