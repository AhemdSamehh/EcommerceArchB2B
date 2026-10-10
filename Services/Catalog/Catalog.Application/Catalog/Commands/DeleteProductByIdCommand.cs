using Catalog.core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Catalog.Commands
{

	public record DeleteProductByIdCommand : IRequest<bool>
	{
        public string Id { get; set; }
    }

	public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand , bool>
	{
        private readonly IProductRepository _productRepository;

        public DeleteProductByIdCommandHandler(IProductRepository productRepository)
        {
            _productRepository=productRepository;
        }
        public async Task<bool> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
		{
            return await _productRepository.DeleteProduct(request.Id);
		}
	}
}
