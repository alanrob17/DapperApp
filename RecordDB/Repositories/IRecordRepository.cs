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
        Task<bool> UpdateRecordAsync(Record record);
        Task<bool> AddRecordAsync(Record record);
        Task<int> AddRecordAsync(int artistId, string name, string field, int recorded, string label, string pressing, string rating, int discs, string media, DateTime bought, decimal cost, string coverName, string review);
        Task<bool> DeleteRecordAsync(int recordId);
        Task<int> CountTotalRecordsAsync();
        Task<IEnumerable<Record>> GetRecordsByArtistIdAsync(int artistId);
        Task<List<Record>> GetArtistRecordsAsync(int artistId);
        Task<List<Record>> GetRecordReviewsAsync();
        Task<int> UpdateRecordAsync(int recordId, string name, string field, int recorded, string label, string pressing, string rating, int discs, string media, DateTime bought, decimal cost, string coverName, string review);
        Task<string> CountDiscsAsync(string show);
        Task<string> GetArtistNumberOfRecordsAsync(int artistId);
        Task<List<Record>> GetNoRecordReviewsAsync();
        Task<Record> GetRecordByNameAsync(string name);
        Task DeleteAsync(int recordId);
        Task<int> GetRecordsByYearAsync(int year);
        Task<int> GetTotalNumberOfCDsAsync();
        Task<int> GetNoReviewCountAsync();
        Task<int> GetBoughtDiscCountForYear(int year);
        Task<int> GetTotalNumberOfDiscsAsync();
        Task<Record> GetRecordDetailsAsync(int recordId);
        Task<string> GetArtistNameFromRecordAsync(int recordId);
        Task<List<Total>> GetTotalArtistCostAsync();
        Task<List<Total>> GetTotalArtistDiscsAsync();
    }
}
