using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace RecordDB.Data.Interfaces
{
    public interface IDataAccess
    {
        Task<int> GetCountOrIdAsync(string storedProcedureName, object parameters = null);
        Task<IEnumerable<T>> GetDataAsync<T>(string storedProcedureName);
        Task<IEnumerable<T>> GetDataAsync<T>(string storedProcedureName, object parameters);
        Task<T?> GetSingleAsync<T>(string storedProcedureName, object parameters) where T : class;
        Task<T> GetSingleEntityAsync<T>(string storedProcedureName, object parameters) where T : class;
        Task<string> GetTextAsync(string storedProcedureName, object parameters = null);
        Task<int> SaveDataAsync<T>(string storedProcedureName, T entity, string outputParameterName = "Id", DbType outputDbType = DbType.Int32);
        Task<int> DeleteDataAsync(string storedProcedureName, object parameter);
        //Task<IEnumerable<T>> GetAllAsync<T>(string sql, object? parameters = null);
        //Task<T> GetSingleAsync<T>(string sql, object? parameters = null);
        //Task<int> ExecuteAsync(string sql, object? parameters = null);
        //Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null);
        //Task<T> QuerySingleAsync<T>(string sql, object? parameters = null);
        //Task<int> ExecuteScalarAsync(string sql, object? parameters = null);
    }
}
