using Chinook.ClientModels;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class TrackService : ITrackService
    {
        private readonly ChinookContext DbContext;
        public TrackService(ChinookContext ctx)
        {
            DbContext = ctx;
        }

        public async Task<List<PlaylistTrackDto>> GetTracksByUserId(long artistId ,string currentUserId)
        {
            try
            {
                List<PlaylistTrackDto> Tracks = DbContext.Tracks.Where(a => a.Album.ArtistId == artistId)
                        .Include(a => a.Album)
                        .Select(t => new PlaylistTrackDto()
                        {
                            AlbumTitle = (t.Album == null ? "-" : t.Album.Title),
                            TrackId = t.TrackId,
                            TrackName = t.Name,
                            IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == currentUserId && up.Playlist.Name == "Favorites")).Any()
                        })
                        .ToList();
                return Tracks;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateTrack(Track track)
        {
            try
            {
                DbContext.Tracks.Update(track);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
