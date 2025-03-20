import axios from "axios";
import data from "../../dataJSON/data.json";

export default async function AddProductToCart(userId, productId) {
  try {
    const response = await axios.post(
      data.localhost + `/cart/post/${userId}/items/${productId}`,
      {
        headers: {
          "Content-Type": "application/json", // Обов'язковий заголовок для JSON
        },
      }
    );

    if (response.status == 200) {
      return true;
    } else {
      return false;
    }
  } catch (error) {
    console.log(error.message);
  }
}
