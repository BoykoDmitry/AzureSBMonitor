using Azure.Messaging.ServiceBus.Administration;
using AzureSBMonitor.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSBMonitor.Services
{
    public class StatsManager : IStatsManager
    {
        private readonly ILogger _logger;

        public StatsManager(ILogger<StatsManager> logger)
        {
            _logger = logger;
        }

        private async Task<StatEntity> GetQueueStat(string queueName, ServiceBusAdministrationClient client, CancellationToken cancellationToken)
        {
            var pr = await client.GetQueueRuntimePropertiesAsync(queueName, cancellationToken);

            return new StatQueue()
            {
                ActiveMessages = pr.Value.ActiveMessageCount,
                DeadMessages = pr.Value.DeadLetterMessageCount,
                Name = pr.Value.Name,
                SizeInBytes = pr.Value.SizeInBytes
            };
        }

        private async Task<StatEntity> GetSubscrStat(string topicName, 
            string subscriptionName, 
            ServiceBusAdministrationClient client, 
            long topicSize,
            CancellationToken cancellationToken)
        {
            var pr = await client.GetSubscriptionRuntimePropertiesAsync(topicName, subscriptionName, cancellationToken);

            return new StatSubscription()
            {
                ActiveMessages = pr.Value.ActiveMessageCount,
                DeadMessages = pr.Value.DeadLetterMessageCount,
                Name = pr.Value.TopicName,
                SubscrName = pr.Value.SubscriptionName,
                SizeInBytes = topicSize
            };
        }

        public async Task<IEnumerable<StatEntity>> GetStats(string connectionString, CancellationToken cancellationToken)
        {
            var client = new ServiceBusAdministrationClient(connectionString);
            var topics = client.GetTopicsAsync(cancellationToken);
            var queues = client.GetQueuesAsync(cancellationToken);

            var list = new List<Task<StatEntity>>();

            _logger.LogInformation("Gathering topics");
            await foreach (var ri in topics)
            {
                var response = client.GetSubscriptionsAsync(ri.Name, cancellationToken);

                IAsyncEnumerator<SubscriptionProperties> enumerator = response.GetAsyncEnumerator();
                try
                {
                    var prTp = await client.GetTopicRuntimePropertiesAsync(ri.Name, cancellationToken);
                    while (await enumerator.MoveNextAsync())
                    {
                        SubscriptionProperties pr = enumerator.Current;                        
                        
                        list.Add(GetSubscrStat(ri.Name, pr.SubscriptionName, client, prTp.Value.SizeInBytes, cancellationToken));
                    }
                }
                finally
                {
                    await enumerator.DisposeAsync();
                }
            }

            _logger.LogInformation("Gathering queues");
            await foreach (var qi in queues)
            {
                list.Add(GetQueueStat(qi.Name, client, cancellationToken));
            }

            _logger.LogInformation("Processing...");
            await Task.WhenAll(list.ToArray());

            _logger.LogInformation("Done");            

            return list.Select(_ => _.Result).OrderByDescending(_=>_.SizeInBytes).ToList();
        }
    }
}
