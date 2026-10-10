using Discount.Application.Mapper;
using Discount.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Discount.Commands.Dtos
{
    public class CouponDto : IMapFrom<Coupon>
    {
        public string ProductName { get; init; }
        public string Description { get; init; }
        public decimal Amount { get; init; }
        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<CouponDto, Coupon>();
        }
    }
}
