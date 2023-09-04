using Chinook.Models;

namespace Chinook.Services
{
    public interface IArtistService
    {
        Task<Artist> GetArtistsById(long artistId);

        Task<List<Artist>> GetArtistsByNameSearch(string serchTerm);

        Task<List<Album>> GetAlbumsForArtist(long artistId);

    }
}
