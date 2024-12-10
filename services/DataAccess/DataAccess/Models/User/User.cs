using CartService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Models
{
    public class User
    {
        public User()
        {
            Cart = new Cart { Items = new List<Products>() };
        }
        // я створив простий model, просто добавив метод по створення юсера, це зручно
        private User(Guid id, string userName, string passwordHash, string email)
        {
            Id = id;
            UserName = userName;
            PasswordHash = passwordHash;
            Email = email;
        }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public ICollection<Role> Roles { get; set; } = [];

        // Зв'язок один до одного
        public Cart Cart { get; set; }

        public static User Create(Guid id, string userName, string passwordHash, string email)
        {
            return new User(id, userName, passwordHash, email);
        }

    }
}



