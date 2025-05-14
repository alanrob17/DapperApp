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

        public async Task<int> UpdateRecordAsync(Record record)
        {
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@RecordId", record.RecordId);
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
            var recordId = await connection.ExecuteAsync("adm_UpdateRecord", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@RecordId");
        }

        public async Task<IEnumerable<Record>> GetRecordsByArtistIdAsync(int artistId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Record>("up_GetRecordsByArtistId",  new { ArtistId = artistId }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddRecordAsync(int artistId, string name, string field, int recorded, string label, string pressing, string rating, int discs, string media, DateTime bought, decimal cost, string coverName, string review)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@ArtistId", artistId);
            parameters.Add("@Name", name);
            parameters.Add("@Field", field);
            parameters.Add("@Recorded", recorded);
            parameters.Add("@Label", label);
            parameters.Add("@Pressing", pressing);
            parameters.Add("@Rating", rating);
            parameters.Add("@Discs", discs);
            parameters.Add("@Media", media);
            parameters.Add("@Bought", bought);
            parameters.Add("@Cost", cost);
            parameters.Add("@CoverName", null);
            parameters.Add("@Review", review);
            parameters.Add("@RecordId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("adm_RecordInsert", parameters, commandType: CommandType.StoredProcedure);

            // ExecuteAsync() returns the rows affected. I want the output parameter value
            return parameters.Get<int>("@RecordId");
        }

        public async Task<List<Record>> GetArtistRecordsAsync(int artistId)
        {
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ArtistId", artistId);
            return (await connection.QueryAsync<Record>("up_GetRecordsByArtistId", parameters, commandType: CommandType.StoredProcedure)).ToList();
        }

         public async Task<List<ArtistRecordReview>> NoRecordReviewsAsync()
        {

            using var connection = _connectionFactory.CreateConnection();
            return (await connection.QueryAsync<ArtistRecordReview>("up_GetNoRecordReview", commandType: CommandType.StoredProcedure)).ToList();
        }

         public async Task<int> UpdateRecordAsync(int recordId, string name, string field, int recorded, string label, string pressing, string rating, int discs, string media, DateTime bought, decimal cost, string coverName, string review)
        {
            using var connection = _connectionFactory.CreateConnection();

            var record = new Record
            {
                RecordId = recordId,
                Name = name,
                Field = field,
                Recorded = recorded,
                Label = label,
                Pressing = pressing,
                Rating = rating,
                Discs = discs,
                Media = media,
                Bought = bought,
                Cost = cost,
                CoverName = coverName,
                Review = review
            };
            var result = await connection.ExecuteAsync("adm_UpdateRecord", new { Record = record}, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<string> CountDiscsAsync(string show)
        {
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Show", show);
            var discs = await connection.ExecuteScalarAsync<int>("up_CountDiscs", parameters, commandType: CommandType.StoredProcedure);

            return discs.ToString();
        }

        public async Task<string> GetArtistNumberOfRecordsAsync(int artistId)
        {
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ArtistId", artistId);
            var records = await connection.ExecuteScalarAsync<int>("up_GetArtistNumberOfRecords", parameters, commandType: CommandType.StoredProcedure);
            return records.ToString();
        }

        public async Task<Record> GetRecordByNameAsync(string name)
        {
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Name", name);
            return await connection.QueryFirstOrDefaultAsync<Record>("up_GetRecordByPartialName", parameters, commandType: CommandType.StoredProcedure);
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
