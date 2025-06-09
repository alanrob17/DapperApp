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

        public async Task RunAllDatabaseOperations() =>
            // await GetAllArtistsAsync();
            // await DisplayAllArtistsAsync();
            // await GetArtistByIdAsync(114);
            // await CountArtistsAsync();
            // await GetArtistListAsync();
            // await AddArtistAsync();
            // await AddArtistWithoutFirstNameAsync();
            // await AddArtistFromFieldsAsync();
            // await CheckForArtistNameAsync("Bob Dylan");
            // await DeleteArtistAsync(892);
            // await DeleteArtistAsync("Andrew Robson");
            // await GetArtistByFirstLastNameAsync("Bob", "Dylan");
            // await GetArtistByNameAsync("Bob Dylan");
            // await GetArtistIdByNameAsync("Bob", "Dylan");
            // await GetArtistIdFromRecordAsync(5249);
            // await GetArtistsWithNoBioAsync();
            // await GetBiographyAsync(114);
            await GetNoBiographyCountAsync();
            // await UpdateArtistAsync(896, string.Empty, "The Wompoles", "The Wompoles", "The Wompoles are a Comedy music act.");
            // await UpdateArtistAsync();
            // await GetBiographyFromRecordIdAsync(5249);
            // await GetArtistNameFromRecordIdAsync(5249);
            // await ShowArtistAsync(114);
            // await GetArtistNameAsync(114);

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

                await _output.WriteLineAsync($"Id: {artist.ArtistId}, Name: {artist.Name} - {biography}");
            }
        }

        private async Task GetArtistNameAsync(int artistId)
        {
            var artistName = await _repository.GetArtistNameAsync(artistId);
            if (!string.IsNullOrEmpty(artistName))
            {
                await _output.WriteLineAsync($"Artist Name: {artistName}");
            }
            else
            {
                await _output.WriteErrorAsync($"No artist found for ArtistId: {artistId}");
            }
        }

        private async Task ShowArtistAsync(int artistId)
        {
            var artist = await _repository.ShowArtistAsync(artistId);
            if (artist is not null)
            {
                var artistHtml = ToHtml(artist);
                await _output.WriteLineAsync(artistHtml);
            }
            else
            {
                await _output.WriteErrorAsync($"ArtistId: {artistId} not found.");
            }
        }

        private async Task GetArtistNameFromRecordIdAsync(int recordId)
        {
            var artistName = await _repository.GetArtistNameByRecordIdAsync(recordId);
            if (!string.IsNullOrEmpty(artistName))
            {
                await _output.WriteLineAsync($"Artist Name: {artistName}");
            }
            else
            {
                await _output.WriteErrorAsync($"No artist found for RecordId: {recordId}");
            }
        }

        private async Task GetBiographyFromRecordIdAsync(int recordId)
        {
            var biography = await _repository.GetBiographyFromRecordIdAsync(recordId);
            if (!string.IsNullOrEmpty(biography))
            {
                await _output.WriteLineAsync($"Biography: {biography}");
            }
            else
            {
                await _output.WriteErrorAsync($"No biography found for RecordId: {recordId}");
            }
        }

        private async Task UpdateArtistAsync()
        {
            var artistId = 895;
            var firstName = "Ethan J";
            var lastName = "Robson";
            var name = "Ethan J Robson";
            var biography = "Ethan is a Dub music star.";

            var updated = await _repository.UpdateArtistAsync(artistId, firstName, lastName, name, biography);
            if (updated > 0)
            {
                await _output.WriteLineAsync($"Artist {name} updated successfully.");
            }
            else
            {
                await _output.WriteErrorAsync($"Failed to update artist {name}.");
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
                await _output.WriteLineAsync($"Artist {artist.Name} updated successfully.");
            }
            else
            {
                await _output.WriteErrorAsync($"Failed to update artist {artist.Name}.");
            }
        }

        private async Task GetNoBiographyCountAsync()
        {
            int count = await _repository.GetNoBiographyCountAsync();
            await _output.WriteLineAsync($"Total Artists with no biography: {count}");
        }

        private async Task GetBiographyAsync(int artistId)
        {
            var artist = await _repository.GetBiographyAsync(artistId);
            if (artist is not null)
            {
                var biography = ToHtml(artist);
                await _output.WriteLineAsync(biography);
            }
            else
            {
                await _output.WriteErrorAsync($"ArtistId: {artistId} not found.");
            }
        }

        private async Task GetArtistsWithNoBioAsync()
        {
            var artists = await _repository.GetArtistsWithNoBioAsync();
            
            await _output.WriteLineAsync($"Artists with no biography: {artists.Count()}");

            foreach (var artist in artists)
            {
                await _output.WriteLineAsync($"Id: {artist.ArtistId}, Name: {artist.Name}");
            }
        }

        private async Task GetArtistIdFromRecordAsync(int recordId)
        {
            var artistId = await _repository.GetArtistIdFromRecordAsync(recordId);
            if (artistId > 0)
            {
                await _output.WriteLineAsync($"ArtistId: {artistId} found for RecordId: {recordId}");
            }
            else
            {
                await _output.WriteErrorAsync($"ArtistId not found for RecordId: {recordId}");
            }
        }

        private async Task GetArtistIdByNameAsync(string firstName, string lastName)
        {
            var artistId = await _repository.GetArtistIdAsync(firstName, lastName);
            if (artistId > 0)
            {
                await _output.WriteLineAsync($"ArtistId: {artistId} found for {firstName} {lastName}");
            }
            else
            {
                await _output.WriteErrorAsync($"ArtistId not found for {firstName} {lastName}");
            }
        }

        private async Task GetArtistByNameAsync(string name)
        {
            var artist = await _repository.GetArtistByNameAsync(name);
            if (artist is not null)
            {
                await _output.WriteLineAsync($"Artist: {artist.Name} found with Id: {artist.ArtistId}");
            }
            else
            {
                await _output.WriteErrorAsync($"Artist with name {name} not found.");
            }
        }

        private async Task CheckForArtistNameAsync(string name)
        {
            var result = await _repository.CheckForArtistNameAsync(name);
            if (result)
            {
                await _output.WriteLineAsync($"Artist {name} exists in the database.");
            }
            else
            {
                await _output.WriteErrorAsync($"Artist {name} does not exist in the database.");
            }
        }

        private async Task AddArtistAsync()
        {
            var artist = new Artist
            {
                FirstName = "Alan",
                LastName = "Robson",
                Name = string.Empty,
                Biography = "Alan is a Jazz crooner."
            };

            var result = await _repository.AddArtistAsync(artist);

            if (result)
            {
                await _output.WriteLineAsync($"Artist {artist.FirstName} {artist.LastName} added successfully.");
            }
            else
            {
                await _output.WriteErrorAsync($"Failed to add artist {artist.FirstName} {artist.LastName}.");
            }
        }

        private async Task AddArtistWithoutFirstNameAsync()
        {
            var firstName = string.Empty;
            var lastName = "The Wombats";
            var biography = "The Wombats are a Rock music act.";

            var result = await _repository.AddArtistAsync(firstName, lastName, biography);

            if (result)
            {
                await _output.WriteLineAsync($"Artist {firstName} {lastName} added successfully.");
            }
            else
            {
                await _output.WriteErrorAsync($"Failed to add artist {firstName} {lastName}.");
            }
        }

        private async Task AddArtistFromFieldsAsync()
        {
            var firstName = "Ethan";
            var lastName = "Robson";
            var name = string.Empty;
            var biography = "Ethan is a Hip-Hop star.";

            var result = await _repository.AddArtistAsync(firstName, lastName, biography);

            if (result)
            {
                await _output.WriteLineAsync($"Artist {firstName} {lastName} added successfully.");
            }
            else
            {
                await _output.WriteErrorAsync($"Failed to add artist {firstName} {lastName}.");
            }
        }

        private async Task DeleteArtistAsync(string name)
        {
            bool deleted = await _repository.DeleteArtistAsync(name);

            if (deleted)
            {
                await _output.WriteLineAsync($"Successfully deleted artist: {name}");
            }
            else
            {
                await _output.WriteErrorAsync($"Failed to delete artist: {name}");
            }
        }

        private async Task DeleteArtistAsync(int artistId)
        {
            var deleted = await _repository.DeleteArtistAsync(artistId);
            
            // This produces an error but deletes the record.
            if (deleted)
            {
                await _output.WriteLineAsync($"Successfully deleted artist (ID: {artistId})");
            }
            else
            {
                await _output.WriteErrorAsync($"Failed to delete artist (ID: {artistId})");
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
                await _output.WriteLineAsync($"Id: {artist.Key}, Name: {artist.Value}");
            }
        }

        private async Task GetArtistByFirstLastNameAsync(string firstName, string lastName)
        {
            var artist = await _repository.GetArtistByFirstLastNameAsync(firstName, lastName);
            if (artist is not null)
            {
                await _output.WriteLineAsync($"Artist: {artist.Name} found with Id: {artist.ArtistId}");
            }
            else
            {
                await _output.WriteErrorAsync($"Artist with name {firstName} {lastName} not found.");
            }
        }

        private async Task DisplayAllArtistsAsync()
        {
            var artists = await _repository.GetArtistListAsync();
            foreach (var artist in artists)
            {
                await _output.WriteLineAsync($"Id: {artist.ArtistId}, Name: {artist.Name}");
            }
        }

        private async Task GetArtistByIdAsync(int artistId)
        {
            var artist = await _repository.GetArtistByIdAsync(artistId);

            if (artist is not null)
            {
                await _output.WriteLineAsync($"Id: {artist.ArtistId} returns: {artist.Name}");
            }
            else
            {
                await _output.WriteErrorAsync($"ArtistId: {artistId} not found.");
            }
        }

        private async Task CountArtistsAsync()
        {
            var count = await _repository.CountArtistsAsync();
            await _output.WriteLineAsync($"Total Artists: {count}");
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
