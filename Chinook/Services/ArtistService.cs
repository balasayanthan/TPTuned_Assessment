using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class ArtistService : IArtistService
    {
        private readonly ChinookContext DbContext;
        public ArtistService(ChinookContext ctx)
        {
            DbContext = ctx;
        }

        public async Task<List<Album>> GetAlbumsForArtist(long artistId)
        {
            try
            {
                List<Album> dta = await DbContext.Albums.Where(a => a.ArtistId == artistId).ToListAsync();
                return dta;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Artist> GetArtistsById(long artistId)
        {
            try
            {
                Artist dta = DbContext.Artists.SingleOrDefault(a => a.ArtistId == artistId);
                return dta;
            }
            catch (Exception)
            {
                throw;
            }            
        }

        public async Task<List<Artist>> GetArtistsByNameSearch(string searchTerm)
        {
            try
            {
                List<Artist> dta = await DbContext.Artists.Where(a =>EF.Functions.Like(a.Name,$"%{searchTerm}%")).Include(b => b.Albums).ToListAsync();               
                return dta;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
