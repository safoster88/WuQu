namespace WuQu.Subscribing
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Serilog;
    using Serilog.Core;
    using WuQu.HeartBeat;
    using WuQu.Http;

    public class SubscribeRequestHandler : IRequestHandler<SubscribeRequest>
    {
        private readonly IHttpService httpService;
        private readonly Subscriptions subscriptions;
        private readonly ILogger logger;

        public SubscribeRequestHandler(
            IHttpService httpService,
            Subscriptions subscriptions,
            ILogger logger)
        {
            this.httpService = httpService;
            this.subscriptions = subscriptions;
            this.logger = logger;
        }

        public async Task<Unit> Handle(
            SubscribeRequest request,
            CancellationToken cancellationToken)
        {
            logger.Information("Processing subscription request - {@SubscribeRequest}", request);
            
            var heartBeatUri = new Uri(new Uri(request.BaseAddress), request.HeartBeatEndPoint);

            try
            {
                await httpService.Get(heartBeatUri.ToString());
            }
            catch (Exception e)
            {
                logger.Debug(e, "HeartBeat Failed");
                throw new HeartBeatFailedException(e);
            }
            
            subscriptions.Add(new Subscription
            {
                BaseAddress = request.BaseAddress,
                HeartBeatEndPoint = request.HeartBeatEndPoint,
                Routes = request.Routes
            });

            logger.Debug("Finished processing subscription");
            return Unit.Value;
        }
    }
}