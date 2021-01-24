using System;

namespace MassTransit.Message
{
    public class OrderAccepted
    {
        public TimeSpan Timestamp { get; set; }
    }
}