using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductsByNameHandler(IProductRepository productRepository) : IRequestHandler<GetProductByNameQuery, IEnumerable<ProductResponse>>
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<IEnumerable<ProductResponse>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productRepository.GetProductByNameAsync(request.ProductName);
            return productList.ToResponseList();
        }
    }
}
