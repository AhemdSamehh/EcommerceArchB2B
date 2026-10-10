using Catalog.Application.Catalog.Commands;
using Catalog.Application.Catalog.Dtos;
using Catalog.Application.Catalog.Queries;
using Catalog.Application.Catalog.Queries.Dtos;
using Catalog.core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : BaseApiController
    {
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(ILogger<CatalogController> logger)
        {
            _logger=logger;
        }
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IList<ProductDto>>> GetAllProducts([FromQuery] CatalogSpecParams CatalogSpecParams)
        {
            var query = new GetAllProductsQuery { catalogSpecParams = CatalogSpecParams };
            var result = await Sender.Send(query);
            _logger.LogInformation("Fetching products with {@filter}", CatalogSpecParams);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string Id)
        {
            var query = new GetProductByIdQuery { id = Id };
            var result = await Sender.Send(query);
            return Ok(result);
        }
         
        [HttpGet("productName/{productName}")]
        public async Task<ActionResult<IList<ProductDto>>> GetProductByProductName(string productName)
        {
            var query = new GetProductByNameQuery { Name = productName };
            var result = await Sender.Send(query);
            if (result == null || !result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            var productCommand = new CreateProductCommand { createProductDto = createProductDto };
            var result = await Sender.Send(productCommand);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var command = new DeleteProductByIdCommand { Id = id };
            var result = await Sender.Send(command);
            if (!result)
            {
                return NotFound(); //Product wasn't found or maybe deletion failed
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, UpdateProductDto updateProductDto)
        {
            var command = new UpdateProductCommand { updateProductDto = updateProductDto };
            var result = await Sender.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("GetAllBrands")]
        public async Task<ActionResult<IEnumerable<BrandResponseDto>>> GetBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await Sender.Send(query);
            return Ok(result);
        }

        [HttpGet("GetAllTypes")]
        public async Task<ActionResult<IEnumerable<TypesResponseDto>>> GetTypes()
        {
            var query = new GetAllTypesQuery();
            var result = await Sender.Send(query);
            return Ok(result);
        }

        [HttpGet("brand/{brand}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByBrand(string brand)
        {
            var query = new GetProductByBrandQuery { BrandName = brand };
            var result = await Sender.Send(query);
            return Ok(result);
        }
    }
}
