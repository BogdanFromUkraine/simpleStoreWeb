using ProductService.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CartService.Models
{
    public class Cart
    {
        [Key]
        public int? Id { get; set; }
            [JsonIgnore]
    

        public decimal TotalPrice => Items.Sum(i => i.Price * Items.Count()); // Розрахунок загальної ціни

        // Властивість, що пов'язує кошик з користувачем
        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<Products> Items { get; set; } = [];   // Список товарів у кошику
    }
}
