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
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PlaylistRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<Playlist>> GetPlaylists(int? pageNumber, IDictionary<string, string> sortOrder, IDictionary<string, string> filters)
        {
            try
            {
                int pageSize = 6;
                var playlistList = _dbContext.Playlists
                                         .Include(x => x.Thumbnail)
                                         .Include(x => x.Category).AsQueryable();

                if (filters != null)
                { 
                    foreach (var filter in filters)
                    {
                        var filterField = filter.Key;
                        var filterValue = filter.Value;

                        playlistList = playlistList.WhereDynamic(filterField, filterValue);
                    }
                }
                if (sortOrder != null)
                { 
                    foreach (var sort in sortOrder)
                    {
                        var sortField = sort.Key;
                        var sortValue = sort.Value;

                        playlistList = playlistList.OrderByDynamic(sortField, sortValue);
                    }
                }

                return await PaginatedList<Playlist>.CreateAsync(playlistList.AsNoTracking(), pageNumber ?? 1, pageSize);
            }
            catch
            {
                throw new ApplicationException();
            }
        }

        public Playlist GetPlaylist(int id)
        {
            var playlist = _dbContext.Playlists
                .Where(x => x.Id == id)
                .Include(x => x.Image)
                .Include(x => x.Category)
                .Include(x => x.Owner.User)
                .Include(x => x.Chapters).ThenInclude(x => x.Episodes).ThenInclude(x => x.WatchHistory)
                .Include(x => x.Enrollments).ThenInclude(x => x.Profile)
                .FirstOrDefault();

            return playlist;
        }

        public Playlist AddPlaylist(Playlist playlist)
        {
            var addedEntity = _dbContext.Playlists.Add(playlist);
            _dbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public Playlist UpdatePlaylist(Playlist playlist)
        {
            var foundPlaylist = _dbContext.Playlists.FirstOrDefault(e => e.Id == playlist.Id);

            if (foundPlaylist != null)
            {
                foundPlaylist.Title = playlist.Title;
                foundPlaylist.Description = playlist.Description;
                foundPlaylist.ImageId = playlist.ImageId;
                foundPlaylist.ThumbnailId = playlist.ThumbnailId;
                foundPlaylist.OwnerId = playlist.OwnerId;
                foundPlaylist.CategoryId = playlist.CategoryId;
                foundPlaylist.Language = playlist.Language;
                foundPlaylist.Featured = playlist.Featured;
                foundPlaylist.Popular = playlist.Popular;

                _dbContext.SaveChanges();

                return foundPlaylist;
            }

            return null;
        }

        public void DeletePlaylist(int id)
        {
            var foundPlaylist = _dbContext.Playlists.FirstOrDefault(e => e.Id == id);
            if (foundPlaylist == null) return;

            _dbContext.Playlists.Remove(foundPlaylist);
            _dbContext.SaveChanges();
        }
    }
}
