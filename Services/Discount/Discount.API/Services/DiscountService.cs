using Discount.Application.Commands;
using Discount.Application.Mappers;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.API.Services
{
    public class DiscountService(IMediator mediator) : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IMediator mediator = mediator;

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var query = new GetDiscountQuery(request.ProductName);
            var couponDto = await mediator.Send(query);
            return couponDto.ToModel();
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var command = new CreateDiscountCommand(request.Coupon.ProductName, request.Coupon.Description, request.Coupon.Amount);
            var couponDto = await mediator.Send(command);
            return couponDto.ToModel();
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon;
            var command = new UpdateDiscountCommand(coupon.Id,coupon.ProductName, coupon.Description, coupon.Amount);
            var couponDto = await mediator.Send(command);
            return couponDto.ToModel();
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var command = new DeleteDiscountCommand(request.ProductName);
            var result = await mediator.Send(command);
            return new DeleteDiscountResponse { Success = result };
        }
    }
}
