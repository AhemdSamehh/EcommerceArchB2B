using AutoMapper;
using Discount.Application.Discount.Commands.Dtos;
using Discount.Application.Extensions;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Discount.Commands
{

	public record UpdateDiscountCommand : IRequest<UpdateCoupon>
	{
		public UpdateCoupon updateCoupon { get; set; } = null!;
    }

	public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand , UpdateCoupon>
	{
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public UpdateDiscountCommandHandler(IDiscountRepository discountRepository , IMapper mapper)
        {
            _discountRepository=discountRepository;
            _mapper=mapper;
        }
        public async Task<UpdateCoupon> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
		{
            var validationErrors = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(request.updateCoupon.ProductName))
                validationErrors["ProductName"] = "Product name must not be empty.";
            if (string.IsNullOrWhiteSpace(request.updateCoupon.Description))
                validationErrors["Description"] = "Product Description must not be empty.";
            if (request.updateCoupon.Amount<=0)
                validationErrors["Amount"] = "Amount must be greater than zero.";
            if (validationErrors.Any())
                throw GrpcErrorHelper.CreateValidationException(validationErrors);

            var coupon = _mapper.Map<Coupon>(request.updateCoupon);
            var updatedCoupon = await _discountRepository.UpdateDiscount(coupon);
            return _mapper.Map<UpdateCoupon>(updatedCoupon);
        }
	}
}
