﻿using CoursePlus.Server.Data;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly HttpClient _httpClient;
        
        public StudentRepository(ApplicationDbContext dbContext, UserManager<CustomUser> userManager, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpClient = httpClient;
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

        public async Task<Student> AddStudent(Student student)
        {
            try
            {
                var newUser = new CustomUser { FirstName = student.User.FirstName, LastName = student.User.LastName, UserName = student.User.Email, Email = student.User.Email, AvatarId = student.User.AvatarId };
                var result = await _userManager.CreateAsync(newUser, "Pa$$w0rd");

                if (!result.Succeeded)
                {
                    throw new ApplicationException();
                }

                await _userManager.AddToRoleAsync(newUser, "User");

                student.User = newUser;

                var addedEntity = _dbContext.Students.Add(student);

                await _dbContext.SaveChangesAsync();

                return addedEntity.Entity;
            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }
            
        }

        public Student UpdateStudent(Student student)
        {
            var foundStudent = _dbContext.Students.FirstOrDefault(e => e.Id == student.Id);
            var foundUser = _dbContext.Users.FirstOrDefault(e => e.Id == student.UserId);

            if (foundStudent != null)
            {
                foundStudent.UserId = student.UserId;
                foundStudent.Joined = student.Joined;

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

        public async Task<FakeStudentModel[]> GetFakeStudents()
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "get_random_users?ps=10&av=f&is=400");

                //rm.SetBrowserRequestMode(BrowserRequestMode.NoCors);
                //rm.SetBrowserRequestCache(BrowserRequestCache.NoCache);

                var response = await _httpClient.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();

                var parsedUsers = JsonSerializer.Deserialize<FakeStudentModel[]>(result.ToString(), new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true,
                });

                var index = 0;

                foreach (var elm in parsedUsers)
                {
                    elm.Index = index;
                    index++;
                }

                return parsedUsers;
            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }
        }
    }
}