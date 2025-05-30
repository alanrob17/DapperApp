﻿using Microsoft.VisualBasic.FileIO;
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
            await GetAllRecordsAsync();
            // await GetRecordByIdAsync(1076);
            // await CountTotalRecordsAsync();
            // await GetTotalCostAsync();
            // await GetTotalCdCostAsync();
            // await GetRecordsByArtistIdAsync(114);
            // await AddNewRecord();
            // await AddNewRecord(893, "Hip-Hop TipTop", "Rock", 2025, "Wobble Dobble Music", "Aus", "***", 1, "CD", DateTime.Now, 19.99m, "", "This is Charlie's second album.");
            // await DeleteRecordAsync(5294);
            // await UpdateRecordAsync();
            // await UpdateRecordAsync(5291, "Rockin' The Boogie Bass Again", "Rock", 2023, "Wibble Wobble Music", "Aus", "***", 1, "CD", DateTime.Now, 19.99m, "", "This is Charlies's second album.");
            // await GetArtistRecordsAsync(114);
            // await GetNoRecordReviewsAsync();
            // await CountDiscsAsync("All");
            // await GetArtistNumberOfRecordsAsync(114);
            // await GetRecordByNameAsync("Doggo");
            // await GetRecordsByNameAsync("Bringing");
            // await GetArtistNameFromRecordAsync(3232);
            // await GetRecordNumberByYearAsync(1974);
            // await GetTotalNumberOfCDsAsync();
            // await GetNoReviewCountAsync();
            // await GetBoughtDiscCountForYearAsync(2022);
            // await GetTotalCostForYearAsync(2017);
            // await GetTotalNumberOfDiscsAsync();
            // await GetRecordDetailsAsync(3232);
            // await GetTotalArtistCostAsync();
            // await GetTotalArtistDiscsAsync();
            // await GetRecordListbyArtistAsync(114);
            // await GetRecordHtmlAsync(3232);
            // await GetDiscCountForYearAsync(1974);
            // await GetArtistRecordListAsync();
            // await GetTotalNumberOfRecordsAsync();
            // await GetTotalNumberOfBluraysAsync();
            // await GetTotalNumberOfDVDsAsync();
        }

        private async Task GetTotalCdCostAsync()
        {
            var totalCost = await _repository.GetTotalCdCostAsync();
            if (totalCost > 0)
            {
                await _output.WriteLineAsync($"Total cost of all CDs: ${decimal.Round(totalCost, 2)}");
            }
            else
            {
                await _output.WriteErrorAsync("No CDs found.");
            }
        }

        private async Task GetTotalNumberOfDVDsAsync()
        {
            var total = await _repository.GetTotalNumberOfDVDsAsync();
            if (total > 0)
            {
                await _output.WriteLineAsync($"Total number of DVDs: {total}");
            }
            else
            {
                await _output.WriteErrorAsync("No DVDs found.");
            }
        }

        private async Task GetTotalNumberOfBluraysAsync()
        {
            var total = await _repository.GetTotalNumberOfBluraysAsync();
            if (total > 0)
            {
                await _output.WriteLineAsync($"Total number of Blurays: {total}");
            }
            else
            {
                await _output.WriteErrorAsync("No Blurays found.");
            }
        }

        private async Task GetTotalNumberOfRecordsAsync()
        {
            var total = await _repository.GetTotalNumberOfRecordsAsync();
            if (total > 0)
            {
                await _output.WriteLineAsync($"Total number of records: {total}");
            }
            else
            {
                await _output.WriteErrorAsync("No records found.");
            }
        }

        private async Task GetTotalCostAsync()
        {
            var totalCost = await _repository.GetTotalCostAsync();
            if (totalCost > 0)
            {
                await _output.WriteLineAsync($"Total cost of all discs: ${decimal.Round(totalCost, 2)}");
            }
            else
            {
                await _output.WriteErrorAsync("No discs found.");
            }
        }

        private async Task GetArtistRecordListAsync()
        {
            var records = await _repository.GetArtistRecordListAsync();
            foreach (var record in records)
            {
                await _output.WriteLineAsync($"ArtistId: {record.ArtistId}, Artist: {record.ArtistName} - Record Id: {record.RecordId}, Name: {record.Name}, Recorded: {record.Recorded}, Media: {record.Media}");
            }
        }

        private async Task GetRecordHtmlAsync(int recordId)
        {
            ArtistRecord record = await _repository.GetRecordHtmlAsync(recordId);
            if (record is not null)
            {
                string recordHtml = RecordHtml(record);
                await _output.WriteLineAsync(recordHtml);
            }
            else
            {
                await _output.WriteErrorAsync($"Record with ID {recordId} not found.");
            }
        }

        private async Task GetRecordListbyArtistAsync(int artistId)
        {

            var records = await _repository.GetRecordListByArtistAsync(artistId);

            var recordDictionary = new Dictionary<int, string>();

            foreach (var record in records)
            {
                recordDictionary.Add(record.RecordId, record.Name);
            }

            foreach (var record in recordDictionary)
            {
                await _output.WriteLineAsync($"Id: {record.Key}, Name: {record.Value}");
            }
        }

        private async Task GetTotalArtistDiscsAsync()
        {
            var totals = await _repository.GetTotalArtistDiscsAsync();
            foreach (var total in totals)
            {
                await _output.WriteLineAsync($"ArtistId: {total.ArtistId}, Artist: {total.Name} - Total Discs: {total.TotalDiscs}");
            }
        }

        private async Task GetTotalArtistCostAsync()
        {
            var totals = await _repository.GetTotalArtistCostAsync();
            foreach (var total in totals)
            {
                await _output.WriteLineAsync($"ArtistId: {total.ArtistId}, Artist: {total.Name} - Total Discs: {total.TotalDiscs} - Total Cost: {total.TotalCost}");
            }
        }

        private async Task GetRecordDetailsAsync(int recordId)
        {

            var record = await _repository.GetRecordDetailsAsync(recordId);
            if (record is not null)
            {
                await _output.WriteLineAsync($"ArtistId: {record.ArtistId}, Artist: {record.ArtistName} -- Record Id: {record.RecordId}, Name: {record.Name}, Recorded: {record.Recorded}, Media: {record.Media}");
            }
            else
            {
                await _output.WriteErrorAsync($"Record with ID {recordId} not found.");
            }
        }

        private async Task GetTotalNumberOfDiscsAsync()
        {
            var total = await _repository.GetTotalNumberOfDiscsAsync();
            if (total > 0)
            {
                await _output.WriteLineAsync($"Total number of discs: {total}");
            }
            else
            {
                await _output.WriteErrorAsync("No discs found.");
            }
        }

        private async Task GetBoughtDiscCountForYearAsync(int year)
        {
            var count = await _repository.GetBoughtDiscCountForYearAsync(year);
            if (count > 0)
            {
                await _output.WriteLineAsync($"Total number of bought discs for year {year}: {count}");
            }
            else
            {
                await _output.WriteErrorAsync($"No bought discs found for year: {year}");
            }
        }

        private async Task GetTotalCostForYearAsync(int year)
        {
            var totalCost = await _repository.GetTotalCostForYearAsync(year);
            if (totalCost > 0)
            {
                await _output.WriteLineAsync($"Total cost of records bought in {year}: ${decimal.Round(totalCost, 2)}");
            }
            else
            {
                await _output.WriteErrorAsync($"No records found for year: {year}");
            }
        }


        private async Task GetDiscCountForYearAsync(int year)
        {
            var count = await _repository.GetDiscCountForYearAsync(year);
            if (count > 0)
            {
                await _output.WriteLineAsync($"Total number of discs for year {year}: {count}");
            }
            else
            {
                await _output.WriteErrorAsync($"No discs found for year: {year}");
            }
        }

        private async Task GetNoReviewCountAsync()
        {
            var count = await _repository.GetNoReviewCountAsync();
            if (count > 0)
            {
                await _output.WriteLineAsync($"Total number of records with no reviews: {count}");
            }
            else
            {
                await _output.WriteErrorAsync("No records found with no reviews.");
            }
        }

        private async Task GetTotalNumberOfCDsAsync()
        {
            var total = await _repository.GetTotalNumberOfCDsAsync();
            if (total > 0)
            {
                await _output.WriteLineAsync($"Total number of CD's: {total}");
            }
            else
            {
                await _output.WriteErrorAsync("No CD's found.");
            }
        }

        private async Task GetRecordNumberByYearAsync(int year)
        {
            var records = await _repository.GetRecordNumberByYearAsync(year);
            if (records > 0)
            {
                await _output.WriteLineAsync($"Total records for year {year}: {records}");
            }
            else
            {
                await _output.WriteErrorAsync($"No records found for year: {year}");
            }
        }

        private async Task GetArtistNameFromRecordAsync(int recordId)
        {
            var artistName = await _repository.GetArtistNameFromRecordAsync(recordId);
            if (!string.IsNullOrEmpty(artistName))
            {
                await _output.WriteLineAsync($"Artist Name: {artistName}");
            }
            else
            {
                await _output.WriteErrorAsync($"No artist found for record ID: {recordId}");
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
                    await _output.WriteLineAsync($"Artist: {artistName} - Record: {record.Name} - {record.Recorded}");
                }
                else
                {
                    await _output.WriteErrorAsync($"No artist found for record: {name}");
                }
            }
            else
            {
                await _output.WriteErrorAsync($"Record: {name} not found.");
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
                        await _output.WriteLineAsync($"Artist: {artistName} - Record: {record.Name} - {record.Recorded}");
                    }
                    else
                    {
                        await _output.WriteErrorAsync($"No artist found for record: {name}");
                    }
                }
            }
            else
            {
                await _output.WriteErrorAsync($"No records found for name: {name}");
            }
        }

        private async Task GetArtistNumberOfRecordsAsync(int artistId)
        {
            var records = await _repository.GetArtistNumberOfRecordsAsync(artistId);
            if (records is not null)
            {
                await _output.WriteLineAsync($"ArtistId: {artistId} - Number of Records: {records}");
            }
            else
            {
                await _output.WriteErrorAsync($"No records found for ArtistId: {artistId}");
            }
        }

        private async Task CountDiscsAsync(string show)
        {
            var discs = await _repository.CountDiscsAsync(show);
            await _output.WriteLineAsync($"{show}: Total Discs: {discs}");
        }

        private async Task GetNoRecordReviewsAsync()
        {
            var records = await _repository.NoRecordReviewsAsync();
            foreach (var record in records)
            {
                await _output.WriteLineAsync($"ArtistId: {record.ArtistId} - Artist: {record.ArtistName} - Record Id: {record.RecordId}: {record.Recorded} - {record.RecordName}");
            }
        }

        private async Task GetArtistRecordsAsync(int artistId)
        {

            var records = await _repository.GetArtistRecordsAsync(artistId);
            foreach (var record in records)
            {
                await _output.WriteLineAsync($"Record Id: {record.RecordId}, Name: {record.Name}, Recorded: {record.Recorded}, Media: {record.Media}");
            }
        }

        private async Task UpdateRecordAsync(int recordId, string name, string field, int recorded, string label, string pressing, string rating, int discs, string media, DateTime bought, decimal cost, string coverName, string review)
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
            
            var rowsAffected = await _repository.UpdateRecordAsync(record);
            if (rowsAffected > 0)
            {
                await _output.WriteLineAsync($"Record Id: {recordId} updated successfully.");
            }
            else
            {
                await _output.WriteErrorAsync($"Failed to update record with Id: {recordId}.");
            }
        }

        private async Task UpdateRecordAsync()
        {
            var record = new Record
            {
                RecordId = 5294,
                ArtistId = 0,
                Name = "Uptown Guitar Rebellion",
                Field = "Rock",
                Recorded = 2023,
                Label = "Wobble Music",
                Pressing = "Aus",
                Rating = "***",
                Discs = 1,
                Media = "CD",
                Bought = DateTime.Now,
                Cost = 19.99m,
                CoverName = "",
                Review = "This is Andrew's sixth album."
            };
            var rowsAffected = await _repository.UpdateRecordAsync(record);
            if (rowsAffected > 0)
            {
                await _output.WriteLineAsync($"Record with ID {record.RecordId} updated successfully.");
            }
            else
            {
                await _output.WriteErrorAsync($"Failed to update record with ID {record.RecordId}.");
            }
        }

        private async Task DeleteRecordAsync(int recordId)
        {
            var result = await _repository.DeleteRecordAsync(recordId);
            if (result)
            {
                await _output.WriteLineAsync($"Record with ID {recordId} deleted successfully.");
            }
            else
            {
                await _output.WriteErrorAsync($"Failed to delete record with ID {recordId}.");
            }
        }

        private async Task AddNewRecord()
        {
            var recordId = await _repository.AddRecordAsync(new Record
            {
                ArtistId = 893,
                Name = "Hip Hop Extroadinaire!",
                Field = "Jazz",
                Recorded = 2024,
                Label = "Wobble Dobble Music",
                Pressing = "Ger",
                Rating = "****",
                Discs = 1,
                Media = "CD",
                Bought = DateTime.Now,
                Cost = 10.99m,
                CoverName = "",
                Review = "This is Charlie's first album."
            });

            if (recordId > 0)
            {
                await _output.WriteLineAsync($"Record with Id: {recordId} added successfully");
            }
            else
            {
                await _output.WriteErrorAsync("Failed to add record!");
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
                CoverName = "",
                Review = review
            });

            if (recordId > 0)
            {
                await _output.WriteLineAsync($"Record added successfully with RecordId: {recordId}");
            }
            else
            {
                await _output.WriteErrorAsync("Failed to add record.");
            }
        }

        private async Task CountTotalRecordsAsync()
        {
            var count = await _repository.CountTotalRecordsAsync();
            await _output.WriteLineAsync($"Total Records: {count}");
        }

        private async Task GetRecordByIdAsync(int recordId)
        {
            var record = await _repository.GetRecordByIdAsync(recordId);

            if (record is not null)
            {
                await _output.WriteLineAsync($"Id: {record.RecordId} returns: {record.Recorded} - {record.Name} - media: {record.Media}");
            }
            else
            {
                await _output.WriteErrorAsync($"RecordId: {recordId} not found.");
            }
        }

        private async Task GetAllRecordsAsync()
        {
            var records = await _repository.GetAllRecordsAsync();
            foreach (var record in records)
            {
                await _output.WriteLineAsync(record.ToString());
            }
        }

        private async Task GetRecordsByArtistIdAsync(int artistId)
        {
            var records = await _repository.GetRecordsByArtistIdAsync(artistId);
            foreach (var record in records)
            {
                await _output.WriteLineAsync(record.ToString());
            }
        }

        private string RecordHtml(ArtistRecord record)
        {
            var bought = record.Bought.HasValue ? record.Bought.Value.ToString("dd-MM-yyyy") : "unk";
            var recordHtml = $@"
                <h1>{record.Name}</h1>
                <h2>Artist: {record.ArtistName}</h2>
                <p>Field: {record.Field}</p>
                <p>Recorded: {record.Recorded}</p>
                <p>Label: {record.Label}</p>
                <p>Pressing: {record.Pressing}</p>
                <p>Rating: {record.Rating}</p>
                <p>Discs: {record.Discs}</p>
                <p>Media: {record.Media}</p>
                <p>Bought: {bought}</p>
                <p>Cost: {record.Cost:C}</p>
                <p>Review: {record.Review}</p>";
            return recordHtml;
        }
    }
}
