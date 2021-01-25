using System;
using System.Threading.Tasks;
using MassTransit.Message;
using Microsoft.AspNetCore.Mvc;

namespace MassTransit.WebApi.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IRequestClient<OrderSubmit> orderSubmtClient;
        private readonly IBusControl bus;

        public TestController(IRequestClient<OrderSubmit> orderSubmtClient, IBusControl bus)
        {
            this.orderSubmtClient = orderSubmtClient;
            this.bus = bus;
        }

        [HttpGet()]
        public async Task<IActionResult> Test()
        {
            var response = await orderSubmtClient.GetResponse<OrderAccepted>(new OrderSubmit
            {
                Id = Convert.ToInt32(new Random().Next(1, 9999))
            });

            return Ok(response);
        }


        [HttpGet("2")]
        public async Task<IActionResult> Test2()
        {
            // var response = await orderSubmtClient.GetResponse<OrderAccepted>(new OrderSubmit
            // {
            //     Id = 1
            // });
            await bus.Publish<OrderSubmit>(new OrderSubmit
            {
                Id = Convert.ToInt32(new Random().Next(1, 9999))
            });

            return Ok();
        }
    }
}