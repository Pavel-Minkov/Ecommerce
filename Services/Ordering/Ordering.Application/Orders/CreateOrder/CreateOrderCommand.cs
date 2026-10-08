using Ordering.Application.Abstractions;

namespace Ordering.Application.Orders.CreateOrder
{
    public record CreateOrderCommand
        (string? UserName, 
        decimal? TotalPrice,
        string? FirstName,
        string? LastName,
        string? EmailAddress,
        string? AddressLine,
        string? Country,
        string? State,
        string? ZipCode,
        string? CardName,
        string? CardNumber,
        string? Expiration,
        string? Cvv,
        int? PaymentMethod) : ICommand<int>;
}
