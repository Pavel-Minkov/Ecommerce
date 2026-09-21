using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Commands
{
    public record CreateProductCommand(
        string Name, 
        string Summary, 
        string Description, 
        string ImageFile, 
        string BrandId, 
        string TypeId, 
        decimal Price
        ) : IRequest<ProductResponse> { }
}
