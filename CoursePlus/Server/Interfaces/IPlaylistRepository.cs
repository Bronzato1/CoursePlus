using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public interface IPlaylistRepository
    {
        Task<PaginatedList<Playlist>> GetPlaylists(int? pageNumber, IDictionary<string, string> sortOrder, IDictionary<string, string> filters);

        public Playlist GetPlaylist(int id);

        public Playlist AddPlaylist(Playlist playlist);

        public Playlist UpdatePlaylist(Playlist playlist);

        public void DeletePlaylist(int id);
    }
}
