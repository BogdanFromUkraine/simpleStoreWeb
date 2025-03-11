import axios from "axios";
import data from "../../dataJSON/data.json";

export default async function GetUserCart(userId) {
  try {
    const response = await axios.get(data.localhost + `/cart/get/${userId}`, {
      headers: {
        "Content-Type": "application/json", // Обов'язковий заголовок для JSON
      },
    });

    return response.data;
  } catch (error) {
    console.log(error.message);
  }
}
