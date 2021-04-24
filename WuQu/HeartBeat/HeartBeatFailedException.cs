namespace WuQu.HeartBeat
{
    using System;

    public class HeartBeatFailedException : Exception
    {
        public HeartBeatFailedException()
        {
        }
        
        public HeartBeatFailedException(Exception inner)
            : base("Heart Beat Failed", inner)
        {
        }
    }
}