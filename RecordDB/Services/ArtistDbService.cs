using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordDB.Models;
using RecordDB.Repositories;
using RecordDB.Services.Output;

namespace RecordDB.Services
{
    public class ArtistDbService
    {
        private readonly IArtistRepository _repository;
        private readonly IOutputService _output;

        public ArtistDbService(IArtistRepository repository, IOutputService output)
        {
            _repository = repository;
            _output = output;
        }

        public async Task RunAllDatabaseOperations()
        {
            // await FixBandArtistsAsync();
            // await DisplayAllArtistsAsync();
            // await GetArtistByIdAsync(114);
            // await CountArtistsAsync();
            // await GetArtistListAsync();
            // await AddArtistAsync();
            // await AddArtistWithoutFirstNameAsync();
            // await AddArtistFromFieldsAsync();
            // await CheckForArtistNameAsync("Alan Robson");
            // await DeleteArtistAsync(860);
            // await DeleteArtistAsync("Alan Robson");
            // await GetArtistByFirstLastNameAsync("Bob", "Dylan");
            // await GetArtistByNameAsync("Bob Dylan");
            // await GetArtistIdByNameAsync("Bob", "Dylan");
            // await GetArtistIdFromRecordAsync(5249);
        }

        private async Task GetArtistIdFromRecordAsync(int recordId)
        {
            var artistId = await _repository.GetArtistIdFromRecordAsync(recordId);
            if (artistId > 0)
            {
                _output.WriteLine($"ArtistId: {artistId} found for RecordId: {recordId}");
            }
            else
            {
                _output.WriteError($"ArtistId not found for RecordId: {recordId}");
            }
        }

        private async Task GetArtistIdByNameAsync(string firstName, string lastName)
        {
            var artistId = await _repository.GetArtistIdAsync(firstName, lastName);
            if (artistId > 0)
            {
                _output.WriteLine($"ArtistId: {artistId} found for {firstName} {lastName}");
            }
            else
            {
                _output.WriteError($"ArtistId not found for {firstName} {lastName}");
            }
        }

        private async Task GetArtistByNameAsync(string name)
        {
            var artist = await _repository.GetArtistByNameAsync(name);
            if (artist is not null)
            {
                _output.WriteLine($"Artist: {artist.Name} found with Id: {artist.ArtistId}");
            }
            else
            {
                _output.WriteError($"Artist with name {name} not found.");
            }
        }

        private async Task CheckForArtistNameAsync(string name)
        {
            var result = await _repository.CheckForArtistNameAsync(name);
            if (result)
            {
                _output.WriteLine($"Artist {name} exists in the database.");
            }
            else
            {
                _output.WriteError($"Artist {name} does not exist in the database.");
            }
        }

        private async Task AddArtistAsync()
        {
            var artist = new Artist
            {
                FirstName = "Alan",
                LastName = "Robson",
                Biography = "Alan is a drone music star."
            };

            var result = await _repository.AddArtistAsync(artist);

            if (result)
            {
                _output.WriteLine($"Artist {artist.FirstName} {artist.LastName} added successfully.");
            }
            else
            {
                _output.WriteError($"Failed to add artist {artist.FirstName} {artist.LastName}.");
            }
        }

        private async Task AddArtistWithoutFirstNameAsync()
        {
            var firstName = string.Empty;
            var lastName = "The Bumbles";
            var biography = "The Bumbles are a Folk music act.";

            var result = await _repository.AddArtistAsync(firstName, lastName, biography);

            if (result)
            {
                _output.WriteLine($"Artist {firstName} {lastName} added successfully.");
            }
            else
            {
                _output.WriteError($"Failed to add artist {firstName} {lastName}.");
            }
        }

        private async Task AddArtistFromFieldsAsync()
        {
            var firstName = "Andrew";
            var lastName = "Robson";
            var biography = "Andrew is a Jazz crooner.";

            var result = await _repository.AddArtistAsync(firstName, lastName, biography);

            if (result)
            {
                _output.WriteLine($"Artist {firstName} {lastName} added successfully.");
            }
            else
            {
                _output.WriteError($"Failed to add artist {firstName} {lastName}.");
            }
        }

