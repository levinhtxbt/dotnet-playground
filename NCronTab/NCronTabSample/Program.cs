using System;
using NCrontab;

namespace NCronTabSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var schedule = CrontabSchedule.Parse("*/5 * * * *");

            Console.ReadKey();
        }
    }
}
