using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Basket.Commands
{

	public record DeleteBasketByUserNameCommand : IRequest<Unit>
	{
        public string UserName { get; set; }
    }

	public class DeleteBasketByUserNameCommandHandler : IRequestHandler<DeleteBasketByUserNameCommand , Unit>
	{
        private readonly IBasketRepository _basketRepository;

        public DeleteBasketByUserNameCommandHandler(IBasketRepository basketRepository)
        {
            _basketRepository=basketRepository;
        }
        public async Task<Unit> Handle(DeleteBasketByUserNameCommand request, CancellationToken cancellationToken)
		{
			await _basketRepository.DeleteBasket(request.UserName);	
            return Unit.Value;

        }
	}
}
