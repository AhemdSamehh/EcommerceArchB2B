using AutoMapper;
using Discount.Application.Discount.Commands;
using Discount.Application.Discount.Queries;
using Discount.Application.Discount.Queries.Dtos;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
namespace Discount.API.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IMediator _mediator;

        public DiscountService(IMediator mediator)
        {
            _mediator=mediator;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var couponDto = new CouponDto
            {
                ProductName = request.ProductName,

            };
            var query = new GetDiscountQuery { couponDto = couponDto };

            var coupon = await _mediator.Send(query);
            var couponModel = new CouponModel
            {
                Id = coupon.Id,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount =(int)coupon.Amount
            };
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var couponDto = new Discount.Application.Discount.Commands.Dtos.CouponDto
            {
                ProductName = request.Coupon.ProductName,
                Description = request.Coupon.Description,
                Amount = request.Coupon.Amount
            };
            var query = new CreateDiscountCommand { couponDto = couponDto };
            var coupon = await _mediator.Send(query);
            var couponModel = new CouponModel
            {
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = (int)coupon.Amount
            };
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var updateCoupon = new Discount.Application.Discount.Commands.Dtos.UpdateCoupon
            {
                Id = request.Coupon.Id,
                ProductName = request.Coupon.ProductName,
                Description = request.Coupon.Description,
                Amount = request.Coupon.Amount
            };
            var command = new UpdateDiscountCommand { updateCoupon = updateCoupon };
            var coupon = await _mediator.Send(command);
            var couponModel = new CouponModel
            {
                Id = coupon.Id,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = (int)coupon.Amount
            };
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var command = new DeleteDiscountCommand { ProductName = request.ProductName };
            var result = await _mediator.Send(command);
            return new DeleteDiscountResponse { Success = result };
        }
    }
}
