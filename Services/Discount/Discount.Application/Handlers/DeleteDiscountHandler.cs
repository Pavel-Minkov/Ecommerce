using Discount.Application.Commands;
using Discount.Core.Repositories;
using MediatR;

namespace Discount.Application.Handlers
{
    public class DeleteDiscountHandler(IDiscountRepository discountRepository) : IRequestHandler<DeleteDiscountCommand, bool>
    {
        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            
            if (string.IsNullOrWhiteSpace(request.ProductName))
            {
                var validationErrors = new Dictionary<string, string>
                {
                    ["ProductName"] = "ProductName cannot be null or empty."
                };
                throw Extensions.GrpcErrorHelper.CreateValidationException(validationErrors);
            }
            return await discountRepository.DeleteDiscount(request.ProductName);
        }
    }
}
