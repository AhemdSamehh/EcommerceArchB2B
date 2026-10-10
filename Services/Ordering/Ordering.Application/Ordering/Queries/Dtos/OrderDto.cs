using Ordering.Application.Mapper;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Ordering.Queries.Dtos
{
    public class OrderDto : IMapFrom<Order>
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? AddressLine { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? CardName { get; set; }
        public string? CardNumber { get; set; }
        public string? Expiration { get; set; }
        public string? Cvv { get; set; }
        public int? PaymentMethod { get; set; }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Order, OrderDto>().ReverseMap();
        }
    }
}
