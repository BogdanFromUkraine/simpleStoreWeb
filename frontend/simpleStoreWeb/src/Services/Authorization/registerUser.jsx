import axios from "axios";
import data from "../../dataJSON/data.json";

export default async function RegisterUser(userName, email, password) {
  try {
    const response = await axios.post(
      data.localhost + "/auth/register",
      {
        userName: userName,
        email: email,
        password: password,
      },
      {
        headers: {
          "Content-Type": "application/json", // Обов'язковий заголовок для JSON
        },
      }
    );

    return true;
  } catch (error) {
    return false;
  }
}
