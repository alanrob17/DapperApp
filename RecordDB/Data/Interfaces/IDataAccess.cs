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
        Task<int> GetCountOrIdQueryAsync(string query);
        Task<IEnumerable<T>> GetDataAsync<T>(string storedProcedureName);
        Task<IEnumerable<T>> GetDataAsync<T>(string storedProcedureName, object parameters);
        Task<T?> GetSingleAsync<T>(string storedProcedureName, object parameters) where T : class;
        Task<T> GetSingleEntityAsync<T>(string storedProcedureName, object parameters) where T : class;
        Task<string> GetTextAsync(string storedProcedureName, object parameters = null);
        Task<int> SaveDataAsync<T>(string storedProcedureName, T entity, string outputParameterName = "Id", DbType outputDbType = DbType.Int32);
        Task<int> DeleteDataAsync(string storedProcedureName, object parameter);
        Task<decimal> GetCostQueryAsync(string query);
       Task<decimal> GetCostAsync(string storedProcedure, object parameter = null);
    }
}
