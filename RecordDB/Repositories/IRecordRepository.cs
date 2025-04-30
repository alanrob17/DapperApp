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
        Task<bool> DeleteRecordAsync(int recordId);
        Task<int> CountTotalRecordsAsync();
        Task<IEnumerable<Record>> GetRecordsByArtistIdAsync(int artistId);
    }
}
