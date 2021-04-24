namespace WuQu.Subscribing
{
    public record Route
    {
        public string Type { get; init; }
        
        public string EndPoint { get; init; }
    }
}