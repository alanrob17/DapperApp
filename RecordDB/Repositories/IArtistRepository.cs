using RecordDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordDB.Repositories
{
    public interface IArtistRepository
    {
        Task<IEnumerable<Artist>> GetBandArtistsAsync();
        Task<bool> UpdateArtistAsync(Artist artist);
        Task<IEnumerable<Artist>> GetAllArtistsAsync();
        Task<Artist> GetArtistByIdAsync(int artistId);
        Task<int> CountArtistsAsync();

    }
}
