﻿using Product.Models;
using ProductService.Models;

namespace Product.Repository.IRepository
{
    public interface IProductRepository
    {
        Task Add(ProductsDTO entity);

        IEnumerable<Products> GetAll();

        Task Remove(string name);

        Task<Products> GetProduct(int id);

        Task Update(int id, ProductsDTO productsDTO);
    }
}