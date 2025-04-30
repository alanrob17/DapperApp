using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            await FixBandArtistsAsync();
            await DisplayAllArtistsAsync();
            await GetArtistByIdAsync(114);
            await CountArtistsAsync();
            // Add more operations here
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
                bool updated = await _repository.UpdateArtistAsync(artist);

                if (!updated)
                {
                    _output.WriteError($"ArtistId: {artist.ArtistId}, Name: {artist.Name} not Updated!");
                }
            }
        }

        private async Task DisplayAllArtistsAsync()
        {
            var artists = await _repository.GetAllArtistsAsync();
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
    }
}
