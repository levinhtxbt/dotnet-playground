using System;
using NodaTime;

namespace NodaTimeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Instant represents time from epoch
            Instant now = SystemClock.Instance.GetCurrentInstant();

            // Convert an instant to a ZonedDateTime
            ZonedDateTime nowInIsoUtc = now.InUtc();

            // Create a duration
            Duration duration = Duration.FromMinutes(3);

            // Add it to our ZonedDateTime
            ZonedDateTime thenInIsoUtc = nowInIsoUtc + duration;

            // Time zone support (multiple providers)
            var london = DateTimeZoneProviders.Tzdb["Asia/Bangkok"];

            // Time zone conversions
            var localDate = new LocalDateTime();
            var before = london.AtStrictly(localDate);
            Console.WriteLine(localDate);
            Console.WriteLine(before);
            Console.ReadKey();
        }
    }
}
