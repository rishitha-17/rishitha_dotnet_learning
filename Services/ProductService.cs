using webapi_practice.Models.DTOs;
using webapi_practice.Repositories.Interfaces;
using webapi_practice.Services.Interfaces;

namespace webapi_practice.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllProductsAsync();
            // Convert Entity to DTO
            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category
            }).ToList();
        }
    }
}