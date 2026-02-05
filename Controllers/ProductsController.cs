using Microsoft.AspNetCore.Mvc;
using webapi_practice.Models.DTOs;
using webapi_practice.Services.Interfaces;

namespace webapi_practice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  // URL: api/products
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // GET: api/products
        // Returns all products
        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
        {
            var products = await _service.GetAllProductsAsync();
            return Ok(products);
        }
    }
}