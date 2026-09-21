using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts([FromQuery] CatalogSpecParams catalogSpecParams) 
        {
            var query = new GetAllProductsQuery(catalogSpecParams);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(string Id) 
        {
            var query = new GetProductByIdQuery(Id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("productName/{productName}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductByName(string productName)
        {
            var query = new GetProductByNameQuery(productName);
            var result = await _mediator.Send(query);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            IEnumerable<ProductDTO> dtoList = [.. result.Select(p => p.ToDto())];
            return Ok(dtoList);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] CreateProductDTO createProductDTO)
        {
            var command = createProductDTO.ToCommand();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProduct(string Id)
        {
            var command = new DeleteProductCommand(Id);
            var result = await _mediator.Send(command);
            return result ? NoContent() : NotFound();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateProduct(string Id, UpdateProductDTO updateProductDto)
        {
            var command = updateProductDto.ToCommand(Id);
            var result = await _mediator.Send(command);
            return result ? NoContent() : NotFound();
        }

        [HttpGet("GetAllBrands")]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetAllTypes")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetTypes() 
        {
            var query = new GetAllTypesQuery();
            var result = _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("/brand/{brand}", Name = "GetProductsByBrandName")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByBrandName(string brand)
        {
            var query = new GetProductByNameQuery(brand);
            var result = _mediator.Send(query);
            return Ok(result);
        }
    }
}
