using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using RecordDB.Data.Interfaces;
using RecordDB.Models;

namespace RecordDB.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        private readonly IDataAccess _db;

        public RecordRepository(IDataAccess db)
        {
            _db = db;
        }

        public async Task<int> AddRecordAsync(Record record)
        {
            return await _db.SaveDataAsync("adm_RecordInsert", record, outputParameterName: "RecordId");
        }
        
        public async Task<bool> DeleteRecordAsync(int recordId)
        {
            var sproc = "up_DeleteRecord";
            var parameter = new { RecordId = recordId };
            var rowsAffected = await _db.DeleteDataAsync("up_deleteRecord", parameter);
            return rowsAffected > 0;
        }
        
        public async Task<int> CountTotalRecordsAsync()
        {
            string sproc = "up_GetTotalNumberOfAllRecords";
            return await _db.GetCountOrIdAsync(sproc, new { });
        }

        public async Task<IEnumerable<Record>> GetAllRecordsAsync()
        {
            string sproc = "up_RecordSelectAll";
            return await _db.GetDataAsync<Record>(sproc, new { });
        }

        public async Task<Record> GetRecordByIdAsync(int recordId)
        {
            string sproc = "up_GetRecordById";
            var parameter = new { RecordId = recordId };
            return await _db.GetSingleAsync<Record>(sproc, parameter);
        }

        public async Task<int> UpdateRecordAsync(Record record)
        {
            return await _db.SaveDataAsync("adm_UpdateRecord", record, outputParameterName: "RowsAffected");
        }

        public async Task<IEnumerable<Record>> GetRecordsByArtistIdAsync(int artistId)
        {
            string sproc = "up_GetRecordsByArtistId";
            var parameter = new DynamicParameters();
            parameter.Add("@ArtistId", artistId);
            return await _db.GetDataAsync<Record>(sproc, parameter);
        }

        public async Task<int> AddRecordAsync(int artistId, string name, string field, int recorded, string label, string pressing, string rating, int discs, string media, DateTime bought, decimal cost, string coverName, string review)
        {
            var record = new Record
            {
                ArtistId = artistId,
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

            return await _db.SaveDataAsync("adm_RecordInsert", record, outputParameterName: "RecordId");
        }

        public async Task<IEnumerable<Record>> GetArtistRecordsAsync(int artistId)
        {
            string sproc = "up_GetRecordsByArtistId";
            var parameter = new DynamicParameters();
            parameter.Add("@ArtistId", artistId);
            return await _db.GetDataAsync<Record>(sproc, parameter);
        }

        public async Task<IEnumerable<ArtistRecordReview>> NoRecordReviewsAsync()
        {
            string sproc = "up_GetNoRecordReview";
            return await _db.GetDataAsync<ArtistRecordReview>(sproc, new { });
        }

        public async Task<int> UpdateRecordAsync(int recordId, string name, string field, int recorded, string label, string pressing, string rating, int discs, string media, DateTime bought, decimal cost, string coverName, string review)
        {
            var record = new Record
            {
                RecordId = recordId,
                ArtistId = 0,
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

            return await _db.SaveDataAsync("adm_UpdateRecord", record, outputParameterName: "RowsAffected");
        }

        public async Task<string> CountDiscsAsync(string show)
        {
            string sproc = "up_CountDiscs";
            var parameter = new DynamicParameters();
            parameter.Add("@Show", show);
            var discs = await _db.GetCountOrIdAsync(sproc, parameter);
            return discs.ToString();
        }

        public async Task<string> GetArtistNumberOfRecordsAsync(int artistId)
        {
            string sproc = "up_GetArtistNumberOfRecords";
            var parameter = new DynamicParameters();
            parameter.Add("@ArtistId", artistId);
            var records = await _db.GetCountOrIdAsync(sproc, parameter);
            return records.ToString();
        }

        public async Task<Record> GetRecordByNameAsync(string name)
        {
            var sproc = "up_GetRecordByPartialName";
            var parameter = new { Name = name };
            return await _db.GetSingleAsync<Record>(sproc, parameter);
        }

         public async Task<int> GetRecordNumberByYearAsync(int year)
        {
            var sproc = "up_GetRecordedYearNumber";
            var parameter = new { Year = year };
            return await _db.GetCountOrIdAsync(sproc, parameter);
        }

         public async Task<int> GetTotalNumberOfCDsAsync()
        {
            var sproc = "adm_GetTotalCDCount";
            return await _db.GetCountOrIdAsync(sproc, new { });
        }

         public async Task<int> GetNoReviewCountAsync()
        {
            var sproc = "up_GetNoRecordReviewCount";
            return await _db.GetCountOrIdAsync(sproc, new { });
        }

        public async Task<int> GetBoughtDiscCountForYearAsync(int year)
        {
            // Get count from Bought field
            var sproc = "up_GetTotalYearNumber";
            var parameter = new { Year = year };
            return await _db.GetCountOrIdAsync(sproc, parameter);
        }

        public async Task<int> GetDiscCountForYearAsync(int year)
        {
            // Get count from Recorded field
            var sproc = "up_GetNumberOfRecordsForYear";
            var parameter = new { Year = year };
            return await _db.GetCountOrIdAsync(sproc, parameter);
        }

        public async Task<int> GetTotalNumberOfDiscsAsync()
        {
            var sproc = "up_GetTotalNumberOfAllRecords";
            return await _db.GetCountOrIdAsync(sproc, new { });
        }

        public async Task<ArtistRecord> GetRecordDetailsAsync(int recordId)
        {
            var sproc = "up_getSingleArtistAndRecord";
            var parameter = new { RecordId = recordId };
            return await _db.GetSingleAsync<ArtistRecord>(sproc, parameter);
        }

        public async Task<string> GetArtistNameFromRecordAsync(int recordId)
        {
            var sproc = "up_GetArtistNameByRecordId";
            var parameter = new { RecordId = recordId };
            var name = await _db.GetTextAsync(sproc, parameter);
            return name ?? string.Empty;
        }

        public async Task<IEnumerable<Total>> GetTotalArtistCostAsync()
        {
            var sproc = "sp_getTotalsForEachArtist";
            return await _db.GetDataAsync<Total>(sproc, new { });
        }

        public async Task<IEnumerable<Total>> GetTotalArtistDiscsAsync()
        {
            var sproc = "sp_getTotalDiscsForEachArtist";
            return await _db.GetDataAsync<Total>(sproc, new { });
        }

        public async Task<IEnumerable<Record>> GetRecordsByNameAsync(string name)
        {
            var sproc = "up_GetRecordByName";
            var parameter = new { Name = name };
            return await _db.GetDataAsync<Record>(sproc, parameter);
        }

        public async Task<IEnumerable<Record>> GetRecordListByArtistAsync(int artistId)
        {
            string sproc = "up_getRecordListandNone";
            var parameter = new { ArtistId = artistId };
            return await _db.GetDataAsync<Record>(sproc, parameter);
        }

        public async Task<ArtistRecord> GetRecordHtmlAsync(int recordId)
        {
            var sproc = "up_getSingleArtistAndRecord";
            var parameter = new { RecordId = recordId };
            return await _db.GetSingleAsync<ArtistRecord>(sproc, parameter);
        }

        public async Task<IEnumerable<ArtistRecord>> GetArtistRecordListAsync()
        {
            var sproc = "up_GetAllArtistsAndRecords";
            return await _db.GetDataAsync<ArtistRecord>(sproc, new { });
        }

        public async Task<int> GetTotalNumberOfRecordsAsync()
        {
            string sproc = "up_GetTotalNumberOfRecords";
            return await _db.GetCountOrIdAsync(sproc, new { });
        }

        public async Task<int> GetTotalNumberOfBluraysAsync()
        {
            string sproc = "up_GetTotalNumberOfAllBlurays";
            return await _db.GetCountOrIdAsync(sproc, new { });
        }

        public async Task<int> GetTotalNumberOfDVDsAsync()
        {
            string sproc = "up_GetTotalNumberOfAllDVDs";
            return await _db.GetCountOrIdAsync(sproc, new { });
        }
    }
}
