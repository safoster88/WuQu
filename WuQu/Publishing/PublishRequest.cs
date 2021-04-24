namespace WuQu.Publishing
{
    using MediatR;

    public record PublishRequest : IRequest
    {
        public string Type { get; init; }
        
        public object Payload { get; init; }
    }
}