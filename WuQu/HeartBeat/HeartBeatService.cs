namespace WuQu.HeartBeat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                var tasks = new List<Task<HeartBeatResult>>(subscriptions.Count);
                
                foreach (var subscription in subscriptions)
                {
                    tasks.Add(ExecuteHeartBeatForSubscription(subscription));
                }

                logger.Debug("Waiting on {HeartBeatCount} heartbeats", tasks.Count);
                var results = await Task.WhenAll(tasks);
                var failedHeartBeats = results.Where(x => !x.IsHeartBeatSuccessful).ToList();

                if (failedHeartBeats.Count() > 0)
                {
                    foreach (var failure in failedHeartBeats)
                    {
                        logger.Information("HeartBeat failed for {BaseAddress} - Removing subscription", failure.Subscription.BaseAddress);
                        subscriptions.Remove(failure.Subscription);
                    }    
                }
                else
                {
                    logger.Debug("All heartbeats ok");
                }
            } while (!ct.IsCancellationRequested);
        }

        private async Task<HeartBeatResult> ExecuteHeartBeatForSubscription(
            Subscription subscription) => new(subscription, await heartBeater.Execute(subscription));

        private struct HeartBeatResult
        {
            public HeartBeatResult(
                Subscription subscription,
                bool isHeartBeatSuccessful)
            {
                Subscription = subscription;
                IsHeartBeatSuccessful = isHeartBeatSuccessful;
            }
            
            public Subscription Subscription { get; }

            public bool IsHeartBeatSuccessful { get; }
        }
    }
}