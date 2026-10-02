using Discount.Application.Commands;
using Discount.Application.DTOs;
using Discount.Application.Mappers;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers
{
    public class CreateDiscountHandler(IDiscountRepository discountRepository) : IRequestHandler<CreateDiscountCommand, CouponDto>
    {
        public async Task<CouponDto> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var validationErrors = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(request.ProductName))
            {
                validationErrors["ProductName"] = "ProductName cannot be null or empty.";
            }
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                validationErrors["Description"] = "Description cannot be null or empty.";
            }
            if (request.Amount <= 0 || request.Amount > 100)
            {
                validationErrors["Amount"] = "Amount must be between 1 and 100.";
            }

            if (validationErrors.Count > 0)
            {
                throw Extensions.GrpcErrorHelper.CreateValidationException(validationErrors);
            }

            var coupon = request.ToEntity();
            var created_discount = await discountRepository.CreateDiscount(coupon);
            if (!created_discount)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Could not create discount for product: '{request.ProductName}'"));
            }
            return coupon.ToDto();
        }
    }
}
