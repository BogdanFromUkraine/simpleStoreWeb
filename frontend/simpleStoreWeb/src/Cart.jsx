import React, { useState, useEffect } from "react";
import style from "./style/cart.module.css";

import { observer } from "mobx-react-lite";
import { useStores } from "../store/root-store-context";
import { toJS } from "mobx";
import getUserId from "./utils/getUserId.jsx";

export const Cart = observer(() => {
  const [cartItems, setCartItems] = useState([]);
  const [userIdToken, setUserIdToken] = useState();

  const { get_User_Cart, ItemsOfCart, remove_Product_From_Cart } = useStores();

  useEffect(() => {
    GetCartItem();

    async function GetCartItem() {
      await get_User_Cart(await getUserId());
      setCartItems(toJS(ItemsOfCart));
    }
    console.log(toJS(ItemsOfCart));
  }, [cartItems]);

  // Функція для оновлення кількості
  const updateQuantity = (id, delta) => {
    setCartItems((prevItems) =>
      prevItems.map((item) =>
        item.id === id
          ? {
              ...item,
              stock: Math.max(1, item.stock + delta), // Мінімальна кількість - 1
            }
          : item
      )
    );
  };

  // Функція для видалення товару
  const removeItem = async (id) => {
    await remove_Product_From_Cart(await getUserId(), id);
    setCartItems(ItemsOfCart);
  };

  // Підрахунок загальної вартості
  const totalPrice = cartItems.reduce(
    (total, item) => total + item.price * item.stock,
    0
  );

  return (
    <div className={style.cart_container}>
      <h1>Shopping Cart</h1>
      {cartItems.length === 0 ? (
        <p>Your cart is empty!</p>
      ) : (
        <table className={style.cart_table}>
          <thead>
            <tr>
              <th>Product</th>
              <th>Price</th>
              <th>Quantity</th>
              <th>Total</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {cartItems.map((item) => (
              <tr key={item.id}>
                <td>{item.name}</td>
                <td>${item.price.toFixed(2)}</td>
                <td>
                  <button
                    onClick={() => updateQuantity(item.id, -1)}
                    className={style.quantity_btn}
                  >
                    -
                  </button>
                  <span>{item.stock}</span>
                  <button
                    onClick={() => updateQuantity(item.id, 1)}
                    className={style.quantity_btn}
                  >
                    +
                  </button>
                </td>
                <td>${(item.price * item.stock).toFixed(2)}</td>
                <td>
                  <button
                    onClick={() => removeItem(item.id)}
                    className={style.remove_btn}
                  >
                    Remove
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      <h2>Total: ${totalPrice.toFixed(2)}</h2>
    </div>
  );
});
