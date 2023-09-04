using Chinook.ClientModels;
using Chinook.Models;

namespace Chinook.Services
{
    public interface IUserPlayListService
    {
        Task<Int64> GetMyFavoutitPlayLIst(string userId);

        Task<DataResult> AddFavoriteTrack(string userId, long addingTrackId);

        Task<DataResult> RemoveFavoriteTrack(string userId, long reomingTrackId);
    }
}
