import React from "react";
import ProductCard from "./ProductCard";

const Shop = () => {
  const products = [
    { id: 1, name: "Product 1", price: 10, image: "/assets/product1.jpg" },
    { id: 2, name: "Product 2", price: 20, image: "/assets/product2.jpg" },
    { id: 3, name: "Product 3", price: 30, image: "/assets/product3.jpg" },
  ];

  return (
    <div className="shop">
      <h1>Shop</h1>
      <div className="product-grid">
        {products.map((product) => (
          <ProductCard key={product.id} product={product} />
        ))}
      </div>
    </div>
  );
};

export default Shop;
