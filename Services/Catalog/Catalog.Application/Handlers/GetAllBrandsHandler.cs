using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllBrandsHandler(IBrandRepository brandRepository) : IRequestHandler<GetAllBrandsQuery, IReadOnlyList<BrandResponse>>
    {

        public async Task<IReadOnlyList<BrandResponse>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brandList = await brandRepository.GetAllBrandsAsync();
            return brandList.ToResponseList();
        }
    }
}
