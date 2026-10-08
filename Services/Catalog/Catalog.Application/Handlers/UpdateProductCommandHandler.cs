using Catalog.Application.Commands;
using Catalog.Application.Exceptions;
using Catalog.Application.Mappers;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class UpdateProductCommandHandler(IProductRepository productRepository) : IRequestHandler<UpdateProductCommand, bool>
    {
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await productRepository.GetProductAsync(request.Id) ?? throw new CustomNotFoundException(nameof(Product), request.Id);
            var brand = await productRepository.GetBrandByIdAsync(request.BrandId);
            var type = await productRepository.GetTypeByIdAsync(request.TypeId);
            if (brand == null || type == null)
            {
                throw new CustomNotFoundException(brand == null ? nameof(ProductBrand) : nameof(ProductType), brand == null ? request.BrandId : request.TypeId);
            }
            var updatedProduct = request.ToUpdatedEntity(existingProduct, brand, type);
            return await productRepository.UpdateProductAsync(updatedProduct);
        }
    }
}
