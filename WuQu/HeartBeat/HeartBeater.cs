namespace WuQu.HeartBeat
{
    using System;
    using System.Threading.Tasks;
    using Serilog;
    using WuQu.Http;
    using WuQu.Subscribing;

    public class HeartBeater
    {
        private readonly IHttpService httpService;
        private readonly ILogger logger;

        public HeartBeater(
            IHttpService httpService,
            ILogger logger)
        {
            this.httpService = httpService;
            this.logger = logger;
        }

        public Task Execute(Subscription subscription) =>
            Execute(subscription.BaseAddress, subscription.HeartBeatEndPoint);

        public Task Execute(string baseAddress, string relativeUrl)
        {
            var heartBeatUri = new Uri(new Uri(baseAddress), relativeUrl);
            return Execute(heartBeatUri);
        }

        public async Task Execute(Uri uri)
        {
            try
            {
                await httpService.Get(uri.ToString());
            }
            catch (Exception e)
            {
                logger.Debug(e, "HeartBeat Failed");
                throw new HeartBeatFailedException(e);
            }
        }
    }
}