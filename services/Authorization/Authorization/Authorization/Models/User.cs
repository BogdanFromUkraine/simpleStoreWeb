namespace Authorization.Models
{
    public class User
    {
        public User()
        {

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
        public static User Create(Guid id, string userName, string passwordHash, string email)
        {
            return new User(id, userName, passwordHash, email);
        }
    }
}
