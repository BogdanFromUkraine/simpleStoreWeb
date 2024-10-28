using Product.DataAccess;
using Product.Models;
using Product.Repository.IRepository;
using WebApp.DataAccess.Repository;

namespace Product.Repository
{
    public class ProductRepository : Repository<Products>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
