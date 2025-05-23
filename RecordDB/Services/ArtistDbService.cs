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
            await GetAllArtistsAsync();
            // await DisplayAllArtistsAsync();
            // await GetArtistByIdAsync(114);
            // await CountArtistsAsync();
            // await GetArtistListAsync();
            // await AddArtistAsync();
            // await AddArtistWithoutFirstNameAsync();
            // await AddArtistFromFieldsAsync();
            // await CheckForArtistNameAsync("Charley Robson");
            // await DeleteArtistAsync(892);
            // await DeleteArtistAsync("Andrew Robson");
            // await GetArtistByFirstLastNameAsync("Bob", "Dylan");
            // await GetArtistByNameAsync("Bob Dylan");
            // await GetArtistIdByNameAsync("Bob", "Dylan");
            // await GetArtistIdFromRecordAsync(5249);
            // await GetArtistsWithNoBioAsync();
            // await GetBiographyAsync(114);
            // await GetNoBiographyCountAsync();
            // await UpdateArtistAsync(861, "Charles", "Robson", "Charles Robson", "Charles is a Jazz music star.");
            // await UpdateArtistAsync();
            // await GetBiographyFromRecordIdAsync(5249);
            // await GetArtistNameFromRecordIdAsync(5249);
            // await ShowArtistAsync(114);
            // await GetArtistNameAsync(114);
        }

        private async Task GetAllArtistsAsync()
        {

            var artists = await _repository.GetArtistListAsync();
            foreach (var artist in artists)
            {
                var biography = string.Empty;
                if (artist.Biography != null && artist.Biography.Length > 60)
                {
                    biography = artist.Biography.Substring(0, 60);
                }

                _output.WriteLine($"Id: {artist.ArtistId}, Name: {artist.Name} - {biography}");
            }
        }

        private async Task GetArtistNameAsync(int artistId)
        {
            var artistName = await _repository.GetArtistNameAsync(artistId);
            if (!string.IsNullOrEmpty(artistName))
            {
                _output.WriteLine($"Artist Name: {artistName}");
            }
            else
            {
                _output.WriteError($"No artist found for ArtistId: {artistId}");
            }
        }

        private async Task ShowArtistAsync(int artistId)
        {
            var artist = await _repository.ShowArtistAsync(artistId);
            if (artist is not null)
            {
                var artistHtml = ToHtml(artist);
                _output.WriteLine(artistHtml);
            }
            else
            {
                _output.WriteError($"ArtistId: {artistId} not found.");
            }
        }

        private async Task GetArtistNameFromRecordIdAsync(int recordId)
        {
            var artistName = await _repository.GetArtistNameByRecordIdAsync(recordId);
            if (!string.IsNullOrEmpty(artistName))
            {
                _output.WriteLine($"Artist Name: {artistName}");
            }
            else
            {
                _output.WriteError($"No artist found for RecordId: {recordId}");
            }
        }

        private async Task GetBiographyFromRecordIdAsync(int recordId)
        {
            var biography = await _repository.GetBiographyFromRecordIdAsync(recordId);
            if (!string.IsNullOrEmpty(biography))
            {
                _output.WriteLine($"Biography: {biography}");
            }
            else
            {
                _output.WriteError($"No biography found for RecordId: {recordId}");
            }
        }

        private async Task UpdateArtistAsync()
        {
            var artistId = 887;
            var firstName = "Charley";
            var lastName = "Robson";
            var name = "Charley Robson";
            var biography = "Charley is a Rock music star.";

            var updated = await _repository.UpdateArtistAsync(artistId, firstName, lastName, name, biography);
            if (updated > 0)
            {
                _output.WriteLine($"Artist {name} updated successfully.");
            }
            else
            {
                _output.WriteError($"Failed to update artist {name}.");
            }
        }

        private async Task UpdateArtistAsync(int artistId, string firstName, string lastName, string name, string biography)
        {
            var artist = new Artist
            {
                ArtistId = artistId,
                FirstName = firstName,
                LastName = lastName,
                Name = name,
                Biography = biography
            };
            int updated = await _repository.UpdateArtistAsync(artist);
            if (updated > 0)
            {
                _output.WriteLine($"Artist {artist.Name} updated successfully.");
            }
            else
            {
                _output.WriteError($"Failed to update artist {artist.Name}.");
            }
        }

        private async Task GetNoBiographyCountAsync()
        {
            int count = await _repository.GetNoBiographyCountAsync();
            _output.WriteLine($"Total Artists with no biography: {count}");
        }

        private async Task GetBiographyAsync(int artistId)
        {
            var artist = await _repository.GetBiographyAsync(artistId);
            if (artist is not null)
            {
                var biography = ToHtml(artist);
                _output.WriteLine(biography);
            }
            else
            {
                _output.WriteError($"ArtistId: {artistId} not found.");
            }
        }

        private async Task GetArtistsWithNoBioAsync()
        {
            var artists = await _repository.GetArtistsWithNoBioAsync();
            
            _output.WriteLine($"Artists with no biography: {artists.Count()}");

            foreach (var artist in artists)
            {
                _output.WriteLine($"Id: {artist.ArtistId}, Name: {artist.Name}");
            }
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
                FirstName = "Andrew",
                LastName = "Robson",
                Name = string.Empty,
                Biography = "Andrew is a Country & Western star."
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
            var firstName = "Ethan";
            var lastName = "Robson";
            var name = string.Empty;
            var biography = "Ethan is a Jazz crooner.";

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
            var deleted = await _repository.DeleteArtistAsync(artistId);
            
            // This produces an error but deletes the record.
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
