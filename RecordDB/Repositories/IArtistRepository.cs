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
        Task<int> UpdateArtistAsync(Artist artist);
        Task<Artist> GetArtistByIdAsync(int artistId);
        Task<string> GetArtistNameAsync(int artistId);
        Task<int> CountArtistsAsync();
        Task<IEnumerable<Artist>> GetArtistListAsync(); // get a dropdown list of all artists
        Task<IEnumerable<Artist>> GetArtistsWithNoBioAsync();
        Task<int> GetNoBiographyCountAsync();
        Task<int> GetArtistIdAsync(string firstName, string lastName);
        Task<int> GetArtistIdFromRecordAsync(int recordId);
        Task<Artist?> GetArtistByNameAsync(string name);
        Task<Artist?> GetArtistByFirstLastNameAsync(string firstName, string lastName);
        Task<Artist?> GetBiographyAsync(int artistId);
        Task<string> GetBiographyFromRecordIdAsync(int recordId);
        Task<bool> AddArtistAsync(Artist artist);
        Task<bool> AddArtistAsync(string firstName, string lastName, string biography);
        Task<bool> DeleteArtistAsync(int artistId);
        Task<bool> DeleteArtistAsync(string name);
        Task<IEnumerable<Artist>> GetBandArtistsAsync();
        Task<int> UpdateArtistAsync(int artistId, string firstName, string lastName, string name, string biography);
        Task<bool> CheckForArtistNameAsync(string name);
        Task<string> GetArtistNameByRecordIdAsync(int recordId);
        Task<Artist> ShowArtistAsync(int artistId);
    }
}
