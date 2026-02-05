
using webapi_practice.Models.DTOs;

namespace webapi_practice.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
    }
}