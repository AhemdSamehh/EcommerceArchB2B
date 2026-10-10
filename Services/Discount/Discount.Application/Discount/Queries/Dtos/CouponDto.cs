using Discount.Application.Mapper;
using Discount.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Discount.Queries.Dtos
{
    public class CouponDto : IMapFrom<Coupon>
    {
        public int Id { get; init; }
        public string ProductName { get; init; } = null!;
        public string Description { get; init; } = null!;
        public decimal Amount { get; init; }
        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Coupon, CouponDto>();
        }
    }
}
