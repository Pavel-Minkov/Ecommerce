using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductsByNameHandler(IProductRepository productRepository) : IRequestHandler<GetProductByNameQuery, IReadOnlyList<ProductResponse>>
    {
        public async Task<IReadOnlyList<ProductResponse>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var productList = await productRepository.GetProductByNameAsync(request.ProductName);
            return productList.ToResponseList();
        }
    }
}
