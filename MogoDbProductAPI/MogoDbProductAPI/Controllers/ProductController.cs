using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MogoDbProductAPI.Domain.Contracts;
using MogoDbProductAPI.Service;
using MongoDB.Driver;

namespace MogoDbProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;


        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto dto)
        {
            try
            {
                var auth = Request.Headers["Authorization"];
                Console.WriteLine($"Received token: {auth}");

                var product = await _productService.CreateProductAsync(dto);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new
                {
                    status = 401,
                    title = "Unauthorized",
                    detail = "You must be logged in to perform this action.",
                    type = "https://tools.ietf.org/html/rfc7235#section-3.1"
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input");
                return BadRequest(ex.Message);
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "MongoDB error");
                return BadRequest($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetProductById([FromRoute] string id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Error retrieving product {ProductId}", id);
                return ex.Message.Contains("not found") ? NotFound(ex.Message) : BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts(
        [FromQuery] string? search,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortDirection = "asc",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 2)
        {
            try
            {
                var result = await _productService.GetFilteredProductsAsync(search, sortBy, sortDirection, page, pageSize);
                return Ok(result);
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Error retrieving filtered products");
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> UpdateProduct([FromRoute] string id, [FromBody] UpdateProductDto dto)
        {
            try
            {
                var product = await _productService.UpdateProductAsync(id, dto);
                return Ok(product);
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId}", id);
                return ex.Message.Contains("not found") ? NotFound(ex.Message) : BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Error deleting product {ProductId}", id);
                return ex.Message.Contains("not found") ? NotFound(ex.Message) : BadRequest(ex.Message);
            }
        }


        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<ProductDto>>> GetFiltered(
            [FromQuery] string? search,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDirection,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _productService.GetFilteredProductsAsync(search, sortBy, sortDirection, page, pageSize);
            return Ok(result);
        }




    }
}
