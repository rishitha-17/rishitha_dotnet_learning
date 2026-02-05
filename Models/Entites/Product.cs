using System.ComponentModel.DataAnnotations.Schema;

namespace webapi_practice.Models.Entites
{
    [Table("products")]
    public class Product
    {
        [Column("id")] 
        public int Id { get; set; }
        [Column("name")]
        public required string Name { get; set; }
        [Column("price")]
        public decimal Price { get; set; }
        [Column("category")]
        public required string Category { get; set; }
    }
}