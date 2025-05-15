using Microsoft.VisualBasic.FileIO;
using RecordDB.Models;
using RecordDB.Repositories;
using RecordDB.Services.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordDB.Services
{
    public class RecordDbService
    {
        private readonly IRecordRepository _repository;
        private readonly IOutputService _output;

        public RecordDbService(IRecordRepository repository, IOutputService output)
        {
            _repository = repository;
            _output = output;
        }
        public async Task RunAllDatabaseOperations()
        {
            //await GetAllRecordsAsync();
            //await GetRecordByIdAsync(1076);
            //await CountTotalRecordsAsync();
            //await GetRecordsByArtistIdAsync(114);
            // await AddNewRecord();
            // await AddNewRecord(863, "Bass Rebellion", "Rock", 2025, "Wobble Music", "Aus", "***", 1, "CD", DateTime.Now, 19.99m, null, "This is James' second album.");
            // await DeleteRecordAsync(5285);
            // await UpdateRecordAsync();
            // await UpdateRecordAsync(5288, "Rockin' The Bass Again", "Rock", 2023, "Wibble Wobble Music", "Aus", "***", 1, "CD", DateTime.Now, 19.99m, null, "This is James' ninth album.");
            // await GetArtistRecordsAsync(114);
            // await GetNoRecordReviewsAsync();
            // await CountDiscsAsync("****");
            // await GetArtistNumberOfRecordsAsync(114);
            // await GetRecordByNameAsync("Blonde On");
            // await GetRecordsByNameAsync("Bringing");
            // await GetArtistNameFromRecordAsync(3232);
            // await GetRecordsByYearAsync(1974);
            // await GetTotalNumberOfCDsAsync();
            // await GetNoReviewCountAsync();
            // await GetBoughtDiscCountForYear(2010);
            // await GetTotalNumberOfDiscsAsync();
            // await GetRecordDetailsAsync(3232);
            // await GetTotalArtistCostAsync();
            await GetTotalArtistDiscsAsync();
        }

        private async Task GetTotalArtistDiscsAsync()
        {
            var totals = await _repository.GetTotalArtistDiscsAsync();
            foreach (var total in totals)
            {
                _output.WriteLine($"ArtistId: {total.ArtistId}, Artist: {total.Name} - Total Discs: {total.TotalDiscs}");
            }
        }

        private async Task GetTotalArtistCostAsync()
        {
            var totals = await _repository.GetTotalArtistCostAsync();
            foreach (var total in totals)
            {
                _output.WriteLine($"ArtistId: {total.ArtistId}, Artist: {total.Name} - Total Discs: {total.TotalDiscs} - Total Cost: {total.TotalCost}");
            }
        }

        private async Task GetRecordDetailsAsync(int recordId)
        {

            var record = await _repository.GetRecordDetailsAsync(recordId);
            if (record is not null)
            {
                _output.WriteLine($"ArtistId: {record.ArtistId}, Artist: {record.ArtistName} -- Record Id: {record.RecordId}, Name: {record.Name}, Recorded: {record.Recorded}, Media: {record.Media}");
            }
            else
            {
                _output.WriteError($"Record with ID {recordId} not found.");
            }
        }

        private async Task GetTotalNumberOfDiscsAsync()
        {
            var total = await _repository.GetTotalNumberOfDiscsAsync();
            if (total > 0)
            {
                _output.WriteLine($"Total number of discs: {total}");
            }
            else
            {
                _output.WriteError("No discs found.");
            }
        }

        private async Task GetBoughtDiscCountForYear(int year)
        {
            var count = await _repository.GetBoughtDiscCountForYear(year);
            if (count > 0)
            {
                _output.WriteLine($"Total number of bought discs for year {year}: {count}");
            }
            else
            {
                _output.WriteError($"No bought discs found for year: {year}");
            }
        }

        private async Task GetNoReviewCountAsync()
        {
            var count = await _repository.GetNoReviewCountAsync();
            if (count > 0)
            {
                _output.WriteLine($"Total number of records with no reviews: {count}");
            }
            else
            {
                _output.WriteError("No records found with no reviews.");
            }
        }

        private async Task GetTotalNumberOfCDsAsync()
        {
            var total = await _repository.GetTotalNumberOfCDsAsync();
            if (total > 0)
            {
                _output.WriteLine($"Total number of CD's: {total}");
            }
            else
            {
                _output.WriteError("No CD's found.");
            }
        }

        private async Task GetRecordsByYearAsync(int year)
        {
            var records = await _repository.GetRecordsByYearAsync(year);
            if (records > 0)
            {
                _output.WriteLine($"Total records for year {year}: {records}");
            }
            else
            {
                _output.WriteError($"No records found for year: {year}");
            }
        }

        private async Task GetArtistNameFromRecordAsync(int recordId)
        {
            var artistName = await _repository.GetArtistNameFromRecordAsync(recordId);
            if (!string.IsNullOrEmpty(artistName))
            {
                _output.WriteLine($"Artist Name: {artistName}");
            }
            else
            {
                _output.WriteError($"No artist found for record ID: {recordId}");
            }
        }

        private async Task GetRecordByNameAsync(string name)
        {
            var record = await _repository.GetRecordByNameAsync(name);
            if (record is not null)
            {
                var artistName = await _repository.GetArtistNameFromRecordAsync(record.RecordId);
                if (artistName is not null)
                {
                    _output.WriteLine($"Artist: {artistName} - Record: {record.Name} - {record.Recorded}");
                }
                else
                {
                    _output.WriteError($"No artist found for record: {name}");
                }
            }
            else
            {
                _output.WriteError($"Record: {name} not found.");
            }
        }

        private async Task GetRecordsByNameAsync(string name)
        {

            var records = await _repository.GetRecordsByNameAsync(name);
            if (records is not null)
            {
                foreach (var record in records)
                {
                    var artistName = await _repository.GetArtistNameFromRecordAsync(record.RecordId);
                    if (artistName is not null)
                    {
                        _output.WriteLine($"Artist: {artistName} - Record: {record.Name} - {record.Recorded}");
                    }
                    else
                    {
                        _output.WriteError($"No artist found for record: {name}");
                    }
                }
            }
            else
            {
                _output.WriteError($"No records found for name: {name}");
            }
        }

        private async Task GetArtistNumberOfRecordsAsync(int artistId)
        {
            var records = await _repository.GetArtistNumberOfRecordsAsync(artistId);
            if (records is not null)
            {
                _output.WriteLine($"ArtistId: {artistId} - Number of Records: {records}");
            }
            else
            {
                _output.WriteError($"No records found for ArtistId: {artistId}");
            }
        }

        private async Task CountDiscsAsync(string show)
        {

            var discs = await _repository.CountDiscsAsync(show);
            _output.WriteLine($"{show}: Total Discs: {discs}");
        }

        private async Task GetNoRecordReviewsAsync()
        {
            var records = await _repository.NoRecordReviewsAsync();
            foreach (var record in records)
            {
                _output.WriteLine($"ArtistId: {record.ArtistId} - Artist: {record.ArtistName} - Record Id: {record.RecordId}: {record.Recorded} - {record.RecordName}");
            }
        }

        private async Task GetArtistRecordsAsync(int artistId)
        {

            var records = await _repository.GetArtistRecordsAsync(artistId);
            foreach (var record in records)
            {
                _output.WriteLine($"Record Id: {record.RecordId}, Name: {record.Name}, Recorded: {record.Recorded}, Media: {record.Media}");
            }
        }

        private async Task UpdateRecordAsync(int recordId, string name, string field, int recorded, string label, string pressing, string rating, int discs, string media, DateTime bought, decimal cost, string coverName, string review)
        {
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
            recordId = await _repository.UpdateRecordAsync(record);
            if (recordId > 0)
            {
                _output.WriteLine($"Record Id: {recordId} updated successfully.");
            }
            else
            {
                _output.WriteError($"Failed to update record with Id: {recordId}.");
            }
        }

        private async Task UpdateRecordAsync()
        {
            var record = new Record
            {
                RecordId = 5288,
                Name = "Rockin' Bass Rebellion",
                Field = "Rock",
                Recorded = 2023,
                Label = "Wobble Music",
                Pressing = "Aus",
                Rating = "***",
                Discs = 3,
                Media = "CD",
                Bought = DateTime.Now,
                Cost = 29.99m,
                CoverName = null,
                Review = "This is James' sixth album."
            };
            var recordId = await _repository.UpdateRecordAsync(record);
            if (recordId > 0)
            {
                _output.WriteLine($"Record with ID {record.RecordId} updated successfully.");
            }
            else
            {
                _output.WriteError($"Failed to update record with ID {record.RecordId}.");
            }
        }

        private async Task DeleteRecordAsync(int recordId)
        {
            var result = await _repository.DeleteRecordAsync(recordId);
            if (result)
            {
                _output.WriteLine($"Record with ID {recordId} deleted successfully.");
            }
            else
            {
                _output.WriteError($"Failed to delete record with ID {recordId}.");
            }
        }

        private async Task AddNewRecord()
        {
            var recordId = await _repository.AddRecordAsync(new Record
            {
                RecordId = 5288,
                ArtistId = 863,
                Name = "Rockin' and Rollin' Bass",
                Field = "Rock",
                Recorded = 2020,
                Label = "Wobble Dobble Music",
                Pressing = "Ger",
                Rating = "****",
                Discs = 1,
                Media = "CD",
                Bought = DateTime.Now,
                Cost = 10.99m,
                CoverName = null,
                Review = "This is James' fifth album."
            });

            if (recordId > 0)
            {
                _output.WriteLine($"Record updated successfully");
            }
            else
            {
                _output.WriteError("Failed to update record.");
            }
        }

        private async Task AddNewRecord(int artistId, string name, string field, int recorded, string label, string pressing, string rating, int discs, string media, DateTime bought, decimal cost, string coverName, string review)
        {
            var recordId = await _repository.AddRecordAsync(new Record
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
                CoverName = null,
                Review = review
            });

            if (recordId > 0)
            {
                _output.WriteLine($"Record added successfully with ArtistId: {recordId}");
            }
            else
            {
                _output.WriteError("Failed to add record.");
            }
        }

        private async Task CountTotalRecordsAsync()
        {
            var count = await _repository.CountTotalRecordsAsync();
            _output.WriteLine($"Total Records: {count}");
        }

        private async Task GetRecordByIdAsync(int recordId)
        {
            var record = await _repository.GetRecordByIdAsync(recordId);

            if (record is not null)
            {
                _output.WriteLine($"Id: {record.RecordId} returns: {record.Recorded} - {record.Name} - media: {record.Media}");
            }
            else
            {
                _output.WriteError($"RecordId: {recordId} not found.");
            }
        }

        private async Task GetAllRecordsAsync()
        {
            var records = await _repository.GetAllRecordsAsync();
            foreach (var record in records)
            {
                _output.WriteLine(record.ToString());
            }
        }

        private async Task GetRecordsByArtistIdAsync(int artistId)
        {
            var records = await _repository.GetRecordsByArtistIdAsync(artistId);
            foreach (var record in records)
            {
                _output.WriteLine(record.ToString());
            }
        }
    }
}
