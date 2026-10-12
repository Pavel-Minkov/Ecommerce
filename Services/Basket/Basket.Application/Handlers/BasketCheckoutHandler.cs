using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Basket.Application.Handlers
{
    public class BasketCheckoutHandler(IMediator mediator, IPublishEndpoint publishEndpoint, ILogger<BasketCheckoutHandler> logger) : IRequestHandler<BasketCheckoutCommand, Unit>
    {
        public async Task<Unit> Handle(BasketCheckoutCommand request, CancellationToken cancellationToken)
        {
            var basketDto = request.basketCheckoutDto;
            var basketResponse = await mediator.Send(new GetBasketByUserNameQuery(basketDto.UserName), cancellationToken);
            if (basketResponse == null || basketResponse.Items.Count == 0)
            {
                throw new InvalidOperationException("Basket not found or empty");
            }
            var basket = basketResponse.ToEntity();
            var evt = basketDto.ToBasketCheckoutEvent(basket);
            logger.LogInformation("Publishing BasketCheckoutEvent for user {UserName}", basket.UserName);
            await publishEndpoint.Publish(evt, cancellationToken);
            await mediator.Send(new DeleteBasketByUserNameCommand(basket.UserName), cancellationToken);
            return Unit.Value;
        }
    }
}
