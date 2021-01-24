using System.Threading.Tasks;
using MassTransit;
using MassTransit.Message;
using Microsoft.Extensions.Logging;

namespace MassTransit.WebApi.Consumer
{
    public class OrderSubmitConsumer : IConsumer<OrderSubmit>
    {
        private readonly ILogger<OrderSubmitConsumer> logger;

        public OrderSubmitConsumer(ILogger<OrderSubmitConsumer> logger)
        {
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<OrderSubmit> context)
        {
            logger.LogInformation("Hello", context.Message);

            await context.RespondAsync<OrderAccepted>(new OrderAccepted
            {
                Timestamp = new System.TimeSpan(5, 0, 0)
            });
        }

    }
}