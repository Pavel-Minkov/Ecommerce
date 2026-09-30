using Discount.Application.Commands;
using Discount.Application.DTOs;
using Discount.Core.Entities;
using Discount.Grpc.Protos;

namespace Discount.Application.Mappers
{
    public static class CouponMapper
    {
        public static CouponDto ToDto(this Coupon coupon)
        {
            return new CouponDto(coupon.Id, coupon.ProductName, coupon.Description, (int)coupon.Amount);
        }
        public static Coupon ToEntity(this CreateDiscountCommand couponDto)
        {
            return new Coupon
            {
                ProductName = couponDto.ProductName,
                Description = couponDto.Description,
                Amount = couponDto.Amount
            };
        }
        public static Coupon ToEntity(this UpdateDiscountCommand couponDto)
        {
            return new Coupon
            {
                ProductName = couponDto.ProductName,
                Description = couponDto.Description,
                Amount = couponDto.Amount
            };
        }
        public static CouponModel ToModel(this CouponDto couponDto)
        {
            return new CouponModel
            {
                Id = couponDto.Id,
                ProductName = couponDto.ProductName,
                Description = couponDto.Description,
                Amount = couponDto.Amount
            };
        }
    }
}
