import React, { useEffect } from "react";
import { ProductCard } from "./ProductCard";
import { observer } from "mobx-react-lite";
import { useStores } from "../store/root-store-context";

export const Shop = observer(() => {
  const { products, get_Products } = useStores();

  useEffect(() => {
    async function getAllProducts() {
      await get_Products();
    }
    getAllProducts();
  }, []);

  return (
    <div className="shop">
      <h1>Shop</h1>
      <div className="product-grid">
        {products && products.length > 0 ? (
          products.map((product) => (
            <ProductCard key={product.id} product={product} />
          ))
        ) : (
          <div>Products are not find</div>
        )}
      </div>
    </div>
  );
});
