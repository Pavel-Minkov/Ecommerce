using Basket.Application.Commands;
using Basket.Application.Responses;
using Basket.Core.Entities;

namespace Basket.Application.Mappers
{
    public static class BasketMapper
    {
        public static ShoppingCartResponse ToResponse(this ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
            {
                return new ShoppingCartResponse();
            }

            return new ShoppingCartResponse
            {
                UserName = shoppingCart.UserName,
                Items = [.. shoppingCart.Items.Select(item => new ShoppingCartItemResponse
                (
                    item.Quantity,
                    item.ImageFile,
                    item.Price,
                    item.ProductId,
                    item.ProductName
                ))]
            };
        }
        // A function to map a ShoppingCart to a ShoppingCartResponse
        public static readonly Func<ShoppingCart, ShoppingCartResponse> ToResponseFunc = delegate (ShoppingCart cart)
        {
            if (cart == null)
            {
                return new ShoppingCartResponse();
            }
            return new ShoppingCartResponse
            {
                UserName = cart.UserName,
                Items = [.. cart.Items.Select(item => new ShoppingCartItemResponse
                (
                    item.Quantity,
                    item.ImageFile,
                    item.Price,
                    item.ProductId,
                    item.ProductName
                ))]
            };
        };

        public static ShoppingCart ToEntity(this CreateShoppingCartCommand command)
        {
            return new ShoppingCart
            {
                UserName = command.UserName,
                Items = [.. command.Items.Select(item => new ShoppingCartItem
                {
                    Quantity = item.Quantity,
                    ImageFile = item.ImageFile,
                    Price = item.Price,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName
                })]
            };
        }
    }
}
