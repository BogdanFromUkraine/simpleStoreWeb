import { observer } from "mobx-react-lite";
import React from "react";
import { useStores } from "../store/root-store-context.ts";
import getUserId from "./utils/getUserId.jsx";

export const ProductCard = observer(({ product }) => {
  const { dataStore, notificationStore } = useStores();

  async function handleButtonClick(id) {
    await dataStore.add_Product_To_Cart(await getUserId(), id);
    //логіка toastr
    if (dataStore.response == true) {
      notificationStore.notify("successfully added!", "success");
    } else {
      notificationStore.notify("something wrong!", "error");
    }
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
