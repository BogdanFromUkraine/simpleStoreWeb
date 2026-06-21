using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Interfaces
{
    public interface IProductStockService
    {
        Task<bool> IsInStockAsync(string productId, int quantity);
    }
}
