namespace WuQu.Subscribing
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Serilog;
    using WuQu.HeartBeat;

    public class SubscribeRequestHandler : IRequestHandler<SubscribeRequest>
    {
        private readonly Subscriptions subscriptions;
        private readonly ILogger logger;
        private readonly HeartBeater heartBeater;

        public SubscribeRequestHandler(
            Subscriptions subscriptions,
            ILogger logger,
            HeartBeater heartBeater)
        {
            this.subscriptions = subscriptions;
            this.logger = logger;
            this.heartBeater = heartBeater;
        }

        public async Task<Unit> Handle(
            SubscribeRequest request,
            CancellationToken cancellationToken)
        {
            logger.Information("Processing subscription request - {@SubscribeRequest}", request);

            await heartBeater.Execute(request.BaseAddress, request.HeartBeatEndPoint);

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