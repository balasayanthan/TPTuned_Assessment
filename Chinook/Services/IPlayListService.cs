using Chinook.ClientModels;
using Chinook.Models;

namespace Chinook.Services
{
    public interface IPlayListService
    {
        Task<List<Models.Playlist>> GetAllPlayList();

        Task <ClientModels.Playlist> GetPlayListbyId(long PlaylistId, string currentUserId);

        Task<DataResult> AddToNewPlaylist(PlayListDto playliSt, long trackId);

        Task<DataResult> AddToExsitingPlaylist(long playliStId, long  trackId);

        Task<DataResult> RemoveFromPlaylist(long playliStId, long trackId);
    }
}
