using Chinook.ClientModels;
using Chinook.Models;
using Chinook.Profiles;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;


namespace Chinook.Services
{
    public class PlaylistService : IPlayListService
    {
        private readonly ChinookContext DbContext;
        public PlaylistService(ChinookContext ctx)
        {
            DbContext = ctx;
        }

        public async Task<List<Models.Playlist>> GetAllPlayList()
        {
            try
            {
                List<Models.Playlist> dta = await DbContext.Playlists.ToListAsync();
                return dta;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClientModels.Playlist> GetPlayListbyId(long PlaylistId,string currentUserId)
        {
            try
            {
                ClientModels.Playlist Playlist  = DbContext.Playlists
                .Include(a => a.Tracks).ThenInclude(a => a.Album).ThenInclude(a => a.Artist)
                .Where(p => p.PlaylistId == PlaylistId)
                .Select(p => new ClientModels.Playlist()
                {
                    Name = p.Name,
                    Tracks = p.Tracks.Select(t => new ClientModels.PlaylistTrackDto()
                    {
                        AlbumTitle = t.Album.Title,
                        ArtistName = t.Album.Artist.Name,
                        TrackId = t.TrackId,
                        TrackName = t.Name,
                        IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == currentUserId && up.Playlist.Name == "Favorites")).Any()
                    }).ToList()
                })
                .FirstOrDefault();

                return Playlist;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DataResult> AddToNewPlaylist(PlayListDto playliSt, long trackId)
        {
            try
            {
                bool exists = DbContext.Playlists.Any(e => e.Name == playliSt.Name);
                if (exists) {
                    return new DataResult {Success=false,Message = "Playlist already exist" };
                }
                else
                {
                    Models.Playlist dta = new() { 
                        PlaylistId = DbContext.Playlists.Select(a => a.PlaylistId).Max() + 1,
                        Name = playliSt.Name 
                    };
                    DbContext.Playlists.Add(dta);
                    await DbContext.SaveChangesAsync();


                    var Trak = DbContext.Tracks.Where(a => a.TrackId == trackId).FirstOrDefault();
                    var PlayList = DbContext.Playlists.Where(a => a.PlaylistId == dta.PlaylistId).Include(a => a.Tracks).FirstOrDefault();
                    if (Trak != null)
                    {
                        PlayList.Tracks.Add(Trak);
                        DbContext.Playlists.Update(dta);
                        await DbContext.SaveChangesAsync();
                    }
                    return new DataResult { Success = false, Message = $"New play list {playliSt.Name} Created. And Track : {Trak.Name} - Added" };
                }
            }
            catch (Exception ex)
            {
                return new DataResult { Success = true, Message = ex.Message }; ;
            }
        }
        public async Task<DataResult> AddToExsitingPlaylist(long playliStId, long trackId)
        {
            try
            {
                var Trak = DbContext.Tracks.Where(a => a.TrackId == trackId).FirstOrDefault();
                var PlayList = DbContext.Playlists.Where(a => a.PlaylistId == playliStId).Include(a=>a.Tracks).FirstOrDefault();

                PlayList.Tracks.Add(Trak);
                DbContext.Playlists.Update(PlayList);
                await DbContext.SaveChangesAsync();
                return new DataResult { Success = true, Message=$"Track : {Trak.Name} - Added to play list : {PlayList.Name}" };
            }
            catch (Exception ex)
            {
                return new DataResult { Success = true,Message =ex.Message };
            }
        }

        public async Task<DataResult> RemoveFromPlaylist(long playliStId, long trackId)
        {
            try
            {
                var playList = DbContext.Playlists.Where(a => a.PlaylistId == playliStId).Include(a => a.Tracks).FirstOrDefault();
                var Trak = DbContext.Tracks.Where(a => a.TrackId == trackId).FirstOrDefault();

                playList.Tracks.Remove(Trak);
                DbContext.Playlists.Update(playList);
                await DbContext.SaveChangesAsync();
                return new DataResult { Success = true, Message = $"Track : {Trak.Name} - Removed from play list : {playList.Name}" };
            }
            catch (Exception ex)
            {
                return new DataResult { Success = false, Message = ex.Message };
            }
        }
    }
}
