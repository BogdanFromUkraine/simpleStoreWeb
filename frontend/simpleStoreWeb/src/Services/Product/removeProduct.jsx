import axios from "axios";
import data from "../../dataJSON/data.json";

export default async function RemoveProduct(name) {
  try {
    const jwtToken = localStorage.getItem("token");
    const response = await axios.delete(data.localhost + `/products/${name}`, {
      headers: {
        "Content-Type": "application/json", // Обов'язковий заголовок для JSON
        Authorization: `Bearer ${jwtToken}`,
      },
    });

    if (response.status == 200) {
      return true;
    } else {
      return false;
    }
  } catch (error) {
    console.log(error.message);
  }
}
