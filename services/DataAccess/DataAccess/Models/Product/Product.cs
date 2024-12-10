using CartService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductService.Models
{
 
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        // Властивість зв’язку з кошиком
        [JsonIgnore]
        public int? CartId { get; set; }
        [JsonIgnore]
        public Cart? Cart { get; set; }
    } 
}
