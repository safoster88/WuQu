namespace WuQu.HeartBeat
{
    public record HeartBeatOptions
    {
        public const string HeartBeat = "HeartBeat";
        
        public int HeartBeatIntervalInSeconds { get; init; }
    }
}