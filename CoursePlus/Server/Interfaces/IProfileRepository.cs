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

        public Profile GetProfile(int id);

        public Profile GetProfileByUserId(string profileId);

        public Task<Profile> AddProfile(Profile profile);

        public Profile UpdateProfile(Profile profile);

        public void DeleteProfile(int id);

        Task<FakeProfileModel[]> GetFakeProfiles();
    }
}
