using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers
{
    public class GetBasketByUserNameHandler(IBasketRepository basketRepository) : IRequestHandler<GetBasketByUserNameQuery, ShoppingCartResponse>
    {
        public async Task<ShoppingCartResponse> Handle(GetBasketByUserNameQuery request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetBasket(request.UserName);
            if (basket == null)
            {
                return new ShoppingCartResponse(request.UserName, []);
            }
            return basket.ToResponse();
            //Using a function to map the basket to a response
            //return BasketMapper.ToResponseFunc(basket);
        }
    }
}
