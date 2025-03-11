import axios from "axios";
import data from "../../dataJSON/data.json";

export default async function RemoveProductFromCart(userId, productId) {
  try {
    const response = await axios.delete(
      data.localhost + `/cart/delete/${userId}/items/${productId}`,
      {
        headers: {
          "Content-Type": "application/json", // Обов'язковий заголовок для JSON
        },
      }
    );

    return response.data;
  } catch (error) {
    console.log(error.message);
  }
}
