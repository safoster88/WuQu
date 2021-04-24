namespace WuQu.HeartBeat
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Serilog;
    using WuQu.Subscribing;

    public class HeartBeatService
    {
        private readonly Subscriptions subscriptions;
        private readonly HeartBeater heartBeater;
        private readonly IOptions<HeartBeatOptions> heartBeatOptions;
        private readonly ILogger logger;

        public HeartBeatService(
            Subscriptions subscriptions,
            HeartBeater heartBeater,
            IOptions<HeartBeatOptions> heartBeatOptions,
            ILogger logger)
        {
            this.subscriptions = subscriptions;
            this.heartBeater = heartBeater;
            this.heartBeatOptions = heartBeatOptions;
            this.logger = logger;
        }

        public async Task Start(CancellationToken ct)
        {
            logger.Information("HeartBeat service start");

            do
            {
                logger.Debug("Waiting heartbeat interval");
                await Task.Delay(TimeSpan.FromSeconds(heartBeatOptions.Value.HeartBeatIntervalInSeconds));
                var tasks = new List<Task>(subscriptions.Count);
                
                foreach (var subscription in subscriptions)
                {
                    tasks.Add(heartBeater.Execute(subscription));
                }

                logger.Debug("Waiting on {HeartBeatCount} heartbeats", tasks.Count);
                await Task.WhenAll(tasks);
                logger.Debug("All heartbeats ok");
            } while (!ct.IsCancellationRequested);
        }
    }
}