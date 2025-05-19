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
using Microsoft.Data.SqlClient;
using Microsoft.Win32.SafeHandles;

namespace RecordDB.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataAccess _db;

        public ArtistRepository(IDbConnectionFactory connectionFactory, IDataAccess db)
        {
            _connectionFactory = connectionFactory;
            _db = db;
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
            string sproc = "up_CheckArtistExists";
            var parameter = new { Name = name };
            return await _db.GetCountOrIdAsync(sproc, parameter) > 0;
        }

        public async Task<int> CountArtistsAsync()
        {
            var sproc = "up_GetArtistCount";
            return await _db.GetCountOrIdAsync(sproc, new { });
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
            string sproc = "up_ArtistByFirstLastName";
            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", firstName);
            parameters.Add("@LastName", lastName);
            return await _db.GetSingleAsync<Artist>(sproc, parameters);
        }

        public async Task<Artist>? GetArtistByIdAsync(int artistId)
        {
            var sproc = "up_ArtistSelectById";
            var parameter = new { ArtistId = artistId };
            return await _db.GetSingleAsync<Artist>(sproc, parameter);
        }

        public async Task<Artist?> GetArtistByNameAsync(string name)
        {
            string sproc = "up_GetFullArtistByName";
            var parameter = new { Name = name };
            return await _db.GetSingleAsync<Artist>(sproc, parameter);
        }

        public async Task<int> GetArtistIdAsync(string firstName, string lastName)
        {
            var sproc = "up_getArtistIdByName";
            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", firstName);
            parameters.Add("@LastName", lastName);
            parameters.Add("@ArtistId", 0, DbType.Int32, ParameterDirection.Output);
            return await _db.GetCountOrIdAsync(sproc, parameters);
        }

        public async Task<int> GetArtistIdFromRecordAsync(int recordId)
        {
            var sproc = "up_getArtistIdFromRecordId";
            var parameter = new { RecordId = recordId };
            return await _db.GetCountOrIdAsync(sproc, parameter);
        }

        public async Task<IEnumerable<Artist>> GetArtistListAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Artist>("up_ArtistSelectAll", commandType: CommandType.StoredProcedure);
        }

        public async Task<string> GetArtistNameAsync(int artistId)
        {
            var sproc = "up_GetArtistNameByArtistId";
            var parameters = new { ArtistId = artistId};
            var name = await _db.GetTextAsync(sproc, parameters);

            return name ?? string.Empty;
        }

        public async Task<string> GetArtistNameByRecordIdAsync(int recordId)
        {
            var sproc = "up_GetArtistNameByRecordId";
            var parameter = new { RecordId = recordId };           
            Artist artist = await _db.GetSingleAsync<Artist>(sproc, parameter);

            return artist?.Name ?? string.Empty;
        }

        public async Task<IEnumerable<Artist>> GetArtistsWithNoBioAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Artist>("up_selectArtistsWithNoBio", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Artist>> GetBandArtistsAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Artist>("up_ArtistsBandTitles", commandType: CommandType.StoredProcedure);
        }

        public async Task<Artist?> GetBiographyAsync(int artistId)
        {
            var sproc = "up_ArtistSelectById";
            var parameters = new { ArtistId = artistId };
            return await _db.GetSingleAsync<Artist>(sproc, parameters);
        }

        public async Task<string> GetBiographyFromRecordIdAsync(int recordId)
        {
            var sproc = "up_GetBiography";
            var parameter = new { RecordId = recordId };
            return await _db.GetTextAsync(sproc, parameter);
        }

        public async Task<int> GetNoBiographyCountAsync()
        {
            var sproc = "up_NoBioCount";
            return await _db.GetCountOrIdAsync(sproc, new { });
        }

        public async Task<Artist> ShowArtistAsync(int artistId)
        {
            var sproc = "up_ArtistSelectById";
            var parameter = new { ArtistId = artistId };
            return await _db.GetSingleAsync<Artist>(sproc, parameter);
        }

        public async Task<int> UpdateArtistAsync(Artist artist)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sproc = "up_UpdateArtist";

            var parameters = new DynamicParameters();
            parameters.Add("@ArtistId", artist.ArtistId);
            parameters.Add("@FirstName", artist.FirstName);
            parameters.Add("@LastName", artist.LastName);
            parameters.Add("@Name", artist.Name);
            parameters.Add("@Biography", artist.Biography);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

            return await connection.ExecuteAsync(sproc, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateArtistAsync(int artistId, string firstName, string lastName, string name, string biography)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sproc = "up_UpdateArtist";
            var parameters = new DynamicParameters();
            parameters.Add("@ArtistId", artistId);
            parameters.Add("@FirstName", firstName);
            parameters.Add("@LastName", lastName);
            parameters.Add("@Name", name);
            parameters.Add("@Biography", biography);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

            return await connection.ExecuteAsync(sproc, parameters, commandType: CommandType.StoredProcedure);
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
