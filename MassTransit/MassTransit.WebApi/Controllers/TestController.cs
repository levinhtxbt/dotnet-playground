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

        public TestController(IRequestClient<OrderSubmit> orderSubmtClient)
        {
            this.orderSubmtClient = orderSubmtClient;
        }

        [HttpGet()]
        public async Task<IActionResult> Test()
        {
            var response = await orderSubmtClient.GetResponse<OrderAccepted>(new OrderSubmit
            {
                Id = 1
            });

            return Ok(response);
        }
    }
}