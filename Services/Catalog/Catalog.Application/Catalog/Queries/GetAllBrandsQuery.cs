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

	public record GetAllBrandsQuery : IRequest<IList<BrandResponseDto>>
	{
	}

	public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandResponseDto>>
	{
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public GetAllBrandsQueryHandler(IBrandRepository brandRepository , IMapper mapper)
        {
            _brandRepository=brandRepository;
            _mapper=mapper;
        }
        public async Task<IList<BrandResponseDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
		{
			var brandList = await _brandRepository.GetAllBrands();
			var mappedBrand = _mapper.Map<List<BrandResponseDto>>(brandList);

			return mappedBrand;
		  
		}
	}
}
