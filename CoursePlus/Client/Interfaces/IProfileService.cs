using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public interface IProfileService
    {
        Task<PaginatedList<Profile>> GetProfiles(int pageNumber = 1, string sortField = "", string sortOrder = "", string filterField = "", string filterValue = "");

        Task<Profile> GetProfile(int id);

        Task<Profile> GetProfileByUserId(string userId);

        Task<Profile> AddProfile(Profile profile);

        Task UpdateProfile(Profile profile);

        Task DeleteProfile(int id);

        Task<FakeProfileModel[]> GetFakeProfiles();

        Task<CreateFakeProfilesResult> CreateFakeProfiles(List<FakeProfileModel> users);
    }
}
