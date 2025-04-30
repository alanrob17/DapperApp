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

namespace RecordDB.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ArtistRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CountArtistsAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("up_GetArtistCount", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Artist>> GetAllArtistsAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Artist>("up_ArtistSelectAll", commandType: CommandType.StoredProcedure);
        }

        public async Task<Artist>? GetArtistByIdAsync(int artistId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Artist>("up_ArtistSelectById", new { ArtistId = artistId }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Artist>> GetBandArtistsAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Artist>("up_ArtistsBandTitles", commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> UpdateArtistAsync(Artist artist)
        {
            using var connection = _connectionFactory.CreateConnection();
            var affected = await connection.ExecuteAsync(
                "UPDATE Artist SET FirstName = null, LastName = @LastName, Name = @Name, Biography = @Biography WHERE ArtistId = @ArtistId", artist);
            return affected > 0;
        }

    }
}
