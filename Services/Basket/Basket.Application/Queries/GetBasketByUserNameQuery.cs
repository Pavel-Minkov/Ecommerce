using Basket.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Queries
{
    public record GetBasketByUserNameQuery(string UserName) : IRequest<ShoppingCartResponse>;
}
