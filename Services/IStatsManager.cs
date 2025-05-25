using AzureSBMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSBMonitor.Services
{
    public interface IStatsManager
    {
        public Task<IEnumerable<StatEntity>> GetStats(string connectionString, CancellationToken cancellationToken);
    }
}
