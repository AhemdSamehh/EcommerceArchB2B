using AutoMapper;
using Catalog.Application.Catalog.Queries.Dtos;
using Catalog.core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Catalog.Queries
{

	public record GetProductByBrandQuery : IRequest<IList<ProductDto>>
	{
		public string BrandName { get; set; } = null!;
    }

	public class GetProductByBrandQueryHandler : IRequestHandler<GetProductByBrandQuery, IList<ProductDto>>
	{
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductByBrandQueryHandler(IProductRepository productRepository , IMapper mapper)
        {
           _productRepository=productRepository;
            _mapper=mapper;
        }
        public async Task<IList<ProductDto>> Handle(GetProductByBrandQuery request, CancellationToken cancellationToken)
		{
            var products = await _productRepository.GetProductsByBrand(request.BrandName);
            return _mapper.Map<IList<ProductDto>>(products);

        }
	}
}
