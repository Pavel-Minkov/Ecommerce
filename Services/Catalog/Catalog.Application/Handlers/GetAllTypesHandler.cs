using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllTypesHandler(ITypeRepository typeRepository) : IRequestHandler<GetAllTypesQuery, List<TypeResponse>>
    {
        private readonly ITypeRepository _typeRepository = typeRepository;

        public async Task<List<TypeResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var typeList = await _typeRepository.GetAllTypesAsync();
            return typeList.ToResponseList();
        }
    }
}
