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
                _output.WriteLine("Statistics:");
                _output.WriteLine($"Rock Discs: {statistics.RockDisks}");
                _output.WriteLine($"Folk Discs: {statistics.FolkDisks}");
                _output.WriteLine($"Acoustic Discs: {statistics.AcousticDisks}");
                _output.WriteLine($"Jazz Discs: {statistics.JazzDisks}");
                _output.WriteLine($"Blues Discs: {statistics.BluesDisks}");
                _output.WriteLine($"Country Discs: {statistics.CountryDisks}");
                _output.WriteLine($"Classical Discs: {statistics.ClassicalDisks}");
                _output.WriteLine($"Soundtrack Discs: {statistics.SoundtrackDisks}");
                _output.WriteLine($"Four Star Discs: {statistics.FourStarDisks}");
                _output.WriteLine($"Three Star Discs: {statistics.ThreeStarDisks}");
                _output.WriteLine($"Two Star Discs: {statistics.TwoStarDisks}");
                _output.WriteLine($"One Star Discs: {statistics.OneStarDisks}");
                _output.WriteLine($"Total Cost of Records and CDs: {statistics.TotalCost:C}");
                _output.WriteLine($"Total CDs: {statistics.TotalCDs}");
                _output.WriteLine($"Total CD Cost: {statistics.CDCost:C}");
                _output.WriteLine($"Average CD Cost: {statistics.AvCDCost:C}");
                _output.WriteLine($"Total Records: {statistics.TotalRecords}");
                _output.WriteLine($"Total Record Cost: {statistics.RecordCost:C}");
                _output.WriteLine($"Cost in 2022: {statistics.Cost2022:C}, Discs in 2022: {statistics.Disks2022}, Average Cost in 2022: {statistics.Av2022:C}");
                _output.WriteLine($"Cost in 2021: {statistics.Cost2021:C}, Discs in 2021: {statistics.Disks2021}, Average Cost in 2021: {statistics.Av2021:C}");
                _output.WriteLine($"Cost in 2020: {statistics.Cost2020:C}, Discs in 2020: {statistics.Disks2020}, Average Cost in 2020: {statistics.Av2020:C}");
                _output.WriteLine($"Cost in 2019: {statistics.Cost2019:C}, Discs in 2019: {statistics.Disks2019}, Average Cost in 2019: {statistics.Av2019:C}");
                _output.WriteLine($"Cost in 2018: {statistics.Cost2018:C}, Discs in 2018: {statistics.Disks2018}, Average Cost in 2018: {statistics.Av2018:C}");
                _output.WriteLine($"Cost in 2017: {statistics.Cost2017:C}, Discs in 2017: {statistics.Disks2017}, Average Cost in 2017: {statistics.Av2017:C}");
            }
        }
    }
}
