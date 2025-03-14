import { observer } from "mobx-react-lite";
import React from "react";
import { useStores } from "../store/root-store-context";
import getUserId from "./utils/getUserId.jsx";

export const ProductCard = observer(({ product }) => {
  const { add_Product_To_Cart } = useStores();

  async function handleButtonClick(id) {
    await add_Product_To_Cart(await getUserId(), id);
  }

  return (
    <div className="product-card">
      <img src={product.image} alt={product.name} />
      <h3>{product.name}</h3>
      <p>${product.price}</p>
      <button onClick={() => handleButtonClick(product.id)}>Add to Cart</button>
    </div>
  );
});
