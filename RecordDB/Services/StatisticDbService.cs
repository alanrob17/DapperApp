using RecordDB.Repositories;
using RecordDB.Services.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordDB.Services
{
    public class StatisticDbService
    {
        private readonly IStatisticRepository _repository;
        private readonly IOutputService _output;

        public StatisticDbService(IStatisticRepository repository, IOutputService output)
        {
            _repository = repository;
            _output = output;
        }
        public async Task RunAllDatabaseOperations()
        {
            await DisplayStatistics();
        }

        public async Task DisplayStatistics()
        {
            var statistics = await _repository.GetStatistics();

            if (statistics != null)
            {
                await _output.WriteLineAsync("Statistics:");
                await _output.WriteLineAsync($"Rock Discs: {statistics.RockDisks}");
                await _output.WriteLineAsync($"Folk Discs: {statistics.FolkDisks}");
                await _output.WriteLineAsync($"Acoustic Discs: {statistics.AcousticDisks}");
                await _output.WriteLineAsync($"Jazz Discs: {statistics.JazzDisks}");
                await _output.WriteLineAsync($"Blues Discs: {statistics.BluesDisks}");
                await _output.WriteLineAsync($"Country Discs: {statistics.CountryDisks}");
                await _output.WriteLineAsync($"Classical Discs: {statistics.ClassicalDisks}");
                await _output.WriteLineAsync($"Soundtrack Discs: {statistics.SoundtrackDisks}");
                await _output.WriteLineAsync($"Four Star Discs: {statistics.FourStarDisks}");
                await _output.WriteLineAsync($"Three Star Discs: {statistics.ThreeStarDisks}");
                await _output.WriteLineAsync($"Two Star Discs: {statistics.TwoStarDisks}");
                await _output.WriteLineAsync($"One Star Discs: {statistics.OneStarDisks}");
                await _output.WriteLineAsync($"Total Cost of Records and CDs: {statistics.TotalCost:C}");
                await _output.WriteLineAsync($"Total CDs: {statistics.TotalCDs}");
                await _output.WriteLineAsync($"Total CD Cost: {statistics.CDCost:C}");
                await _output.WriteLineAsync($"Average CD Cost: {statistics.AvCDCost:C}");
                await _output.WriteLineAsync($"Total Records: {statistics.TotalRecords}");
                await _output.WriteLineAsync($"Total Record Cost: {statistics.RecordCost:C}");
                await _output.WriteLineAsync($"Cost in 2022: {statistics.Cost2022:C}, Discs in 2022: {statistics.Disks2022}, Average Cost in 2022: {statistics.Av2022:C}");
                await _output.WriteLineAsync($"Cost in 2021: {statistics.Cost2021:C}, Discs in 2021: {statistics.Disks2021}, Average Cost in 2021: {statistics.Av2021:C}");
                await _output.WriteLineAsync($"Cost in 2020: {statistics.Cost2020:C}, Discs in 2020: {statistics.Disks2020}, Average Cost in 2020: {statistics.Av2020:C}");
                await _output.WriteLineAsync($"Cost in 2019: {statistics.Cost2019:C}, Discs in 2019: {statistics.Disks2019}, Average Cost in 2019: {statistics.Av2019:C}");
                await _output.WriteLineAsync($"Cost in 2018: {statistics.Cost2018:C}, Discs in 2018: {statistics.Disks2018}, Average Cost in 2018: {statistics.Av2018:C}");
                await _output.WriteLineAsync($"Cost in 2017: {statistics.Cost2017:C}, Discs in 2017: {statistics.Disks2017}, Average Cost in 2017: {statistics.Av2017:C}");
            }
        }
    }
}
