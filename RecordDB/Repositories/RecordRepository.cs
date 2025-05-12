using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using RecordDB.Data.Interfaces;
using RecordDB.Models;

namespace RecordDB.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RecordRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<bool> AddRecordAsync(Record record)
        {
            throw new NotImplementedException();
        }
        
        public async Task<bool> DeleteRecordAsync(int recordId)
        {
            throw new NotImplementedException();
        }
        
        public async Task<int> CountTotalRecordsAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("up_GetTotalNumberOfAllRecords", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Record>> GetAllRecordsAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Record>("up_RecordSelectAll", commandType: CommandType.StoredProcedure);
        }

        public async Task<Record> GetRecordByIdAsync(int recordId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Record>("up_GetRecordById", new { RecordId = recordId }, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> UpdateRecordAsync(Record record)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Record>> GetRecordsByArtistIdAsync(int artistId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Record>("up_GetRecordsByArtistId",  new { ArtistId = artistId }, commandType: CommandType.StoredProcedure);
        }

         public async Task<int> AddRecordAsync(int artistId, string name, string field, int recorded, string label, string pressing, string rating, int discs, string media, DateTime bought, decimal cost, string coverName, string review)
        {
            throw new NotImplementedException();
        }

         public async Task<List<Record>> GetArtistRecordsAsync(int artistId)
        {
            throw new NotImplementedException();
        }

         public async Task<List<Record>> GetRecordReviewsAsync()
        {
            throw new NotImplementedException();
        }

         public async Task<int> UpdateRecordAsync(int recordId, string name, string field, int recorded, string label, string pressing, string rating, int discs, string media, DateTime bought, decimal cost, string coverName, string review)
        {
            throw new NotImplementedException();
        }

         public async Task<string> CountDiscsAsync(string show)
        {
            throw new NotImplementedException();
        }

         public async Task<string> GetArtistNumberOfRecordsAsync(int artistId)
        {
            throw new NotImplementedException();
        }

         public async Task<List<Record>> GetNoRecordReviewsAsync()
        {
            throw new NotImplementedException();
        }

         public async Task<Record> GetRecordByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

         public async Task DeleteAsync(int recordId)
        {
            throw new NotImplementedException();
        }

         public async Task<int> GetRecordsByYearAsync(int year)
        {
            throw new NotImplementedException();
        }

         public async Task<int> GetTotalNumberOfCDsAsync()
        {
            throw new NotImplementedException();
        }

         public async Task<int> GetNoReviewCountAsync()
        {
            throw new NotImplementedException();
        }

         public async Task<int> GetBoughtDiscCountForYear(int year)
        {
            throw new NotImplementedException();
        }

         public async Task<int> GetTotalNumberOfDiscsAsync()
        {
            throw new NotImplementedException();
        }

         public async Task<Record> GetRecordDetailsAsync(int recordId)
        {
            throw new NotImplementedException();
        }

         public async Task<string> GetArtistNameFromRecordAsync(int recordId)
        {
            throw new NotImplementedException();
        }

         public async Task<List<Total>> GetTotalArtistCostAsync()
        {
            throw new NotImplementedException();
        }

         public async Task<List<Total>> GetTotalArtistDiscsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
