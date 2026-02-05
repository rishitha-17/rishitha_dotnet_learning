using webapi_practice.Models.Entites;

namespace webapi_practice.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
    }
}