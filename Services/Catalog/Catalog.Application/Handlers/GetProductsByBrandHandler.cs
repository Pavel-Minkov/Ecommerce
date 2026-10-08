using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductsByBrandHandler(IProductRepository productRepository) : IRequestHandler<GetProductsByBrandQuery, IReadOnlyList<ProductResponse>>
    {
        public async Task<IReadOnlyList<ProductResponse>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
        {
            var productList = await productRepository.GetProductByBrandAsync(request.BrandName);
            return productList.ToResponseList();
        }
    }
}
