namespace WuQu.Publishing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Serilog;
    using WuQu.Http;
    using WuQu.Subscribing;

    public class PublishRequestHandler : IRequestHandler<PublishRequest>
    {
        private readonly IHttpService httpService;
        private readonly Subscriptions subscriptions;
        private readonly ILogger logger;

        public PublishRequestHandler(
            IHttpService httpService,
            Subscriptions subscriptions,
            ILogger logger)
        {
            this.httpService = httpService;
            this.subscriptions = subscriptions;
            this.logger = logger;
        }

        public async Task<Unit> Handle(
            PublishRequest request,
            CancellationToken cancellationToken)
        {
            logger.Information("Processing publish payload - {Type} - {Payload}", request.Type, request.Payload);
            
            var tasks = new List<Task>();
            
            foreach (var subscription in subscriptions)
            {
                foreach (var route in subscription.Routes)
                {
                    if (request.Type == route.Type)
                    {
                        var routeUri = new Uri(new Uri(subscription.BaseAddress), route.EndPoint);

                        logger.Debug("Publishing payload to subscriber - {Address}", routeUri);

                        tasks.Add(httpService.Post(routeUri.ToString(), request.Payload));
                    }
                }
            }

            await Task.WhenAll(tasks);
            logger.Debug("Finished processing publish payload");
            return Unit.Value;
        }
    }
}