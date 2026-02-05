using Microsoft.EntityFrameworkCore;
using webapi_practice.Data;
using webapi_practice.Models.Entites;
using webapi_practice.Repositories.Interfaces;

namespace webapi_practice.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all products
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}