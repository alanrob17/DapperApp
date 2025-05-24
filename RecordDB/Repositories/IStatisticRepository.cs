using RecordDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordDB.Repositories
{
    public interface IStatisticRepository
    {
        Task<Statistic> GetStatistics();
    }
}
