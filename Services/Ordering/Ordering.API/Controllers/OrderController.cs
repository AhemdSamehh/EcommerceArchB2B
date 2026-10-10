using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Ordering.Commands;
using Ordering.Application.Ordering.Commands.Dtos;
using Ordering.Application.Ordering.Queries;
using Ordering.Application.Ordering.Queries.Dtos;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseApiController
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }
        [HttpGet("{userName}", Name = "GetOrdersByUserName")]
        public async Task<ActionResult> GetOrdersByUserName(string userName, CancellationToken cancellationToken)
        {
            var query = new GetOrderListQuery { UserName = userName };

            var result = await Sender.Send(query);

            _logger.LogInformation("Orders fetched for user {@UserName}", userName);
            return Ok(result);
        }

        [HttpPost(Name = "CheckoutOrder")]

        public async Task<ActionResult> CheckoutOrder([FromBody] CreateOrderDto createOrderDto, CancellationToken cancellationToken)
        {
            var command = new CreateOrderCommand { CreateOrderDto = createOrderDto };
            var result = await Sender.Send(command);
            _logger.LogInformation("Order created for user {@UserName}", createOrderDto.UserName);
            return Ok();
        }

        [HttpPut(Name = "UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            var command = new UpdateOrderCommand { UpdateOrderDto = updateOrderDto };
            await Sender.Send(command);
            _logger.LogInformation("Order updated for user {@UserName}", updateOrderDto.UserName);
            return Ok();
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]

        public async Task<IActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand { Id = id };
            await Sender.Send(command);
            _logger.LogInformation("Order with id {@Id} deleted", id);
            return Ok();
        }
    }
}
