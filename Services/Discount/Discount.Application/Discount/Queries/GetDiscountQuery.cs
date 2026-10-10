using AutoMapper;
using Discount.Application.Discount.Queries.Dtos;
using Discount.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Discount.Queries
{

	public record GetDiscountQuery : IRequest<CouponDto>
	{
		public CouponDto couponDto { get; set; } = null!;
    }

	public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponDto>
	{
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public GetDiscountQueryHandler(IDiscountRepository discountRepository , IMapper mapper)
        {
           _discountRepository=discountRepository;
            _mapper=mapper;
        }
        public async Task<CouponDto> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
		{
			// validation the input
			if(string.IsNullOrEmpty(request.couponDto.ProductName))
			{
				var validationsError = new Dictionary<string, string>()
				{
					{"ProductName","ProductName is required."}
				};
            }
            // fetch the data from database
			var coupon = await _discountRepository.GetDiscount(request.couponDto.ProductName);
			if(coupon == null)
				throw new Exception("Discount not found.");

            // map the data to the output
			var mappedCoupon = _mapper.Map<CouponDto>(coupon);
			return mappedCoupon;
        }
    }
}
