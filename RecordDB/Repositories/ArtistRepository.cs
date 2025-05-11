using RecordDB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using RecordDB.Data.Interfaces;
using System.Net.Quic;

namespace RecordDB.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ArtistRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<bool> AddArtistAsync(Artist artist)
        {
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", artist.FirstName);
            parameters.Add("@LastName", artist.LastName);
            parameters.Add("@Name", null);
            parameters.Add("@Biography", artist.Biography);
            parameters.Add("@ArtistId", 0);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

            var affected = await connection.ExecuteAsync("adm_ArtistInsert", parameters, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> AddArtistAsync(string firstName, string lastName, string biography)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", firstName);
            parameters.Add("@LastName", lastName);
            parameters.Add("@Name", null);
            parameters.Add("@Biography", biography);
            parameters.Add("@ArtistId", 0);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

            var affected = await connection.ExecuteAsync("adm_ArtistInsert", parameters, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> CheckForArtistNameAsync(string name)
        {
            using var connection = _connectionFactory.CreateConnection();

            string sproc = "up_CheckArtistExists";
            var parameter = new DynamicParameters();
            parameter.Add("@Name", name);

            return await connection.ExecuteScalarAsync<bool>(sproc, parameter, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> CountArtistsAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("up_GetArtistCount", commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> DeleteArtistAsync(int artistId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteScalarAsync<bool>("up_ArtistDelete", new { ArtistId = artistId }, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> DeleteArtistAsync(string name)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteScalarAsync<bool>("up_ArtistDeleteByName", new { Name = name }, commandType: CommandType.StoredProcedure);
        }

        public async Task<Artist?> GetArtistByFirstLastNameAsync(string firstName, string lastName)
        {
            using var connection = _connectionFactory.CreateConnection();

            string sproc = "up_ArtistByFirstLastName";
            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", firstName);
            parameters.Add("@LastName", lastName);

            IEnumerable<Artist> artist = await connection.QueryAsync<Artist>(sproc, parameters, commandType: CommandType.StoredProcedure);
            return artist?.FirstOrDefault() ?? null;
        }

        public async Task<Artist>? GetArtistByIdAsync(int artistId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Artist>("up_ArtistSelectById", new { ArtistId = artistId }, commandType: CommandType.StoredProcedure);
        }

        public async Task<Artist?> GetArtistByNameAsync(string name)
        {
            using var connection = _connectionFactory.CreateConnection();

            string sproc = "up_GetFullArtistByName";
            IEnumerable<Artist> artist = await connection.QueryAsync<Artist>(sproc, new { Name = name }, commandType: CommandType.StoredProcedure);
            return artist?.FirstOrDefault() ?? null;
        }

        public async Task<int> GetArtistIdAsync(string firstName, string lastName)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sproc = "up_getArtistIdByName";
            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", firstName);
            parameters.Add("@LastName", lastName);
            parameters.Add("@ArtistId", 0, DbType.Int32, ParameterDirection.Output);

            return await connection.ExecuteScalarAsync<int>(sproc, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> GetArtistIdFromRecordAsync(int recordId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sproc = "up_getArtistIdFromRecordId";
            var parameters = new DynamicParameters();
            parameters.Add("@RecordId", recordId);

            return await connection.ExecuteScalarAsync<int>(sproc, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Artist>> GetArtistListAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Artist>("up_ArtistSelectAll", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Artist>> GetArtistsWithNoBioAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Artist>> GetBandArtistsAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Artist>("up_ArtistsBandTitles", commandType: CommandType.StoredProcedure);
        }

        public async Task<Artist?> GetBiographyAsync(int artistId)
        {
            using var connection = _connectionFactory.CreateConnection();
            throw new NotImplementedException();
        }

        public async Task<string> GetBiographyFromRecordIdAsync(int recordId)
        {
            using var connection = _connectionFactory.CreateConnection();
            throw new NotImplementedException();
        }

        public async Task<int> GetNoBiographyCountAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateArtistAsync(Artist artist)
        {
            using var connection = _connectionFactory.CreateConnection();
            throw new NotImplementedException();
        }

        public async Task<int> UpdateArtistAsync(int artistId, string firstName, string lastName, string name, string biography)
        {
            using var connection = _connectionFactory.CreateConnection();
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateArtistsBandTitlesAsync(Artist artist)
        {
            using var connection = _connectionFactory.CreateConnection();
            var affected = await connection.ExecuteAsync(
                "UPDATE Artist SET FirstName = null, LastName = @LastName, Name = @Name, Biography = @Biography WHERE ArtistId = @ArtistId", artist);
            return affected > 0;
        }
    }
}
