namespace WuQu.Subscribing
{
    using MediatR;

    public record SubscribeRequest : IRequest
    {
        public string BaseAddress { get; init; }
        
        public string HeartBeatEndPoint { get; init; }

        public Route[] Routes { get; init; }
    }
}