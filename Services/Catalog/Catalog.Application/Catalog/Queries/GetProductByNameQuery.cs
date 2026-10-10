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

	public record GetProductByNameQuery : IRequest<IList<ProductDto>>
	{
        public string Name { get; set; }
    }

	public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, IList<ProductDto>>
	{
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductByNameQueryHandler(IProductRepository productRepository , IMapper mapper)
        {
           _productRepository=productRepository;
            _mapper=mapper;
        }
        public async Task<IList<ProductDto>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
		{
            var productList = await _productRepository.GetProductsByName(request.Name);
            var productDtoList = _mapper.Map<IList<ProductDto>>(productList);
            return productDtoList;
        }
    }
}
