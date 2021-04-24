namespace WuQu.Subscribing
{
    public record Subscription
    {
        public string BaseAddress { get; init; }

        public string HeartBeatEndPoint { get; init; }

        public Route[] Routes { get; init; }
    }
}