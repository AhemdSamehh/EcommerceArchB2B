using AutoMapper;
using Catalog.Application.Catalog.Dtos;
using Catalog.Application.Catalog.Queries.Dtos;
using Catalog.core.Entities;
using Catalog.core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Catalog.Commands
{

	public record CreateProductCommand : IRequest<CreateProductDto>
	{
        public CreateProductDto createProductDto { get; set; }
    }

	public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand , CreateProductDto>
	{
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IProductRepository productRepository , IMapper mapper)
        {
            _productRepository=productRepository;
            _mapper=mapper;
        }
        public async Task<CreateProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
		{
            var brand = await _productRepository.GetBrandByIdAsync(request.createProductDto.BrandId);
            var type = await _productRepository.GetTypeByIdAsync(request.createProductDto.TypeId);

            if (brand == null || type == null)
            {
                throw new ApplicationException("Invalid Brand or Type Specified");
            }

            var mappedProduct = _mapper.Map<Product>(request.createProductDto);
            var newProduct = await _productRepository.CreateProduct(mappedProduct);
            var response = _mapper.Map<CreateProductDto>(newProduct);
            return response;
        }
	}
}
