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

        public Task<bool> Execute(Subscription subscription) =>
            Execute(subscription.BaseAddress, subscription.HeartBeatEndPoint);

        public Task<bool> Execute(string baseAddress, string relativeUrl)
        {
            var heartBeatUri = new Uri(new Uri(baseAddress), relativeUrl);
            return Execute(heartBeatUri);
        }

        public async Task<bool> Execute(Uri uri)
        {
            try
            {
                await httpService.Get(uri.ToString());
                return true;
            }
            catch (Exception e)
            {
                logger.Debug(e, "HeartBeat Failed");
                return false;
            }
        }
    }
}