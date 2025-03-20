import axios from "axios";
import data from "../../dataJSON/data.json";

export default async function AddProductFunc(name, description, price, stock) {
  try {
    const jwtToken = localStorage.getItem("token");

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
          "Content-Type": "application/json",
          Authorization: `Bearer ${jwtToken}`,
        },
        withCredentials: true,
      }
    );

    if (response.status == 200) {
      return true;
    } else {
      return false;
    }
  } catch (error) {
    console.log(error);
  }

  //   const jwtToken = localStorage.getItem("token");
  //   const a = axios
  //     .get("https://localhost:7240/api/health", {
  //       headers: {
  //         Authorization: `Bearer ${jwtToken}`,
  //       },
  //     })
  //     .then((response) => console.log(response))
  //     .catch((error) => console.log(error));

  //   return a;
}
