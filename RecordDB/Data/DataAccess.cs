using Dapper;
using Microsoft.AspNetCore.Builder;
using RecordDB.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Data.SqlClient;

namespace RecordDB.Data
{
    public class DataAccess : IDataAccess
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<DataAccess> _logger;

        public DataAccess(IDbConnectionFactory connectionFactory, ILogger<DataAccess> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<T>> GetDataAsync<T>(string storedProcedureName)
        {
            return await GetDataAsync<T>(storedProcedureName, null);
        }

        public async Task<IEnumerable<T>> GetDataAsync<T>(string storedProcedureName, object parameters)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                _logger.LogInformation("Executing stored procedure: {StoredProcedure} with parameters: {@Parameters}", storedProcedureName, parameters);
                return await connection.QueryAsync<T>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving all {typeof(T).Name} records");
                throw;
            }
        }

        public async Task<T?> GetSingleAsync<T>(string storedProcedureName, object parameters) where T : class
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                return await connection.QueryFirstOrDefaultAsync<T>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving {TypeName}", typeof(T).Name);
                throw;
            }
        }

        public async Task<T> GetSingleEntityAsync<T>(string storedProcedureName, object parameters) where T : class
        {
            var result = await GetSingleAsync<T>(storedProcedureName, parameters);
            return result ?? throw new KeyNotFoundException($"{typeof(T).Name} record not found");
        }

        public async Task<int> GetCountOrIdAsync(string storedProcedureName, object parameters = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
            _logger.LogInformation("Successfully executed {StoredProcedure}, returned {Count} items", storedProcedureName, result);
            return result;
        }

        public async Task<string> GetTextAsync(string storedProcedureName, object parameters = null)
        {
            var dynamicParameters = new DynamicParameters(parameters);

            try
            {
                using var connection = _connectionFactory.CreateConnection();
                var command = new CommandDefinition(storedProcedureName, dynamicParameters, commandType: CommandType.StoredProcedure);

                var result = await connection.ExecuteScalarAsync<string>(command);
                return result ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing text query {StoredProcedure}", storedProcedureName);
                return string.Empty;
            }
        }
    }
}
