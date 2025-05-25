using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSBMonitor.Models
{
    public class StatQueue : StatEntity
    {
        public string Name { get; init; }

        public override string Title => $"qeueu: {Name}";
           
    }

}
