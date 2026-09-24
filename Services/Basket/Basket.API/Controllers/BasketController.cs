using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController(IMediator mediatR) : ControllerBase
    {
        private readonly IMediator mediatR = mediatR;

        //Get api/v1/basket/{userName}
        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingCartDto>> GetBasket(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);
            var response = await mediatR.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCartDto>> CreateOrUpdateBasket([FromBody] CreateShoppingCartCommand command)
        {
            var response = await mediatR.Send(command);
            return Ok(response);
        }

        [HttpDelete("{userName}")]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            var command = new DeleteBasketByUserNameCommand(userName);
            await mediatR.Send(command);
            return Ok();
        }
    }
}
