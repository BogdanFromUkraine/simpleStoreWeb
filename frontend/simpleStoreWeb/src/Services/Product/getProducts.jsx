import axios from "axios";
import data from "../../dataJSON/data.json";

export default async function GetProducts() {
  try {
    const response = await axios.get(data.localhost + "/products", {
      headers: {
        "Content-Type": "application/json", // Обов'язковий заголовок для JSON
      },
    });

    return response.data;
  } catch (error) {
    console.log(error.message);
  }
}
