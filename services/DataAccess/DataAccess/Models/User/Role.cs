using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductService.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        //добавляються дозволи до ролі, типу юсеру можна тільки читати, а адміну редагувати
        //реазізовані зв'язки tables
        [JsonIgnore]
        public ICollection<Permission> Permissions { get; set; } = [];

        [JsonIgnore]
        public ICollection<User> Users { get; set; } = [];
    }
}
