namespace Basket.Application.Responses
{
    public record ShoppingCartResponse
    {
        public string UserName { get; init; }
        public IEnumerable<ShoppingCartItemResponse> Items { get; init; }
        public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);

        public ShoppingCartResponse()
        {
            UserName = string.Empty;
            Items = [];
        }

        public ShoppingCartResponse(string userName) : this(userName, [])
        {
            
        }

        public ShoppingCartResponse(string userName, IEnumerable<ShoppingCartItemResponse> items)
        {
            UserName = userName ?? string.Empty;
            Items = items ?? [];
        }
    }
}
