using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public interface IProfileRepository
    {
        Task<PaginatedList<Profile>> GetProfiles(int? pageNumber, string sortField, string sortOrder, string filterField, string filterValue);

        Task<List<Profile>> GetAllProfiles();

        Profile GetProfile(int id);

        Profile GetProfileByUserId(string profileId);

        Profile GetProfileByUserName(string userName);

        Task<Profile> AddProfile(Profile profile);

        Profile UpdateProfile(Profile profile);

        void DeleteProfile(int id);

        Task<FakeProfileModel[]> GetFakeProfiles();
    }
}
