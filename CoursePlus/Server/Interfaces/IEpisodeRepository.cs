using CoursePlus.Server.Controllers;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public interface IEpisodeRepository
    {
        Task<List<Episode>> GetList();

        public Episode GetEpisode(int id);

        public Episode AddEpisode(Episode episode);

        public Episode UpdateEpisode(Episode episode);

        public void DeleteEpisode(int id);
    }
}
