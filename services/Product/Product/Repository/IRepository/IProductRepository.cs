using Product.Models;
using System.Linq.Expressions;
using WebApp.DataAccess.Repository.IRepository;

namespace Product.Repository.IRepository
{
    public interface IProductRepository : IRepository<Products>
    {
    }
}
