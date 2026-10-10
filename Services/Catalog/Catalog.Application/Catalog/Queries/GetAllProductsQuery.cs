using AutoMapper;
using Catalog.Application.Catalog.Queries.Dtos;
using Catalog.core.Repositories;
using Catalog.core.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Catalog.Queries
{

	public record GetAllProductsQuery : IRequest<Pagination<ProductDto>>
	{
		public CatalogSpecParams catalogSpecParams { get; set; } = null!;
    }

	public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProductDto>>
	{
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public GetAllProductsQueryHandler(IProductRepository productRepository , IMapper mapper , ILogger<GetAllProductsQueryHandler> logger)
        {
           _productRepository=productRepository;
            _mapper=mapper;
            _logger=logger;
        }
        public async Task<Pagination<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
		{
            var productList = await _productRepository.GetProducts(request.catalogSpecParams);
            if (productList == null)
            {
                _logger.LogWarning("Product repository returned null for GetProducts with params {@SpecParams}", request.catalogSpecParams);

                return new Pagination<ProductDto>(
                    request.catalogSpecParams.PageIndex,
                    request.catalogSpecParams.PageSize,
                    0,
                    Array.Empty<ProductDto>());
            }

            var productDtos = _mapper.Map<IReadOnlyList<ProductDto>>(productList.Data);

            _logger.LogInformation("Successfully retrieved {Count} products for GetProducts with params {@SpecParams}", productDtos.Count, request.catalogSpecParams);

            return new Pagination<ProductDto>(
                request.catalogSpecParams.PageIndex,
                request.catalogSpecParams.PageSize,
                productList.Count,
                productDtos);
        }
    }
}
