using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries
{
    public record GetAllTypesQuery : IRequest<IReadOnlyList<TypeResponse>>
    {
    }
}
