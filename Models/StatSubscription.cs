using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSBMonitor.Models
{
    public class StatSubscription : StatEntity
    {
        public string Name { get; init; }
        public required string SubscrName { get; init; }

        public override string Title =>
            $"topic: {Name}, subscription: {SubscrName}";

    }
}
