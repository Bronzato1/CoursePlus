using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Services
{
    public interface IPlaylistService
    {
        Task<PaginatedList<Playlist>> GetPlaylists(int pageNumber = 1, IDictionary<string, string> sortOrder = null, IDictionary<string, string> filters = null);

        Task<Playlist> GetPlaylist(int id);

        Task<Playlist> AddPlaylist(Playlist playlist);

        Task UpdatePlaylist(Playlist playlist);

        Task DeletePlaylist(int id);
    }
}
