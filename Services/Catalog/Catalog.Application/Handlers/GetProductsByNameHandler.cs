using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductsByNameHandler(IProductRepository productRepository) : IRequestHandler<GetProductByNameQuery, IEnumerable<ProductResponse>>
    {
        public async Task<IEnumerable<ProductResponse>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var productList = await productRepository.GetProductByNameAsync(request.ProductName);
            return productList.ToResponseList();
        }
    }
}
