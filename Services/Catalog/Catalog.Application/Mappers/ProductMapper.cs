using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;

namespace Catalog.Application.Mappers
{
    public static class ProductMapper
    {
        public static ProductResponse ToResponse(this Product product)
        {
            if (product == null) return null;

            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Summary = product.Summary,
                Description = product.Description,
                ImageFile = product.ImageFile,
                Brand = product.Brand,
                Type = product.Type,
                Price = product.Price,
                CreatedDate = product.CreatedDate
            };
        }

        public static Pagination<ProductResponse> ToResponse(this Pagination<Product> pagination)
            => new(pagination.PageIndex, pagination.PageSize, pagination.Count, [.. pagination.Items.Select(p => p.ToResponse())]);

        public static IReadOnlyList<ProductResponse> ToResponseList(this IReadOnlyList<Product> products) => [.. products.Select(ToResponse)];

        public static Product ToEntity(this CreateProductCommand command, ProductBrand brand, ProductType type)
        {
            return new Product
            {
                Name = command.Name,
                Summary = command.Summary,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Brand = brand,
                Type = type,
                Price = command.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
        }

        public static Product ToUpdatedEntity(this UpdateProductCommand command, Product existingProduct, ProductBrand brand, ProductType type)
        {
            return new Product
            {
                Id = existingProduct.Id,
                Name = command.Name,
                Summary = command.Summary,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Brand = brand,
                Type = type,
                Price = command.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
        }

        public static ProductDTO ToDto(this ProductResponse productResponse) 
        {
            if (productResponse == null)
            {
                return null;
            }
            return new ProductDTO(
                productResponse.Id, 
                productResponse.Name, 
                productResponse.Summary, 
                productResponse.Description, 
                productResponse.ImageFile, 
                new BrandDTO(productResponse.Brand.Id, productResponse.Brand.Name),
                new TypeDTO(productResponse.Type.Id, productResponse.Type.Name),
                productResponse.Price, 
                DateTimeOffset.UtcNow
                );
        }

        public static UpdateProductCommand ToCommand(this UpdateProductDTO dto, string id)
        {
            return new UpdateProductCommand(id,dto.Name,dto.Summary,dto.Description,dto.ImageFile,dto.BrandId,dto.TypeId,dto.Price);
        }
        public static CreateProductCommand ToCommand(this CreateProductDTO dto)
        {
            return new CreateProductCommand(dto.Name, dto.Summary, dto.Description, dto.ImageFile, dto.BrandId, dto.TypeId, dto.Price);
        }
    }
}
