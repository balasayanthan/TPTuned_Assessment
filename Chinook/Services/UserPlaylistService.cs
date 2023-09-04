using Chinook.ClientModels;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class UserPlaylistService : IUserPlayListService
    {
        private readonly ChinookContext DbContext;
        public UserPlaylistService(ChinookContext ctx)
        {
            DbContext = ctx;
        }

        public async Task<Int64> GetMyFavoutitPlayLIst(string userId)
        {
            try
            {
                UserPlaylist userPlaylist = DbContext.UserPlaylists.Where(a => a.UserId == userId && a.Playlist.Name == "Favorites").FirstOrDefault();
                return userPlaylist.PlaylistId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataResult> AddFavoriteTrack(string userId, long addingTrackId)
        {            
            try
            {
                long PlayListId;
                UserPlaylist? userPlaylist = DbContext.UserPlaylists.Where(a => a.UserId == userId && a.Playlist.Name == "Favorites").FirstOrDefault();
                if (userPlaylist == null)
                {
                    Models.Playlist playlist = new()
                    {
                        PlaylistId = DbContext.Playlists.Select(a => a.PlaylistId).Max() + 1,
                        Name = "Favorites"
                    };

                    DbContext.Playlists.Add(playlist);
                    DbContext.SaveChanges();
                    PlayListId = playlist.PlaylistId;

                    UserPlaylist userPlayList = new()
                    {
                        UserId = userId,
                        PlaylistId = PlayListId
                    };
                    DbContext.UserPlaylists.Add(userPlayList);
                }
                else
                {
                    PlayListId = userPlaylist.PlaylistId;
                }

                var Trak = DbContext.Tracks.Where(a => a.TrackId == addingTrackId).FirstOrDefault();
                var PlayList = DbContext.Playlists.Where(a => a.PlaylistId == PlayListId).FirstOrDefault();

                PlayList.Tracks.Add(Trak);
                DbContext.Playlists.Update(PlayList);
                await DbContext.SaveChangesAsync();
                return new DataResult { Success = true, Message = $"{Trak.Name} - Added to Favourits" };
            }
            catch (Exception ex)
            {
                return new DataResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<DataResult> RemoveFavoriteTrack(string userId, long reomingTrackId)
        {
            try
            {
                UserPlaylist? userPlaylist = DbContext.UserPlaylists.Where(a => a.UserId == userId && a.Playlist.Name == "Favorites").FirstOrDefault();
                if (userPlaylist != null)
                {
                    var PlayList = DbContext.Playlists.Where(a => a.PlaylistId == userPlaylist.PlaylistId).Include(b=>b.Tracks).FirstOrDefault();
                    var Trak = DbContext.Tracks.Where(a => a.TrackId == reomingTrackId).FirstOrDefault();
                    PlayList.Tracks.Remove(Trak);

                    DbContext.Playlists.Update(PlayList);
                    await DbContext.SaveChangesAsync();
                    return new DataResult { Success = true, Message = $"{Trak.Name} - Removed from Favourits" };
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return new DataResult { Success = false, Message = ex.Message };
            }
        }
    }
}
