using CoursePlus.Server.Data;
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
    public class ProfileRepository : IProfileRepository
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly HttpClient _httpClient;
        
        public ProfileRepository(ApplicationDbContext dbContext, UserManager<CustomUser> userManager, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpClient = httpClient;
        }

        public async Task<PaginatedList<Profile>> GetProfiles(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue)
        {
            try
            {
                int pageSize = 5;
                var profileList = _dbContext.Profiles
                                               .Include(x => x.User.Avatar)
                                               .WhereDynamic(filterField, filterValue)
                                               .OrderByDynamic(sortField, sortOrder);

                return await PaginatedList<Profile>.CreateAsync(profileList.AsNoTracking(), pageNumber ?? 1, pageSize);
            }
            catch
            {
                throw new ApplicationException();
            }
        }

        public async Task<List<Profile>> GetAllProfiles()
        {
            try
            {
                var profileList = _dbContext.Profiles
                                               .Include(x => x.User)
                                               .OrderBy(x => x.User.FirstName);

                return await profileList.ToListAsync();
            }
            catch
            {
                throw new ApplicationException();
            }
        }


        public Profile GetProfile(int id)
        {
            var profile = _dbContext.Profiles
                                       .Include(x => x.User)
                                       .Include(x => x.User.Avatar)
                                       .Where(x => x.Id == id)
                                       .FirstOrDefault();

            return profile;
        }

        public Profile GetProfileByUserId(string userId)
        {
            var profile = _dbContext.Profiles
                                       .Include(x => x.User)
                                       .Include(x => x.User.Avatar)
                                       .Where(x => x.UserId == userId)
                                       .FirstOrDefault();

            return profile;
        }

        public Profile GetProfileByUserName(string userName)
        {
            var profile = _dbContext.Profiles
                                       .Include(x => x.User)
                                       .Include(x => x.User.Avatar)
                                       .Where(x => x.User.UserName == userName)
                                       .FirstOrDefault();

            return profile;
        }

        public async Task<Profile> AddProfile(Profile profile)
        {
            try
            {
                var newUser = new CustomUser { FirstName = profile.User.FirstName, LastName = profile.User.LastName, UserName = profile.User.Email, Email = profile.User.Email, AvatarId = profile.User.AvatarId };
                var result = await _userManager.CreateAsync(newUser, "Pa$$w0rd");

                if (!result.Succeeded)
                {
                    throw new ApplicationException();
                }

                await _userManager.AddToRoleAsync(newUser, "User");

                profile.User = newUser;

                var addedEntity = _dbContext.Profiles.Add(profile);

                await _dbContext.SaveChangesAsync();

                return addedEntity.Entity;
            }
            catch
            {
                throw new ApplicationException();
            }
            
        }

        public Profile UpdateProfile(Profile profile)
        {
            var foundProfile = _dbContext.Profiles.FirstOrDefault(e => e.Id == profile.Id);
            var foundUser = _dbContext.Users.FirstOrDefault(e => e.Id == profile.UserId);

            if (foundProfile != null)
            {
                foundProfile.UserId = profile.UserId;
                foundProfile.Joined = profile.Joined;

                if (foundUser != null)
                {
                    foundUser.FirstName = profile.User.FirstName;
                    foundUser.LastName = profile.User.LastName;
                    foundUser.AvatarId = profile.User.AvatarId;
                }

                _dbContext.SaveChanges();

                return foundProfile;
            }

            return null;
        }

        public void DeleteProfile(int id)
        {
            var foundProfile = _dbContext.Profiles.FirstOrDefault(e => e.Id == id);
            if (foundProfile == null) return;

            var foundAvatar = foundProfile.User.Avatar;

            var result = _userManager.DeleteAsync(foundProfile.User).Result;

            if (!result.Succeeded)
            {
                throw new ApplicationException();
            }

            if (foundAvatar != null)
            {
                _dbContext.Avatars.Remove(foundAvatar);
            }

            _dbContext.Profiles.Remove(foundProfile);
            _dbContext.SaveChanges();
        }

        public async Task<FakeProfileModel[]> GetFakeProfiles()
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "get_random_users?ps=10&av=f&is=400");

                //rm.SetBrowserRequestMode(BrowserRequestMode.NoCors);
                //rm.SetBrowserRequestCache(BrowserRequestCache.NoCache);

                var response = await _httpClient.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();

                var parsedUsers = JsonSerializer.Deserialize<FakeProfileModel[]>(result.ToString(), new JsonSerializerOptions
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
            catch
            {
                throw new ApplicationException();
            }
        }
    }
}
