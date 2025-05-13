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

        public async Task<int> AddRecordAsync(Record record)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@ArtistId", record.ArtistId);
            parameters.Add("@Name", record.Name);
            parameters.Add("@Field", record.Field);
            parameters.Add("@Recorded", record.Recorded);
            parameters.Add("@Label", record.Label);
            parameters.Add("@Pressing", record.Pressing);
            parameters.Add("@Rating", record.Rating);
            parameters.Add("@Discs", record.Discs);
            parameters.Add("@Media", record.Media);
            parameters.Add("@Bought", record.Bought);
            parameters.Add("@Cost", record.Cost);
            parameters.Add("@CoverName", null);
            parameters.Add("@Review", record.Review);
            parameters.Add("@RecordId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("adm_RecordInsert", parameters, commandType: CommandType.StoredProcedure);

            // ExecuteAsync() returns the rows affected. I want the output parameter value
            return parameters.Get<int>("@RecordId");
        }
        
        public async Task<bool> DeleteRecordAsync(int recordId)
        {
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@RecordId", recordId);
            var rowsAffected = await connection.ExecuteAsync("up_deleteRecord", parameters, commandType: CommandType.StoredProcedure);
            return rowsAffected > 0;
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
