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
    public class EpisodeRepository : IEpisodeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EpisodeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Episode>> GetEpisodes()
        {
            try
            {
                var episodeList = _dbContext.Episodes;
                return await episodeList.ToListAsync();
            }
            catch
            {
                throw new ApplicationException();
            }
        }

        public Episode GetEpisode(int id)
        {
            var episode = _dbContext.Episodes
                .Where(x => x.Id == id)
                .FirstOrDefault();
            return episode;
        }

        public Episode AddEpisode(Episode episode)
        {
            var addedEntity = _dbContext.Episodes.Add(episode);
            _dbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public Episode UpdateEpisode(Episode episode)
        {
            var foundEpisode = _dbContext.Episodes.FirstOrDefault(e => e.Id == episode.Id);

            if (foundEpisode != null)
            {
                foundEpisode.Title = episode.Title;

                _dbContext.SaveChanges();

                return foundEpisode;
            }

            return null;
        }

        public void DeleteEpisode(int id)
        {
            var foundEpisode = _dbContext.Episodes.FirstOrDefault(e => e.Id == id);
            if (foundEpisode == null) return;

            _dbContext.Episodes.Remove(foundEpisode);
            _dbContext.SaveChanges();
        }
    }
}
