using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Ordering.Commands
{

	public record DeleteOrderCommand : IRequest
	{
        public int Id { get; set; }
    }

	public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
	{
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
		{
			var order = await _orderRepository.GetByIdAsync(request.Id);
            if (order == null)
            {
             _logger.LogWarning("Order with id {OrderId} not found.", request.Id);
                return;
            }
            await _orderRepository.DeleteAsync(order);
            _logger.LogInformation("Order with id {OrderId} deleted successfully.", request.Id);
        }
	}
}
