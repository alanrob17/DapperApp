using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using RecordDB.Data.Interfaces;
using RecordDB.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

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

        public async Task<int> SaveDataAsync<T>(string storedProcedureName, T entity, string outputParameterName = "Id", DbType outputDbType = DbType.Int32)
        {
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            // Get properties to include in parameters
            var properties = typeof(T).GetProperties()
                .Where(p =>
                    // Exclude virtual/navigation properties
                    !p.GetGetMethod()?.IsVirtual == true &&
                    // Include value types or strings, exclude other classes
                    (!p.PropertyType.IsClass || p.PropertyType == typeof(string)) &&
                    // Exclude properties with [NotMapped] attribute
                    !Attribute.IsDefined(p, typeof(NotMappedAttribute)) &&
                    // Exclude computed properties
                    !Attribute.IsDefined(p, typeof(DatabaseGeneratedAttribute)) ||
                    // Include identity columns (DatabaseGeneratedOption.Identity)
                    (Attribute.IsDefined(p, typeof(DatabaseGeneratedAttribute)) &&
                    p.GetCustomAttribute<DatabaseGeneratedAttribute>()?.DatabaseGeneratedOption != DatabaseGeneratedOption.Computed));

            foreach (var prop in properties)
            {
                var value = prop.GetValue(entity);
                parameters.Add($"@{prop.Name}", value ?? (object)DBNull.Value);
                _logger.LogInformation("Adding parameter: {ParameterName} with value: {Value}", prop.Name, value);
            }
            // Add output parameter
            parameters.Add($"@{outputParameterName}", dbType: outputDbType, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>($"@{outputParameterName}");
        }

        //public async Task<int> SaveDataAsync<T>(string storedProcedureName, T entity, string outputParameterName = "Id", DbType outputDbType = DbType.Int32)
        //{
        //    using var connection = _connectionFactory.CreateConnection();
        //    var parameters = new DynamicParameters();

        //    // Get only non-virtual, non-navigation properties
        //    var properties = typeof(T).GetProperties()
        //        .Where(p => !p.GetGetMethod()?.IsVirtual == true && !p.PropertyType.IsClass || p.PropertyType == typeof(string));

        //    foreach (var prop in properties)
        //    {
        //        // Handle null values appropriately
        //        var value = prop.GetValue(entity);
        //        parameters.Add($"@{prop.Name}", value ?? (object)DBNull.Value);
        //    }

        //    // Add output parameter
        //    parameters.Add($"@{outputParameterName}", dbType: outputDbType, direction: ParameterDirection.Output);

        //    await connection.ExecuteAsync(storedProcedureName,  parameters, commandType: CommandType.StoredProcedure);

        //    return parameters.Get<int>($"@{outputParameterName}");
        //}

        public async Task<int> GetCountOrIdAsync(string storedProcedureName, object parameters = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
            _logger.LogInformation("Successfully executed {StoredProcedure}, returned {Count} items", storedProcedureName, result);
            return result;
        }

        public async Task<int> GetCountOrIdQueryAsync(string query)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(query);
            _logger.LogInformation("Successfully executed {query}, returned {Count} items", query, result);
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

        public async Task<int> DeleteDataAsync(string storedProcedureName, object parameter)
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                _logger.LogInformation("Executing delete procedure: {Procedure} for ID: {parameter}", storedProcedureName, parameter);

                int rowsAffected = await connection.ExecuteAsync(storedProcedureName, parameter, commandType: CommandType.StoredProcedure);

                _logger.LogDebug("Delete operation affected {RowsAffected} rows", rowsAffected);
                return rowsAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting record using parameter: {parameter} and procedure {Procedure}", parameter, storedProcedureName);
                throw;
            }
        }

        public async Task<decimal> GetCostAsync(string storedProcedureName, object parameters = null)
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                var cost = await connection.ExecuteScalarAsync<decimal>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Successfully executed stored procedure {Procedure}, returned value: {Value}", storedProcedureName, cost);
                return cost;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing stored procedure {Procedure} with parameters {@Parameters}", storedProcedureName, parameters);
                throw;
            }
        }

        public async Task<decimal> GetCostQueryAsync(string query)
        {
            using var connection = _connectionFactory.CreateConnection();

            try
            {
                var cost = await connection.ExecuteScalarAsync<decimal>(query);
                _logger.LogInformation("Successfully executed query: {Query}, returned value: {Value}", query, cost);
                return cost;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing query: {Query}", query);
                throw;
            }
        }
    }
}
