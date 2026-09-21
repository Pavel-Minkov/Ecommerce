using MongoDB.Bson.Serialization.IdGenerators;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.DTOs
{
    public record ProductDTO(
        string Id,
        string Name,
        string Summary,
        string Description,
        string ImageFile,
        BrandDTO Brand,
        TypeDTO Type,
        decimal Price,
        DateTimeOffset CreatedDate
        );
    public record BrandDTO(string Id, string Name);
    public record TypeDTO(string Id,string Name);

    public record CreateProductDTO {
        [Required]
        public required string Name { get; init; }
        [Required]
        public required string Summary { get; init; }
        [Required]
        public required string Description { get; init; }
        [Required]
        public required string ImageFile { get; init; }
        [Required]
        public required string BrandId { get; init; }
        [Required]
        public required string TypeId { get; init; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public required decimal Price { get; init; }
    }
    public record UpdateProductDTO {
        [Required]
        public required string Name { get; init; }
        [Required]
        public required string Summary { get; init; }
        [Required]
        public required string Description { get; init; }
        [Required]
        public required string ImageFile { get; init; }
        [Required]
        public required string BrandId { get; init; }
        [Required]
        public required string TypeId { get; init; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public required decimal Price { get; init; }
    }
}
