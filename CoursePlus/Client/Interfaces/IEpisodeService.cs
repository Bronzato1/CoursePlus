using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public interface IEpisodeService
    {
        Task<List<Episode>> GetEpisodes();

        Task<Episode> GetEpisode(int id);

        Task<Episode> AddEpisode(Episode episode);

        Task UpdateEpisode(Episode episode);

        Task DeleteEpisode(int id);
    }
}
