using Catalog.Application.Responses;
using Catalog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Mappers
{
    public static class TypeMapper
    {
        public static TypeResponse ToResponse(this ProductType type) => new(type.Id, type.Name);
        public static List<TypeResponse> ToResponseList(this IEnumerable<ProductType> types) => [.. types.Select(ToResponse)];
    }
}
