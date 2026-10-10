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

	public record GetAllTypesQuery : IRequest<IList<TypesResponseDto>>
	{
	}

	public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, IList<TypesResponseDto>>
	{
        private readonly ITypeRepository _typeRepository;
        private readonly IMapper _mapper;

        public GetAllTypesQueryHandler(ITypeRepository typeRepository , IMapper mapper)
        {
            _typeRepository=typeRepository;
            _mapper=mapper;
        }
        public async Task<IList<TypesResponseDto>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
		{
			var types = await _typeRepository.GetAllTypes();
			var typesDtos = _mapper.Map<IList<TypesResponseDto>>(types);
			return typesDtos;

        }
	}
}
