import axios from "axios";
import data from "../../dataJSON/data.json";

export default async function AddProductFunc(name, description, price, stock) {
  try {
    const response = await axios.post(
      data.localhost + "/products",
      {
        Name: name,
        Description: description,
        Price: price,
        Stock: stock,
      },
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
