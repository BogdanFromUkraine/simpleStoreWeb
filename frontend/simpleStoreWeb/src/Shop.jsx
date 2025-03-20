import React, { useEffect } from "react";
import { ProductCard } from "./ProductCard";
import { observer } from "mobx-react-lite";
import { useStores } from "../store/root-store-context.ts";

export const Shop = observer(() => {
  const { dataStore } = useStores();

  useEffect(() => {
    async function getAllProducts() {
      await dataStore.get_Products();
    }
    getAllProducts();
  }, []);

  return (
    <div className="shop" id="section_shop">
      <h1>Shop</h1>
      <div className="product-grid">
        {dataStore.products && dataStore.products.length > 0 ? (
          dataStore.products.map((product) => (
            <ProductCard key={product.id} product={product} />
          ))
        ) : (
          <div>Products are not find</div>
        )}
      </div>
    </div>
  );
});
