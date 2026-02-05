namespace webapi_practice.Models.DTOs
{
    // DTO - Data we send to the client
    public class ProductDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public required string Category { get; set; }
    }
}