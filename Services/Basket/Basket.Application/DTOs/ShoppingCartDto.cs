namespace Basket.Application.DTOs
{
    public record ShoppingCartDto(string UserName, IEnumerable<ShoppingCartItemDto> Items, decimal TotalPrice);

    public record ShoppingCartItemDto(string ProductId, string ProductName, string ImageFile, decimal Price, int Quantity);

    public record CreateShoppingCartItemDto() 
    {
        public string ProductId { get; init; }
        public string ProductName { get; init; }
        public string ImageFile { get; init; }
        public decimal Price { get; set; }
        public int Quantity { get; init; }

    };
}
