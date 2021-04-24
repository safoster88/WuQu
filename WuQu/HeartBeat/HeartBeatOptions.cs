namespace WuQu.HeartBeat
{
    public record HeartBeatOptions
    {
        public const string ConfigurationKey = "HeartBeat";
        
        public int HeartBeatIntervalInSeconds { get; init; }
    }
}