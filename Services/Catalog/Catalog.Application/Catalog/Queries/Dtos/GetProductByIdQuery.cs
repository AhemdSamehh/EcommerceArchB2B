using AutoMapper;
using Catalog.core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Catalog.Queries.Dtos
{

	public record GetProductByIdQuery : IRequest<ProductDto>
	{
        public string id { get; set; }
    }

	public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
	{
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IProductRepository productRepository , IMapper mapper)
        {
            _productRepository=productRepository;
            _mapper=mapper;
        }
        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
		{
            var product = await _productRepository.GetProduct(request.id);
            if (product == null)
            {
                return null;
            }
            return _mapper.Map<ProductDto>(product);

        }
	}
}
