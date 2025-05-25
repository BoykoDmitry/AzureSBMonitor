using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSBMonitor.Models
{
    public abstract class StatEntity
    {
        public required long SizeInBytes { get; init; }
        public required long ActiveMessages { get; init; }
        public required long DeadMessages { get; init; }        
        public abstract string Title { get; }
    }
}
