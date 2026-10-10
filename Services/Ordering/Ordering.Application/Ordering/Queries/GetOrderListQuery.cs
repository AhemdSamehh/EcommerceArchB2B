using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Ordering.Queries.Dtos;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Ordering.Queries
{

	public record GetOrderListQuery : IRequest<List<OrderDto>>
	{
		public string UserName { get; set; } = null!;
    }

	public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrderDto>>
	{
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<GetOrderListQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetOrderListQueryHandler(IOrderRepository orderRepository, ILogger<GetOrderListQueryHandler> logger , IMapper mapper)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            this._mapper=mapper;
        }
        public async Task<List<OrderDto>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
		{
            var orders = await _orderRepository.GetOrdersByUserName(request.UserName);
            var mappedOrders = _mapper.Map<List<OrderDto>>(orders);
            return mappedOrders;

        }
	}
}
