using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers
{
    public class CreateShoppingCartHandler(IBasketRepository basketRepository) : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        private readonly IBasketRepository basketRepository = basketRepository;

        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = request.ToEntity();
            var createdShoppingCart = await basketRepository.UpsertBasket(shoppingCart);
            return createdShoppingCart.ToResponse();
        }
    }
}
