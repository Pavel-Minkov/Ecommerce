using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class UpdateProductCommandHandler(IProductRepository productRepository) : IRequestHandler<UpdateProductCommand, bool>
    {
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await productRepository.GetProductAsync(request.Id);
            if (existingProduct == null)
            {
                throw new ArgumentException("Product not found.");
            }

            var brand = await productRepository.GetBrandByIdAsync(request.BrandId);
            var type = await productRepository.GetTypeByIdAsync(request.TypeId);
            if (brand == null || type == null)
            {
                throw new ArgumentException("Invalid brand or type.");
            }
            var updatedProduct = request.ToUpdatedEntity(existingProduct, brand, type);
            return await productRepository.UpdateProductAsync(updatedProduct);
        }
    }
}
