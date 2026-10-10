using Basket.Application.Basket.Commands;
using Basket.Application.Basket.Commands.Dtos;
using Basket.Application.Basket.Queries;
using Basket.Application.Basket.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : BaseApiController
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IMediator _mediator;

        public BasketController(ILogger<BasketController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        //Get: api/v1/basket/{userName}
        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingCartDto>> GetBasket(string userName)
        {
            var query = new GetBasketByUserNameQuery { UserName = userName };
            var result = await _mediator.Send(query);
            _logger.LogInformation("Fetching Basket for {@UserName}", userName);
            return Ok(result);
        }

        //Post: api/v1/basket
        [HttpPost]
        public async Task<ActionResult<ShoppingCartDto>> CreateOrUpdateBasket([FromBody] CreateShoppingCartCommand command)
        {
            var result = await _mediator.Send(command);
            _logger.LogInformation("Creating or updating basket for {@UserName}", command);
            return Ok(result);
        }

        //Delete: api/v1/basket/{userName}
        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            var cmd = new DeleteBasketByUserNameCommand { UserName = userName};
            await _mediator.Send(cmd);
            return Ok();
        }

        //Post: api/v1/basket/checkout
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckoutDto basketCheckoutDto)
        {
            var command = new BasketCheckoutCommand { BasketCheckoutDto = basketCheckoutDto };
            await _mediator.Send(command);
            _logger.LogInformation("Basket checkout requested for {@UserName}", basketCheckoutDto.UserName);
            return Accepted();
        }

    }
}
