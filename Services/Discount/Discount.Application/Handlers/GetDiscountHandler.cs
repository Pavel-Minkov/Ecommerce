using Discount.Application.DTOs;
using Discount.Application.Mappers;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers
{
    public class GetDiscountHandler(IDiscountRepository discountRepository) : IRequestHandler<GetDiscountQuery, CouponDto>
    {
        private readonly IDiscountRepository discountRepository = discountRepository;

        public async Task<CouponDto> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ProductName))
            {
                var validationErrors = new Dictionary<string, string>
                {
                    { "ProductName", "ProductName cannot be null or empty."}
                };
                throw Extensions.GrpcErrorHelper.CreateValidationException(validationErrors);
            }
            var coupon = await discountRepository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Could not get discount for product: '{request.ProductName}'"));
            }
            return coupon.ToDto();
        }
    }
}