        private async Task DeleteArtistAsync(string name)
        {
            bool deleted = await _repository.DeleteArtistAsync(name);

            if (deleted)
            {
                _output.WriteLine($"Successfully deleted artist: {name}");
            }
            else
            {
                _output.WriteError($"Failed to delete artist: {name}");
            }
        }

        private async Task DeleteArtistAsync(int artistId)
        {
            bool deleted = await _repository.DeleteArtistAsync(artistId);

            if (deleted)
            {
                _output.WriteLine($"Successfully deleted artist (ID: {artistId})");
            }
            else
            {
                _output.WriteError($"Failed to delete artist (ID: {artistId})");
            }
        }

        private async Task GetArtistListAsync()
        {
            var artists = await _repository.GetArtistListAsync();
            artists = artists.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToList();

            var artistDictionary = new Dictionary<int, string>
            {
                { 0, "Select an artist" }
            };

            foreach (var artist in artists)
            {
                if (string.IsNullOrEmpty(artist.FirstName))
                {
                    artistDictionary.Add(artist.ArtistId, artist.LastName);
                }
                else
                {
                    artistDictionary.Add(artist.ArtistId, $"{artist.LastName}, {artist.FirstName}");
                }
            }

            foreach (var artist in artistDictionary)
            {
                _output.WriteLine($"Id: {artist.Key}, Name: {artist.Value}");
            }
        }

        private async Task GetArtistByFirstLastNameAsync(string firstName, string lastName)
        {
            var artist = await _repository.GetArtistByFirstLastNameAsync(firstName, lastName);
            if (artist is not null)
            {
                _output.WriteLine($"Artist: {artist.Name} found with Id: {artist.ArtistId}");
            }
            else
            {
                _output.WriteError($"Artist with name {firstName} {lastName} not found.");
            }
        }

        public async Task FixBandArtistsAsync()
        {
            var artists = await _repository.GetBandArtistsAsync();
            foreach (var artist in artists)
            {
                var firstName = string.Empty;
                artist.FirstName = firstName;
                artist.LastName = $"The {artist.LastName}";
                artist.Name = $"{artist.LastName}";

                _output.WriteLine($"Id: {artist.ArtistId}, FirstName: {artist.FirstName}, LastName: {artist.LastName}, Name: {artist.LastName}");
            }

            foreach (var artist in artists)
            {
                bool updated = await _repository.UpdateArtistsBandTitlesAsync(artist);

                if (!updated)
                {
                    _output.WriteError($"ArtistId: {artist.ArtistId}, Name: {artist.Name} not Updated!");
                }
            }
        }

        private async Task DisplayAllArtistsAsync()
        {
            var artists = await _repository.GetArtistListAsync();
            foreach (var artist in artists)
            {
                _output.WriteLine($"Id: {artist.ArtistId}, Name: {artist.Name}");
            }
        }

        private async Task GetArtistByIdAsync(int artistId)
        {
            var artist = await _repository.GetArtistByIdAsync(artistId);

            if (artist is not null)
            {
                _output.WriteLine($"Id: {artist.ArtistId} returns: {artist.Name}");
            }
            else
            {
                _output.WriteError($"ArtistId: {artistId} not found.");
            }
        }

        private async Task CountArtistsAsync()
        {
            var count = await _repository.CountArtistsAsync();
            _output.WriteLine($"Total Artists: {count}");
        }

        // Return Artist as Html - RecordEfConsole
        private string ToHtml(Artist artist)
        {
            var artistDetails = new StringBuilder();

            artistDetails.Append($"<p><strong>ArtistId: </strong>{artist.ArtistId}</p>\n");

            if (!string.IsNullOrEmpty(artist.FirstName))
            {
                artistDetails.Append($"<p><strong>First Name: </strong>{artist.FirstName}</p>\n");
            }

            artistDetails.Append($"<p><strong>Last Name: </strong>{artist.LastName}</p>\n");

            if (!string.IsNullOrEmpty(artist.Name))
            {
                artistDetails.Append($"<p><strong>Name: </strong>{artist.Name}</p>\n");
            }

            if (!string.IsNullOrEmpty(artist.Biography))
            {
                artistDetails.Append($"<p><strong>Biography: </strong></p>\n<div>\n{artist.Biography}\n</div>\n");
            }

            return artistDetails.ToString();
        }
    }
}
