using System.Threading.Tasks;
using MassTransit.Message;
using Microsoft.Extensions.Logging;

namespace MassTransit.Consumer.Consumer
{
    public class OrderSubmitConsumer : IConsumer<OrderSubmit>
    {
        private readonly ILogger<OrderSubmitConsumer> _logger;

        public OrderSubmitConsumer(ILogger<OrderSubmitConsumer> logger)
        {
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<OrderSubmit> context)
        {
            _logger.LogInformation($"Message: {context.Message.Id}");
        }
    }
}