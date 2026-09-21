using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Application.Responses;
using Catalog.Core.Entities;

namespace Catalog.Application.Mappers
{
    public static class BrandMapper
    {
        public static BrandResponse ToResponse(this ProductBrand brand) => new(brand.Id, brand.Name);

        public static List<BrandResponse> ToResponseList(this IEnumerable<ProductBrand> brands) => [.. brands.Select(b => b.ToResponse())];
    }
}
