  
  
using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

// Grab the Scheduler instance from the Factory
StdSchedulerFactory factory = new StdSchedulerFactory();
IScheduler scheduler = await factory.GetScheduler();

// and start it off
await scheduler.Start();

// define the job and tie it to our HelloJob class
IJobDetail job = JobBuilder.Create<HelloJob>()
	.WithIdentity("job1", "group1")
	.Build();

// Trigger the job to run now, and then repeat every 10 seconds
ITrigger trigger = TriggerBuilder.Create()
	.WithIdentity("trigger1", "group1")
	.StartNow()
	.WithSimpleSchedule(x => x
		.WithIntervalInSeconds(10)
		.RepeatForever())
	.Build();

// Tell quartz to schedule the job using our trigger
await scheduler.ScheduleJob(job, trigger);

Console.WriteLine("Press any key to close the application");
Console.ReadKey();
// and last shut down the scheduler when you are ready to close your program
await scheduler.Shutdown();



public class HelloJob : IJob
{
	public async Task Execute(IJobExecutionContext context)
	{
		await Console.Out.WriteLineAsync("Greetings from HelloJob!");
	}
}