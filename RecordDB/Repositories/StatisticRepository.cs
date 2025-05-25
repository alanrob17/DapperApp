using RecordDB.Data.Interfaces;
using RecordDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordDB.Repositories
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly IDataAccess _db;
        private readonly IRecordRepository _record;

        public StatisticRepository(IDataAccess db, IRecordRepository record)
        {
            _db = db;
            _record = record;
        }

        public async Task<Statistic> GetStatistics()
        {
            Statistic statistics = new Statistic()
            {
                RockDisks = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where field = 'Rock'"),
                FolkDisks = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where field = 'Folk'"),
                AcousticDisks = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where field = 'Acoustic'"),
                JazzDisks = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where field = 'Jazz' or field = 'Fusion'"),
                BluesDisks = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where field = 'Blues'"),
                CountryDisks = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where field = 'Country'"),
                ClassicalDisks = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where field = 'Classical'"),
                SoundtrackDisks = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where field = 'Soundtrack'"),
                FourStarDisks = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where rating = '****'"),
                ThreeStarDisks = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where rating = '***'"),
                TwoStarDisks = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where rating = '**'"),
                OneStarDisks = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where rating = '*'"),
                RecordCost = await _db.GetCostQueryAsync("select sum(cost) from record where media = 'R'"),
                TotalRecords = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where media = 'R'"),
                CDCost = await _db.GetCostQueryAsync("select sum(cost) from record where media = 'CD'"),
                TotalCDs = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where media = 'CD'"),
                AvCDCost = await _record.GetAverageCdCostAsync(),
                TotalCost = await _record.GetTotalCostAsync(),
                Cost2022 = await _db.GetCostQueryAsync("select sum(cost) from record where bought > '31-Dec-2021' and bought < '01-Jan-2023'"),
                Disks2022 = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where bought > '31-Dec-2021' and bought < '01-Jan-2023'"),
                Av2022 = await _record.GetAverageCostForYearAsync(2022),
                Cost2021 = await _db.GetCostQueryAsync("select sum(cost) from record where bought > '31-Dec-2020' and bought < '01-Jan-2022'"),
                Disks2021 = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where bought > '31-Dec-2020' and bought < '01-Jan-2022'"),
                Av2021 = await _record.GetAverageCostForYearAsync(2021),
                Cost2020 = await _db.GetCostQueryAsync("select sum(cost) from record where bought > '31-Dec-2019' and bought < '01-Jan-2021'"),
                Disks2020 = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where bought > '31-Dec-2019' and bought < '01-Jan-2021'"),
                Av2020 = await _record.GetAverageCostForYearAsync(2020),
                Cost2019 = await _db.GetCostQueryAsync("select sum(cost) from record where bought > '31-Dec-2018' and bought < '01-Jan-2020'"),
                Disks2019 = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where bought > '31-Dec-2018' and bought < '01-Jan-2020'"),
                Av2019 = await _record.GetAverageCostForYearAsync(2019),
                Cost2018 = await _db.GetCostQueryAsync("select sum(cost) from record where  bought > '31-Dec-2017' and bought < '01-Jan-2019'"),
                Disks2018 = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where bought > '31-Dec-2017' and bought < '01-Jan-2019'"),
                Av2018 = await _record.GetAverageCostForYearAsync(2018),
                Cost2017 = await _db.GetCostQueryAsync("select sum(cost) from record where bought > '31-Dec-2016' and bought < '01-Jan-2018' "),
                Disks2017 = await _db.GetCountOrIdQueryAsync("select sum(discs) from record where bought > '31-Dec-2016' and bought < '01-Jan-2018'"),
                Av2017 = await _record.GetAverageCostForYearAsync(2017)
            };

            return statistics;
        }
    }
}
