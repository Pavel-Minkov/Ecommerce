using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using DnsClient.Internal;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers
{
    public class GetAllProductsHandler(IProductRepository productRepository, ILogger<GetAllProductsHandler> logger) : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
    {
        public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var productList = await productRepository.GetProductsAsync(request.SpecParams);
            logger.LogInformation("Retrieved {Count} products.", productList.Count);
            return productList.ToResponse();
        }
    }
}
