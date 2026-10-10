using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Ordering.Commands.Dtos;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Ordering.Commands
{

	public record UpdateOrderCommand : IRequest
	{
        public UpdateOrderDto UpdateOrderDto { get; set; } = null!;
    }

	public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
	{
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, ILogger<UpdateOrderCommandHandler> logger , IMapper mapper)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _mapper=mapper;
        }
        public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
		{
			var orderToUpdate = await _orderRepository.GetByIdAsync(request.UpdateOrderDto.Id);

            if (orderToUpdate == null)
            {
                _logger.LogWarning("Order with id {OrderId} not found for update.", request.UpdateOrderDto.Id);
                return;
            }
            var updatedOrder = _mapper.Map(request.UpdateOrderDto, orderToUpdate);
            await _orderRepository.UpdateAsync(updatedOrder);
             _logger.LogInformation("Order with id {OrderId} updated successfully.", request.UpdateOrderDto.Id);
        }
	}
}
