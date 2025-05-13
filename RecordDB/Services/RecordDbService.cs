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
            await DeleteRecordAsync(5285);
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
                ArtistId = 863,
                Name = "Rockin' The Bass",
                Field = "Rock",
                Recorded = 2025,
                Label = "Wobble Music",
                Pressing = "Aus",
                Rating = "***",
                Discs = 2,
                Media = "CD",
                Bought = DateTime.Now,
                Cost = 19.99m,
                Review = "This is James' third album."
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
