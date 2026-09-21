using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductsByBrandHandler(IProductRepository productRepository) : IRequestHandler<GetProductsByBrandQuery, IEnumerable<ProductResponse>>
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<IEnumerable<ProductResponse>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productRepository.GetProductByBrandAsync(request.BrandName);
            return productList.ToResponseList();
        }
    }
}
