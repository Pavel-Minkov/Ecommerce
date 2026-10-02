using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers
{
    public class CreateShoppingCartHandler(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService) : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            // Apply discount to each item in the shopping cart using Grpc call to Discount service
            foreach (var item in request.Items)
            {
                var discount = await discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= discount.Amount;
            }
            var shoppingCart = request.ToEntity();
            var createdShoppingCart = await basketRepository.UpsertBasket(shoppingCart);
            return createdShoppingCart.ToResponse();
        }
    }
}
