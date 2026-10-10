using Discount.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.GrpcService
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceBase;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceBase)
        {
            _discountProtoServiceBase=discountProtoServiceBase;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName };
            var discountResponse = await _discountProtoServiceBase.GetDiscountAsync(discountRequest);
            return discountResponse;
        }

    }
}
