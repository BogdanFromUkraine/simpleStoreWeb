using CartService.DataAccess;
using Microsoft.EntityFrameworkCore; // Потрібен для .ToListAsync() та .FirstOrDefaultAsync()
using Product.Models;
using Product.Repository.IRepository;
using ProductService.Models;

namespace Product.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Products product)
        {
            // Ми просто додаємо те, що нам дали. 
            // Ніяких 'new Products', ніяких 'decimal.Parse'.
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            // Використовуємо асинхронну версію EF Core
            return await _db.Products.ToListAsync();
        }

        public async Task<Products?> GetByIdAsync(int id)
        {
            // Тільки читаємо. SaveChanges тут БУВ ПОМИЛКОЮ, ми його прибрали.
            return await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Products?> GetByNameAsync(string name)
        {
            return await _db.Products.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task RemoveAsync(Products product)
        {
            // Репозиторій просто видаляє об'єкт, який йому дали
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Products product)
        {
            // EF Core сам зрозуміє, що змінилося в об'єкті product
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }
    }
}