using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class UpdateProductCommandHandler(IProductRepository productRepository) : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository = productRepository;
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetProductAsync(request.Id);
            if (existingProduct == null)
            {
                throw new ArgumentException("Product not found.");
            }

            var brand = await _productRepository.GetBrandByIdAsync(request.BrandId);
            var type = await _productRepository.GetTypeByIdAsync(request.TypeId);
            if (brand == null || type == null)
            {
                throw new ArgumentException("Invalid brand or type.");
            }
            var updatedProduct = request.ToUpdatedEntity(existingProduct, brand, type);
            return await _productRepository.UpdateProductAsync(updatedProduct);
        }
    }
}
