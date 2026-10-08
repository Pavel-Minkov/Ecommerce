using Catalog.Application.Exceptions;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductByIdHandler(IProductRepository productRepository) : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetProductAsync(request.productId) ?? throw new CustomNotFoundException(nameof(Product), request.productId);
            return product.ToResponse();
        }
    }
}
