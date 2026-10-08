using Catalog.Application.Commands;
using Catalog.Application.Exceptions;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class CreateProductCommandHandler(IProductRepository productRepository) : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var brand = await productRepository.GetBrandByIdAsync(request.BrandId);
            var type = await productRepository.GetTypeByIdAsync(request.TypeId);
            if (brand == null || type == null)
            {
                throw new CustomNotFoundException(brand == null ? nameof(ProductBrand) : nameof(ProductType), brand == null ? request.BrandId : request.TypeId);
            }
            var productEntity = request.ToEntity(brand, type);
            var newProduct = await productRepository.CreateProductAsync(productEntity);
            return newProduct.ToResponse();
        }
    }
}
