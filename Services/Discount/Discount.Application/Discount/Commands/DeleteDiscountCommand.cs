using Discount.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Discount.Commands
{

	public record DeleteDiscountCommand : IRequest<bool>
	{
		public string ProductName { get; set; } = null!;
    }

	public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, bool>
	{
        private readonly IDiscountRepository _discountRepository;

        public DeleteDiscountCommandHandler(IDiscountRepository discountRepository)
        {
            _discountRepository=discountRepository;
        }
        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
		{
			if(string.IsNullOrEmpty(request.ProductName))
			{
				var ValidationError = new Dictionary<string, string>()
				{
					{ "ProductName", "ProductName is required."}
				};
            }
			var deleted = await _discountRepository.DeleteDiscount(request.ProductName);
			return deleted;
        }
	}
}
