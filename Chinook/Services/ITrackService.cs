using Chinook.ClientModels;
using Chinook.Models;

namespace Chinook.Services
{
    public interface ITrackService
    {
        Task<List<PlaylistTrackDto>> GetTracksByUserId(long ArtistId, string currentUserId);
    }
}
