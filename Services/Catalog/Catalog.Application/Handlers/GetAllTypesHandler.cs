using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllTypesHandler(ITypeRepository typeRepository) : IRequestHandler<GetAllTypesQuery, IReadOnlyList<TypeResponse>>
    {
        public async Task<IReadOnlyList<TypeResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var typeList = await typeRepository.GetAllTypesAsync();
            return typeList.ToResponseList();
        }
    }
}
