using RecordDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordDB.Repositories
{
    public interface IRecordRepository
    {
        Task<IEnumerable<Record>> GetAllRecordsAsync();
        Task<Record> GetRecordByIdAsync(int recordId);
        Task<int> UpdateRecordAsync(Record record);
        Task<int> AddRecordAsync(Record record);
        Task<int> AddRecordAsync(int artistId, string name, string field, int recorded, string label, string pressing, string rating, int discs, string media, DateTime bought, decimal cost, string coverName, string review);
        Task<bool> DeleteRecordAsync(int recordId);
        Task<int> CountTotalRecordsAsync();
        Task<IEnumerable<Record>> GetRecordsByArtistIdAsync(int artistId);
        Task<IEnumerable<Record>> GetArtistRecordsAsync(int artistId);
        Task<IEnumerable<ArtistRecordReview>> NoRecordReviewsAsync();
        Task<int> UpdateRecordAsync(int recordId, string name, string field, int recorded, string label, string pressing, string rating, int discs, string media, DateTime bought, decimal cost, string coverName, string review);
        Task<string> CountDiscsAsync(string show);
        Task<string> GetArtistNumberOfRecordsAsync(int artistId);
        Task<Record> GetRecordByNameAsync(string name);
        Task<IEnumerable<Record>> GetRecordListByArtistAsync(int artistId);
        Task<IEnumerable<Record>> GetRecordsByNameAsync(string name);
        Task<int> GetRecordNumberByYearAsync(int year);
        Task<int> GetTotalNumberOfCDsAsync();
        Task<int> GetNoReviewCountAsync();
        Task<int> GetBoughtDiscCountForYearAsync(int year);
        Task<int> GetDiscCountForYearAsync(int year);
        Task<int> GetTotalNumberOfDiscsAsync();
        Task<ArtistRecord> GetRecordDetailsAsync(int recordId);
        Task<string> GetArtistNameFromRecordAsync(int recordId);
        Task<IEnumerable<Total>> GetTotalArtistCostAsync();
        Task<IEnumerable<Total>> GetTotalArtistDiscsAsync();
        Task<ArtistRecord> GetRecordHtmlAsync(int recordId);
        Task<IEnumerable<ArtistRecord>> GetArtistRecordListAsync();
        Task<int> GetTotalNumberOfRecordsAsync();
        Task<int> GetTotalNumberOfDVDsAsync();
        Task<int> GetTotalNumberOfBluraysAsync();
    }
}
