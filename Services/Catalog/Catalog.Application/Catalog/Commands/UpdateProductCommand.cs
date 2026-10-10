using AutoMapper;
using Catalog.Application.Catalog.Dtos;
using Catalog.core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Catalog.Commands
{

	public record UpdateProductCommand : IRequest<bool>
	{
        public UpdateProductDto updateProductDto { get; set; } = null!;
    }

	public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand , bool>
	{
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IProductRepository productRepository , IMapper mapper)
        {
            _productRepository=productRepository;
            _mapper=mapper;
        }
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
		{
            var existing = await _productRepository.GetProduct(request.updateProductDto.Id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Product with Id {request.updateProductDto.Id} not found");
            }
            var brand = await _productRepository.GetBrandByIdAsync(request.updateProductDto.BrandId);
            var type = await _productRepository.GetTypeByIdAsync(request.updateProductDto.TypeId);
            if (brand == null || type == null)
            {
                throw new ApplicationException("Invalid Brand or Type Specified");
            }

            var updatedProduct = _mapper.Map(request.updateProductDto, existing);
            var result = await _productRepository.UpdateProduct(updatedProduct);   
            return result;
        }
	}
}
