using System.Text.Json.Serialization;

namespace Product.Models
{
    public class ProductsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Stock { get; set; }
        public int CartId { get; set; }
    }
}
