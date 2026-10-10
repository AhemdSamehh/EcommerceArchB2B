using AutoMapper;
using Discount.Application.Discount.Commands.Dtos;
using Discount.Application.Extensions;
using Discount.Core.Repositories;
using Grpc.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Discount.Commands
{

	public record CreateDiscountCommand : IRequest<CouponDto>
	{
		public CouponDto couponDto { get; set; } = null!;
    }

	public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, CouponDto>
	{
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public CreateDiscountCommandHandler(IDiscountRepository discountRepository , IMapper mapper)
        {
            _discountRepository=discountRepository;
            _mapper=mapper;
        }
        public async Task<CouponDto> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
		{
			var validationErrors = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(request.couponDto.ProductName))
                validationErrors["ProductName"] = "Product name must not be empty.";
            if (string.IsNullOrWhiteSpace(request.couponDto.Description))
                validationErrors["Description"] = "Product Description must not be empty.";
            if (request.couponDto.Amount<=0)
                validationErrors["Amount"] = "Amount must be greater than zero.";
            if (validationErrors.Any())
                throw GrpcErrorHelper.CreateValidationException(validationErrors);

            var coupon = _mapper.Map<Core.Entities.Coupon>(request.couponDto);
            var createdCoupon = await _discountRepository.CreateDiscount(coupon);
            if(!createdCoupon)
                throw new RpcException(new Status(StatusCode.Internal, "Failed to create discount."));
            return _mapper.Map<CouponDto>(createdCoupon);
        }
	}
}
